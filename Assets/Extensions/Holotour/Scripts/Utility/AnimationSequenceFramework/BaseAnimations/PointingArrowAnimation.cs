using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingArrowAnimation : CombinedAnimation
{

    private static Transform arrowPrefab;

    [Header("Pointing Animation")]

    [SerializeField]
    private Transform arrowPointTarget;
    [SerializeField]
    private Transform arrowAnimationExtend;
    [Tooltip ("Used to move the extend to this transform over time. Leave it free if a simple sin animation is useed.")]
    [SerializeField]
    private Transform arrowExtendEnd;
    [Tooltip ("Used to move the target to this transform over time. Leave it free if a simple sin animation is useed.")]
    [SerializeField]
    private Transform arrowTargetEnd;
    [SerializeField]
    private float arrowPointFrequence = 1f;
    [SerializeField]
    private float extentTransitionSpeed = 1f;
    [SerializeField]
    private float targetTransitionSpeed = 1f;
    [SerializeField]
    private float autocancelPointingAnimationAfterSeconds = float.MaxValue;

    private bool autoCanceled = false;

    private float timeStarted = float.MinValue;
    private Transform arrow;
    private Transform arrowContainer;

    void Start()
    {
        if (Application.isPlaying) {
            if (arrowPrefab == null)
            {
                arrowPrefab = (Resources.Load("DummyArrow") as GameObject).transform;
            }
            if (arrowPointTarget != null && arrowAnimationExtend != null)
            {
                arrowContainer = new GameObject("Arrow Container").transform;
                arrowContainer.SetParent(arrowPointTarget, false);
                arrowContainer.localScale = Vector3.zero;
                arrow = Instantiate(arrowPrefab);
                arrow.SetParent(arrowContainer, false);
                
            }
        }
    }

    protected override void OnAnimationStarted() {
        AnimateThis.With(arrowContainer).CancelAll().
                    Transformate()
                    .FromScale(0)
                    .ToScale(1)
                    .Duration(3)
                    .Ease(AnimateThis.EaseOutQuintic)
                    .Start();

        autoCanceled = false;
        timeStarted = Time.time;
    }

    protected override void OnAnimationRunning() {
        if (Time.time < timeStarted + autocancelPointingAnimationAfterSeconds)
        {
            arrow.gameObject.SetActive(true);
            if (arrowExtendEnd != null)
            {
                arrowAnimationExtend.position = Vector3.Lerp(arrowAnimationExtend.position, arrowExtendEnd.position, Time.deltaTime * extentTransitionSpeed);
            }
            if (arrowTargetEnd != null)
            {
                arrowPointTarget.position = Vector3.Lerp(arrowPointTarget.position, arrowTargetEnd.position, Time.deltaTime * targetTransitionSpeed);
            }

            Vector3 animationPath = arrowAnimationExtend.position - arrowPointTarget.position;
            arrow.position = arrowPointTarget.position + animationPath.normalized * Mathf.Abs(Mathf.Sin(Time.time * Mathf.PI * arrowPointFrequence)) * animationPath.magnitude;
            arrow.rotation = Quaternion.LookRotation(arrowPointTarget.position - arrowAnimationExtend.position, Vector3.up);
        } else if (!autoCanceled)
        {
            OnAnimationEnded();
            autoCanceled = true;
        }

    }

    protected override void OnAnimationEnded() {
        if (!autoCanceled)
        {
            AnimateThis.With(arrowContainer).CancelAll().
                   Transformate()
                   .FromScale(1)
                   .ToScale(0)
                   .Duration(0.25f)
                   .Ease(AnimateThis.EaseInOutSmooth)
                   .Start();
        }
    }

    public override void Update()
    {
        base.Update();
        if (arrowPointTarget != null && arrowAnimationExtend != null)
        {
            if (!Application.isPlaying)
            {
                Debug.DrawRay(arrowPointTarget.position, arrowAnimationExtend.position - arrowPointTarget.position, gizmoColor);
                if (arrowExtendEnd != null) {
                    Debug.DrawRay(arrowExtendEnd.position, arrowAnimationExtend.position - arrowExtendEnd.position, Color.cyan);
                }
                if (arrowTargetEnd != null) {
                    Debug.DrawRay(arrowTargetEnd.position, arrowPointTarget.position - arrowTargetEnd.position, Color.blue);
                }
                
            }
        }
    }
}
