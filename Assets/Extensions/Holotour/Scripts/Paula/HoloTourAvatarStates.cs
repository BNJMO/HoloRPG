using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class for animation states.
/// </summary>
public abstract class AnimationState : IState<AnimationState, AnimationState.EAnimationEvent>
{
    public enum EAnimationEvent
    {
        USER_SAYS_HALLO, USER_SAY_YES, USER_SAYS_NO, ANIMATION_STOPPED
    }

    public readonly string ANIMATION_TRIGGER_IS_MOVING = "isMoving";
    public readonly string ANIMATION_IDLE = "Idle";
    public readonly string ANIMATION_WELCOME = "Welcome";
    public readonly string ANIMATION_ACCEPTED = "Accept";
    public readonly string ANIMATION_CONFIRM_CANCEL = "Confirm";
    public readonly string ANIMATION_CANEL_CONFIRMED = "Cancel";
    public readonly string ANIMATION_ENFORCEMENT = "Enforcement";
    public readonly string ANIMATION_HELLO_IM_HERE = "HelloImHere";
    public readonly string ANIMATION_TOUR_CONTINUE_ACCEPTED = "TourContinueAccepted";
    public readonly string ANIMATION_CONFIRM_CANCEL_ONGOING_TOUR = "CancelOngoingTour";
    public readonly string ANIMATION_CHANGED_MIND_TOUR_CONTINUE_ACCEPTED = "ChangedMindTourContinueAccepted";
    public readonly string ANIMATION_ONGOING_TOUR_CANCELED = "OngoingTourCancelled";
    public readonly string ANIMATION_READY_FOR_NEXT_HIGHLIGHT = "ReadyForNextHighlight";
    public readonly string ANIMATION_TOUR_OVER = "TourOver";
    public readonly string ANIMATION_GOODBYE = "Goodbye";

    protected AvatarManager AvatarManager
    { get; private set; }

    protected Animator Animator
    { get; private set; }

    protected PaulaSoundbank Sounds
    { get; private set; }

    private string TAG_END = "end_of_animation";
    private string TAG_AUDIO = "play_audio";

    private int lastAnimationStateHash = 0;


    public AnimationState(AvatarManager avatarManager, PaulaSoundbank sounds)
    {
        AvatarManager = avatarManager;
        Animator = avatarManager.AvatarAnimator;
        Sounds = sounds;
    }

    public virtual AnimationState Update()
    {
        AnimationState resultState = this;
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
        int currentAnimationStateHash = info.fullPathHash;

        if (currentAnimationStateHash != lastAnimationStateHash)
        {
            if (info.IsTag(TAG_END))
            {
                resultState = HandleEvent(EAnimationEvent.ANIMATION_STOPPED);
            }
            else if (info.IsTag(TAG_AUDIO))
            {
                OnPlayAudioTrigger();
            }
        }

        lastAnimationStateHash = currentAnimationStateHash;
        return resultState;
    }

    public bool Is<T>()
    {
        return typeof(T) == GetType();
    }

    public abstract AnimationState HandleEvent(EAnimationEvent e);
    public abstract void OnStateEnter();

    public virtual void OnPlayAudioTrigger()
    {
    }
}

/// <summary>
/// base class for cutscene linke animation states. This states ignores all user input until the 
/// animation of the avatar finishes.
/// </summary>
public abstract class LockedAnimationState : AnimationState
{
    protected AudioClip[] MyAudioClips
    { get; set; }

    private int audioTriggerCallCounter = 0;
    private bool animationAlreadyFinished;

    public LockedAnimationState(AvatarManager avatarManager, PaulaSoundbank sounds, AudioClip[] myClips) : base(avatarManager, sounds)
    {
        MyAudioClips = myClips;
    }

    public sealed override AnimationState HandleEvent(EAnimationEvent e)
    {
        if (e == EAnimationEvent.ANIMATION_STOPPED)
        {
            UnlockEventHandler();
            Debug.Log("Anim finished: " + this.GetType());
            Animator.Play(ANIMATION_IDLE, 0);
            return OnAnimationFinished();
        }
        if (animationAlreadyFinished)
        {
            return HandleEventAfterAnimationFinished(e);
        }
        else
        {
            return this;
        }
    }

