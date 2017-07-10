using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenAnimation : CombinedAnimation
{
    [SerializeField]
    GameObject penPrefab;

    [SerializeField]
    private Transform parentTransform;

    private GameObject instance;

    protected override void OnAnimationStarted()
    {
        instance = Instantiate(penPrefab);
        instance.transform.SetParent(parentTransform, false);
        PenAnimationHandler pen = instance.GetComponentInChildren<PenAnimationHandler>();
        AnimateThis.With(pen.transform)
            .Transformate()
            .FromScale(0)
            .ToScale(1)
            .Ease(AnimateThis.EaseInOutSmooth)
            .Duration(1f)
            .Start();
        AnimateThis.With(pen.transform)
            .Transformate()
            .FromRotation(Quaternion.Euler(270f, 270, 270))
            .ToRotation(Quaternion.Euler(13.459f, -7.118001f, 0.33f))
            .FromPosition(new Vector3(0, -0.3f, 0.3f))
            .ToPosition(new Vector3(-0.0247f, -0.0736f, -0.0423f))
            .Ease(AnimateThis.EaseInOutSmooth)
            .Duration(6f)
            .OnEnd(() => { pen.Play(); })
            .Start();
    }

    protected override void OnAnimationEnded()
    {
        AnimateThis.With(instance.transform)
            .Transformate()
            .FromScale(1)
            .ToScale(0)
            .Ease(AnimateThis.EaseInOutSinus)
            .Duration(0.125f)
            .OnEnd(() =>
            {
                Destroy(instance);
                instance = null;
            })
            .Start();
    }
}
