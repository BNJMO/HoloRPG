using System;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : HoloToolkit.Unity.Singleton<AvatarManager>, IKeywordCommandProvider
{

	public event Action OnTourStarted;

	public event Action OnTourEnded;

    [HideInInspector] public bool IsMoving { get { return avatarAnimator != null && avatarAnimator.GetBool("isMoving"); } }
    [HideInInspector] public bool IsPresenting { get { return stateMachine != null && stateMachine.IsCurrentState<PresentingState>(); } }

    public bool IsTourRunning {
		get { 
			return stateMachine != null && !stateMachine.IsCurrentState<InitialState> ();
		}
	}

    private bool showStates = false;

    [SerializeField]
    private float minMoveSpeed = 1;
    public float MinMoveSpeed { get { return minMoveSpeed; } }

    [SerializeField]
    private float maxMoveSpeed = 2;
    public float MaxMoveSpeed { get { return maxMoveSpeed; } }

    [SerializeField]
    private float rotationSpeed = 2;
    public float RotationSpeed { get { return rotationSpeed; } }


    [Header("Other Settings")]
    [SerializeField]
    public float anchorRange = 0.4f;
    public float AnchorRange { get { return anchorRange; } }

    [SerializeField]
    public float poiRange = 0.4f;
    public float PoiRange { get { return poiRange; } }

    [SerializeField]
    private float waitUserRange = 3f;
    public float WaitUserRange { get { return waitUserRange; } }

    [SerializeField]
    private float followUserRange = 4.5f;
    public float FollowUserRange { get { return followUserRange; } }

    [SerializeField]
    private float safetyRange = 2;
    public float SafetyRange { get { return safetyRange; } }


    [Header ("Avatar")]
	[SerializeField]
	private AvatarAgent avatar;
    public AvatarAgent Avatar
    { get { return avatar; } }

    private Route route;
    public Route CurrentRoute
    { get { return route; } }

    private Animator avatarAnimator;
    public Animator AvatarAnimator
    { get { return avatarAnimator; } }

    private Transform userTransform;
    public Transform UserTransform
    { get { return userTransform; } }

	private StateMachine<AnimationState, AnimationState.EAnimationEvent> stateMachine;

	void Start() {
        userTransform = Camera.main.transform;
        avatarAnimator = avatar.animator;

		KeywordCommandManager.Instance.AddKeywordCommandProvider (this);
	}

	void Update()
	{
        if (stateMachine != null)
        {
            stateMachine.Update();
        }
	}


    private void StartTour()
    {
        InitStateMachine();
        InitRoute();
        stateMachine.PostEvent(AnimationState.EAnimationEvent.USER_SAYS_HALLO);
    }

    private void InitRoute()
    {
        route = new Route(AnchorManager.Instance.AnchorList);
    }

	private void InitStateMachine() {
		if (stateMachine != null) {
			stateMachine.OnStateChanged -= OnStateChanged;
		}
		stateMachine = new StateMachine<AnimationState, AnimationState.EAnimationEvent> (new InitialState(this, GetComponent<PaulaSoundbank>()));
		stateMachine.OnStateChanged += OnStateChanged;
	}

    private void OnStateChanged(AnimationState oldState, AnimationState newState)
    {
        if (oldState.Is<InitialState>())
        {
            if (OnTourStarted != null)
            {
                OnTourStarted.Invoke();
            }
        }
        else if (newState.Is<InitialState>())
        {
            if (OnTourEnded != null)
            {
                OnTourEnded.Invoke();
            }
        }
        if (showStates)
        {
            Notify.Show(newState.ToString());
        }
    }

    private void OnWelcome()
	{
        if (!IsTourRunning)
        {
            StartTour();
        }
	}
    
    public void OnAppStartTriggered()
    {
        OnWelcome();
    }	

	private void OnNein()
	{
		stateMachine.PostEvent(AnimationState.EAnimationEvent.USER_SAYS_NO);
	}

	private void OnJa()
	{
		stateMachine.PostEvent(AnimationState.EAnimationEvent.USER_SAY_YES);
	}

	public List<KeywordCommand> GetSpeechCommands() {

        Condition ifAvatarIsListeningForCommands = Condition.New(() =>
        {
            return stateMachine != null && 
            stateMachine.IsCurrentState<EnforcementQuestionState>() && 
            (stateMachine.CurrentState as EnforcementQuestionState).IsListeningForCommands;
        });

		return new List<KeywordCommand> {
			new KeywordCommand(() => { OnWelcome(); }, "Hallo", KeyCode.H),
            new KeywordCommand(() => { OnWelcome(); }, "Hello", KeyCode.H),
            new KeywordCommand(() => { OnNein(); }, ifAvatarIsListeningForCommands, "Nine", KeyCode.N),
			new KeywordCommand(() => { OnJa(); }, ifAvatarIsListeningForCommands, "Yaa", KeyCode.J),
            new KeywordCommand(() => { OnJa(); }, ifAvatarIsListeningForCommands, "Ok", KeyCode.J),
            new KeywordCommand(() => { OnJa(); }, ifAvatarIsListeningForCommands, "Yo", KeyCode.J),
            new KeywordCommand(() => { showStates = true; }, "Show States", KeyCode.W),
            new KeywordCommand(() => { stateMachine.PostEvent(AnimationState.EAnimationEvent.ANIMATION_STOPPED); avatar.GetComponent<AudioSource>().Stop(); }, "Shut up", KeyCode.T) 
        };
	}

    public void StartNavigationHome(bool shortestWay)
    {
        if (route != null)
        {
            route = route.GetWayHome(shortestWay);
        }
    }
}