    protected void UnlockEventHandler()
    {
        animationAlreadyFinished = true;
    }

    protected void LockEventHandler()
    {
        animationAlreadyFinished = false;
    }

    protected virtual AnimationState OnAnimationFinished()
    {
        return this;
    }

    public override void OnPlayAudioTrigger()
    {
        if (MyAudioClips != null)
        {
            Debug.Log("Play Audio Snippet  " + audioTriggerCallCounter);
            Sounds.PlayNextTrack(MyAudioClips[audioTriggerCallCounter++]);
        }
    }

    protected virtual AnimationState HandleEventAfterAnimationFinished(EAnimationEvent e)
    {
        return this;
    }
}

/// <summary>
/// Plays the given animation and redirects to a enforcement state. The enforcement state uses the given enforcement audio tracks.
/// </summary>
public class YesNoQuestionWithEnforcementState : LockedAnimationState
{
    private string animationToPlay;
    private AnimationState yesState;
    private AnimationState noState;
    private AudioClip[] enforcementAudioTracks;


    public YesNoQuestionWithEnforcementState(AvatarManager avatarManager, PaulaSoundbank sounds, AudioClip[] questionAudioTracks, AudioClip[] enforcementAudioTracks, string animationToPlay, AnimationState yesState, AnimationState noState)
        : base(avatarManager, sounds, questionAudioTracks)
    {
        this.animationToPlay = animationToPlay;
        this.yesState = yesState;
        this.noState = noState;
        this.enforcementAudioTracks = enforcementAudioTracks;
    }

    protected override AnimationState OnAnimationFinished()
    {
        return new EnforcementQuestionState(
            AvatarManager,
            Sounds,
            enforcementAudioTracks,
            10,
            yesState,
            noState);
    }

    public override void OnStateEnter()
    {
        Animator.SetTrigger(animationToPlay);
    }
}

/// <summary>
/// Waits a defined couple of second to let the user time to respond the question. After timeout, the enforcement animation is played.
/// This state happens in a loop until the user answers the question.
/// </summary>
public class EnforcementQuestionState : LockedAnimationState
{
    private AnimationState yesState;
    private AnimationState noState;
    private float timeoutToEnforce;
    private float timeToWaitUntilEnforce = float.PositiveInfinity;
    private bool isListeningForCommands;
    public bool IsListeningForCommands
    {
        get { return isListeningForCommands; }
        private set {
            isListeningForCommands = value;
            AvatarManager.Avatar.ShowConversationHint(value);
        }
    }

    public EnforcementQuestionState(AvatarManager avatarManager, PaulaSoundbank sounds, AudioClip[] MyAudioClips, float timeoutToEnforce, AnimationState yesState, AnimationState noState) : base(avatarManager, sounds, MyAudioClips)
    {
        this.yesState = yesState;
        this.noState = noState;
        this.timeoutToEnforce = timeoutToEnforce;
    }

    public override AnimationState Update()
    {
        if (Time.time > timeToWaitUntilEnforce)
        {

            IsListeningForCommands = false;
            LockEventHandler();
            Animator.SetTrigger(ANIMATION_ENFORCEMENT);
            timeToWaitUntilEnforce = float.PositiveInfinity;
        }
        return base.Update();
    }

    protected override AnimationState HandleEventAfterAnimationFinished(EAnimationEvent e)
    {
        if (e == EAnimationEvent.USER_SAY_YES)
        {
            IsListeningForCommands = false;
            return yesState;
        }
        if (e == EAnimationEvent.USER_SAYS_NO)
        {
            IsListeningForCommands = false;
            return noState;
        }
        return this;
    }

    protected override AnimationState OnAnimationFinished()
    {
        return new EnforcementQuestionState(
            AvatarManager,
            Sounds,
            MyAudioClips,
            timeoutToEnforce,
            yesState,
            noState);
    }

    public override void OnStateEnter()
    {
        AvatarManager.Avatar.LookeAt(AvatarManager.UserTransform, false);
        timeToWaitUntilEnforce = Time.time + timeoutToEnforce;
        UnlockEventHandler();
        IsListeningForCommands = true;
    }
}

