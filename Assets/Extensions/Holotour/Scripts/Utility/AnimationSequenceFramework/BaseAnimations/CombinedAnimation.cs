using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CombinedAnimation : MonoPlayable {

    [SerializeField]
    [Header("Sub Animation")]
    private MonoPlayable delegateAnimation;

    [Header("Animation Control")]
    [SerializeField]
    private float animationStartDelay = 0;
    [SerializeField]
    protected Color gizmoColor = Color.white;

    [Header("Avatar Control")]
    [SerializeField]
    private Transform avatarPositionDuringThisAnimation;
    [SerializeField]
    private float movementSpeed = 1;

    private AvatarAgent avatarAgent;

    public override float Duration
    { get { return delegateAnimation.Duration; } protected set { } }

    public override bool IsPlaying
    { get { return delegateAnimation.IsPlaying; } protected set { } }

    public override bool StoppedPlaying
    { get { return delegateAnimation.StoppedPlaying; } protected set { } }

    public override float TimeStarted
    { get { return delegateAnimation.TimeStarted; } protected set { } }

    private float timeToStartAnimation;
    private bool started;
    private bool wasPlaying;

    public override void Update()
    {
        if (!Application.isPlaying && avatarPositionDuringThisAnimation != null)
        {
            Vector3 pos = avatarPositionDuringThisAnimation.position;
            Debug.DrawLine(pos - Vector3.up * 0.01f, pos + Vector3.up * 0.01f, gizmoColor);
            Debug.DrawLine(pos - Vector3.forward * 0.01f, pos + Vector3.forward * 0.01f, gizmoColor);
            Debug.DrawLine(pos - Vector3.right * 0.01f, pos + Vector3.right * 0.01f, gizmoColor);
        }

        if (Application.isPlaying)
        {
            if (IsPlaying)
            {
                MoveAvatarPosition();
                if (Time.time >= timeToStartAnimation)
                {
                    if (!started)
                    {
                        started = true;
                        OnAnimationStarted();
                    }
                    OnAnimationRunning();
                }
            }
            StoppedPlaying = (wasPlaying == true && IsPlaying == false);
            wasPlaying = IsPlaying;
            if (StoppedPlaying)
            {
                OnAnimationEnded();
            }
        }
        
    }

    private void MoveAvatarPosition()
    {
        if (avatarPositionDuringThisAnimation != null && avatarAgent != null)
        {
            avatarAgent.transform.position = Vector3.Lerp(avatarAgent.transform.position, avatarPositionDuringThisAnimation.position, movementSpeed * Time.deltaTime);
        }
    }

    public override bool Play()
    {
        if (delegateAnimation.Play())
        {
            avatarAgent = GameObject.FindObjectOfType<AvatarAgent>();
            started = false;
            timeToStartAnimation = Time.time + animationStartDelay;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Stop()
    {
        return delegateAnimation.Stop();
    }

    public override void Prepare()
    {
        delegateAnimation.Prepare();
    }

    public override void Free()
    {
        delegateAnimation.Free();
    }

    protected virtual void OnAnimationStarted() { }
    protected virtual void OnAnimationRunning() { }
    protected virtual void OnAnimationEnded() { }

}