/// <summary>
/// Initial state. Paula is waiting for a nice "hallo".
/// </summary>
public class InitialState : AnimationState
{
    public InitialState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds)
    { }

    public override AnimationState HandleEvent(EAnimationEvent e)
    {
        if (e == EAnimationEvent.USER_SAYS_HALLO)
        {
            AnimationState doYouReallyWandToCancelState = new YesNoQuestionWithEnforcementState( // nested question
                AvatarManager,
                Sounds,
                Sounds.confirmTracks,
                Sounds.confirmEnforcementTracks,
                ANIMATION_CONFIRM_CANCEL,
                new TourCancelledState(AvatarManager, Sounds),
                new TourAcceptedState(AvatarManager, Sounds));

            return new YesNoQuestionWithEnforcementState(
                AvatarManager,
                Sounds,
                Sounds.welcomeTracks,
                Sounds.welcomeEnforcementTracks,
                ANIMATION_WELCOME,
                new TourAcceptedState(AvatarManager, Sounds),
                doYouReallyWandToCancelState); 
        }
        return this;
    }

    public override void OnStateEnter()
    {
        Animator.SetBool(ANIMATION_TRIGGER_IS_MOVING, false);
    }
}

/// <summary>
/// Paula is happy to start the tour and starts navigating.
/// </summary>
public class TourAcceptedState : LockedAnimationState
{
    public TourAcceptedState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds, sounds.acceptTracks)
    { }

    protected override AnimationState OnAnimationFinished()
    { return new DecideHowToProceedNavigationState(AvatarManager, Sounds); }

    public override void OnStateEnter()
    { Animator.SetTrigger(ANIMATION_ACCEPTED); }
}

/// <summary>
/// Paule says goodby to the user and goes idle.
/// </summary>
public class TourCancelledState : LockedAnimationState
{
    public TourCancelledState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds, sounds.cancelTrakcs)
    { }

    protected override AnimationState OnAnimationFinished()
    {
        return new InitialState(AvatarManager, Sounds);
    }

    public override void OnStateEnter()
    {
        Animator.SetTrigger(ANIMATION_CANEL_CONFIRMED);
    }

}

public abstract class NavigationState : AnimationState
{
    public NavigationState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds)
    {
    }

    #region RangeCheck

    /// <summary>
    /// Checks if we are within range of an Anchor (set by anchorRange)
    /// </summary>
    /// <returns>True, if the distance between Paula and the next Destination is less than anchorRange.</returns>
    protected bool InAnchorRange()
    {
        return Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.CurrentRoute.CurrentPosition) < AvatarManager.AnchorRange;
    }

    /// <summary>
    /// Checks if we are within range of an Anchor (set by anchorRange)
    /// </summary>
    /// <returns>True, if the distance between Paula and the next Destination is less than anchorRange.</returns>
    protected bool InPoiRange()
    {
        return Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.CurrentRoute.CurrentPosition) < AvatarManager.PoiRange;
    }

    /// <summary>
    /// Checks if paula is close enough to the user to follow.
    /// </summary>
    /// <returns>True if the distance between paula and the user is less than FollowUuserRange (nice typo bruh)</returns>
    protected bool InFollowUserRange()
    {
        return Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.UserTransform.position) < AvatarManager.FollowUserRange;
    }


    /// <summary>
    /// Checks if paula is close enough to the user to follow. Uses a hysteresis with 80% to prevent state flickering.
    /// </summary>
    /// <returns>True if the distance between paula and the user is less than FollowUuserRange (nice typo bruh)</returns>
    protected bool InContinueFollowingUserRange()
    {
        return Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.UserTransform.position) < AvatarManager.FollowUserRange * 0.8f;
    }

    /// <summary>
    /// Checks if paula should wait for the user to catch up.
    /// </summary>
    /// <returns>True if the distance between paula and the user is less than waitUserRange.</returns>
    protected bool InWaitUserRange()
    {
        return Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.UserTransform.position) < AvatarManager.WaitUserRange;
    }

    protected bool InSafetyRange()
    {
        return Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.UserTransform.position) < AvatarManager.SafetyRange;
    }
    #endregion
}

/// <summary>
/// Decides how the navigation proceeds, depending on the users proximity or the distance to the next waypoint or poi.
/// </summary>
public class DecideHowToProceedNavigationState : NavigationState
{
    public DecideHowToProceedNavigationState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds)
    { }

    public override AnimationState Update()
    {
        base.Update();

        if (!InFollowUserRange())
        {
            return new HalfwayWaitingState(AvatarManager, Sounds);
        }

        if ((InPoiRange()) && (AvatarManager.CurrentRoute.CurrentIsPoi))
        {
             return new PresentingState(AvatarManager, Sounds);
        }
        else if ((InAnchorRange()) && (AvatarManager.CurrentRoute.CurrentIsLast))
        {
            return new SayGoodbyeState(AvatarManager, Sounds);
        }
        
        return new FlyingToPoiState(AvatarManager, Sounds);
    }

    public override void OnStateEnter()
    {
        Animator.SetBool(ANIMATION_TRIGGER_IS_MOVING, false);
        AvatarManager.Avatar.LookeAt(AvatarManager.UserTransform, false);
    }

    public override AnimationState HandleEvent(EAnimationEvent e)
    {
        return this;
    }
}


/// <summary>
/// Paula waits until the user comes near.
/// </summary>
public class HalfwayWaitingState : NavigationState
{
    protected AudioClip[] MyAudioClips
    { get; set; }

    private float timeUntilAvatarEnforcesUser;
    private float minimalTimeUntilEnforcement;
    private float maximalTimeUntilEnforcement;

    private static int audioClipIndexIncrementor = 0;

    public HalfwayWaitingState(AvatarManager avatarManager, PaulaSoundbank sounds, float minimalTimeUntilEnforcement, float maximalTimeUntilEnforcement) : base(avatarManager, sounds)
    {
        MyAudioClips = sounds.halfwayWaitingTracks;
        this.minimalTimeUntilEnforcement = minimalTimeUntilEnforcement;
        this.maximalTimeUntilEnforcement = Mathf.Max(minimalTimeUntilEnforcement, maximalTimeUntilEnforcement);
    }

    public HalfwayWaitingState(AvatarManager avatarManager, PaulaSoundbank sounds) : this(avatarManager, sounds, 2, 5)
    {
    }

    public override AnimationState Update()
    {
        base.Update();

        if (InContinueFollowingUserRange())
        {
            return new DecideHowToProceedNavigationState(AvatarManager, Sounds);
        }
        if (Time.time > timeUntilAvatarEnforcesUser)
        {
            Animator.SetTrigger(ANIMATION_HELLO_IM_HERE);
            return new HalfwayWaitingState(AvatarManager, Sounds, 5, 10);
        }
        return this;
    }

    public override void OnStateEnter()
    {
        Animator.SetBool(ANIMATION_TRIGGER_IS_MOVING, false);
        AvatarManager.Avatar.LookeAt(AvatarManager.UserTransform, false);
        SetTimer();
    }

    private void SetTimer()
    {
        timeUntilAvatarEnforcesUser = Time.time + minimalTimeUntilEnforcement + UnityEngine.Random.value * (maximalTimeUntilEnforcement - minimalTimeUntilEnforcement);
    }

    public override void OnPlayAudioTrigger()
    {
        if (MyAudioClips != null)
        {
            Sounds.PlayNextTrack(MyAudioClips[audioClipIndexIncrementor]);
            audioClipIndexIncrementor = (audioClipIndexIncrementor + 1) % MyAudioClips.Length;
        }
    }

    public override AnimationState HandleEvent(EAnimationEvent e)
    {
        if (e == EAnimationEvent.ANIMATION_STOPPED)
        {
            Animator.Play(ANIMATION_IDLE, 0);
        }
        return this;
    }
}

/// <summary>
/// Paula flys to the next product.
/// </summary>
public class FlyingToPoiState : NavigationState
{
    private Vector3 paulaPosition;

    public FlyingToPoiState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds)
    { }

    public override AnimationState Update()
    {
        base.Update();

        // Move Paula towards the next Anchor
        // check first if she is in the safety range of the user
        Vector3 safetyOffset = Vector3.zero;
        if (InSafetyRange())
        {
            float relativeDistance = 1 - (Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.UserTransform.position) / AvatarManager.SafetyRange);
            safetyOffset = AvatarManager.Avatar.transform.right * relativeDistance * 3.3f;
        }
        // calculate movement speed
        float movementSpeed = AvatarManager.MinMoveSpeed + (1 - (Vector3.Distance(AvatarManager.Avatar.transform.position, AvatarManager.UserTransform.position) / AvatarManager.FollowUserRange));

        if (movementSpeed > AvatarManager.MaxMoveSpeed)
        {
            movementSpeed = AvatarManager.MaxMoveSpeed;
        }
        else if (movementSpeed < AvatarManager.MinMoveSpeed)
        {
            movementSpeed = AvatarManager.MinMoveSpeed;
        }
        // update paula's relative position to the anchors without the safety offset
        paulaPosition = Vector3.MoveTowards
            (
            paulaPosition,
            AvatarManager.CurrentRoute.CurrentPosition,
            Time.deltaTime * movementSpeed
            );
        // apply the safety offset to Paula's position
        AvatarManager.Avatar.transform.position = Vector3.Lerp
            (
            AvatarManager.Avatar.transform.position,
            paulaPosition + safetyOffset,
            Time.deltaTime * movementSpeed
            );
        // update Paula's rotation
        AvatarManager.Avatar.transform.rotation = Quaternion.Lerp
            (
            AvatarManager.Avatar.transform.rotation,
            Quaternion.LookRotation((AvatarManager.Avatar.transform.position - AvatarManager.CurrentRoute.CurrentPosition).normalized, Vector3.up),
            Time.deltaTime * AvatarManager.RotationSpeed
            );

        if (InFollowUserRange() == false)
        {
            return new DecideHowToProceedNavigationState(AvatarManager, Sounds);
        }

    
        if (((AvatarManager.CurrentRoute.CurrentIsPoi) && (InPoiRange())) 
        || ((AvatarManager.CurrentRoute.CurrentIsLast) && (InAnchorRange())))
        {
            return new DecideHowToProceedNavigationState(AvatarManager, Sounds);
        }
        else if (InAnchorRange())
        {
            AvatarManager.CurrentRoute.Next();
        }
        
        return this;
    }

    public override void OnStateEnter()
    {
        AvatarManager.Avatar.LookeAt(null, false);
        Animator.Play(ANIMATION_IDLE, 0);
        Animator.SetBool(ANIMATION_TRIGGER_IS_MOVING, true);
        paulaPosition = AvatarManager.Avatar.transform.position;
    }

    public override AnimationState HandleEvent(EAnimationEvent e)
    {
        return this;
    }
}

/// <summary>
/// Paula presents the product.
/// </summary>
public class PresentingState : AnimationState
{
    private AnimationSequencePlayer player;
    public PresentingState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds)
    { }

    public override AnimationState Update()
    {
        if (player != null && player.IsPlaying)
        {
            return this;
        }
        else
        {
            IAnchor lastAnchor = AvatarManager.CurrentRoute.Current;
            AvatarManager.CurrentRoute.Next();
            if (!(lastAnchor is EntertainmentAnchor) && AvatarManager.CurrentRoute.RemainingProductsCount == 0)
            {
                return new TourIsOverState(AvatarManager, Sounds);
            }
            if (lastAnchor is PoiAnchor && (lastAnchor as PoiAnchor).NeedsConfirmationToProceed)
            {
                AnimationState doYouReallyWandToCancelOngoingTourState = new YesNoQuestionWithEnforcementState( // nested question
                    AvatarManager,
                    Sounds,
                    Sounds.confirmTracks,
                    Sounds.confirmEnforcementTracks,
                    ANIMATION_CONFIRM_CANCEL_ONGOING_TOUR,
                    new OngoingTourCancelledState(AvatarManager, Sounds),
                    new ChangedMindTourContinueAcceptedState(AvatarManager, Sounds)
                );

                bool lastProduct = AvatarManager.CurrentRoute.RemainingProductsCount == 1;

                return new YesNoQuestionWithEnforcementState(
                    AvatarManager,
                    Sounds,
                    lastProduct ? Sounds.lastHighlightTracks : Sounds.nextHighlightTracks,
                    lastProduct ? Sounds.lastHighlightEnforcementTracks : Sounds.nextHighlightEnforcementTracks,
                    ANIMATION_READY_FOR_NEXT_HIGHLIGHT,
                    new TourContinueAcceptedState(AvatarManager, Sounds),
                    doYouReallyWandToCancelOngoingTourState);
            }
            else
            {
                return new FlyingToPoiState(AvatarManager, Sounds);
            }
        }
    }

    public override AnimationState HandleEvent(EAnimationEvent e)
    {
        return this;
    }

    public override void OnStateEnter()
    {
        player = AvatarManager.CurrentRoute.Current.GameObject.GetComponent<AnimationSequencePlayer>();
        if (player != null)
        {
            player.Play();
        }
        Animator.SetBool(ANIMATION_TRIGGER_IS_MOVING, false);
        AvatarManager.Avatar.LookeAt(AvatarManager.UserTransform, false);
    }
}

/// <summary>
/// Paule is glad that the user continues the tour and resumes navigation tho the next product.
/// </summary>
public class ChangedMindTourContinueAcceptedState : LockedAnimationState
{
    public ChangedMindTourContinueAcceptedState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds, sounds.changedMindAcceptedProceedingTracks)
    { }

    protected override AnimationState OnAnimationFinished()
    {
        return new FlyingToPoiState(AvatarManager, Sounds);
    }

    public override void OnStateEnter()
    {
        Animator.SetTrigger(ANIMATION_CHANGED_MIND_TOUR_CONTINUE_ACCEPTED);
    }
}

/// <summary>
/// Paule is glad that the user continues the tour and resumes navigation tho the next product.
/// </summary>
public class TourContinueAcceptedState : LockedAnimationState
{
    public TourContinueAcceptedState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds, sounds.tourContinueAcceptedTracks)
    { }

    protected override AnimationState OnAnimationFinished()
    {
        return new FlyingToPoiState(AvatarManager, Sounds);
    }

    public override void OnStateEnter()
    {
        Animator.SetTrigger(ANIMATION_TOUR_CONTINUE_ACCEPTED);
    }
}

/// <summary>
/// Paula says that the tour is cancelled and navigates to the start point.
/// </summary>
public class OngoingTourCancelledState : LockedAnimationState
{
    public OngoingTourCancelledState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds, sounds.tourIsCancelledTracks)
    { }

    protected override AnimationState OnAnimationFinished()
    {
        AvatarManager.StartNavigationHome(true);
        return new FlyingToPoiState(AvatarManager, Sounds);
    }

    public override void OnStateEnter()
    {
        Animator.SetTrigger(ANIMATION_ONGOING_TOUR_CANCELED);
    }

}

/// <summary>
/// Paula says that the tour is over and navigates to the start point.
/// </summary>
public class TourIsOverState : LockedAnimationState
{
    public TourIsOverState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds, sounds.tourIsOverTracks)
    { }

    protected override AnimationState OnAnimationFinished()
    {
        AvatarManager.StartNavigationHome(false);
        return new FlyingToPoiState(AvatarManager, Sounds);
    }

    public override void OnStateEnter()
    {
        Animator.SetTrigger(ANIMATION_TOUR_OVER);
    }
}

/// <summary>
/// Paula says goodbye and goes to the initial state.
/// </summary>
public class SayGoodbyeState : LockedAnimationState
{
    public SayGoodbyeState(AvatarManager avatarManager, PaulaSoundbank sounds) : base(avatarManager, sounds, sounds.goodbyeTracks)
    { }

    protected override AnimationState OnAnimationFinished()
    {
        return new InitialState(AvatarManager, Sounds);
    }

    public override void OnStateEnter()
    {
        Animator.SetBool(ANIMATION_TRIGGER_IS_MOVING, false);
        Animator.SetTrigger(ANIMATION_GOODBYE);
    }
}