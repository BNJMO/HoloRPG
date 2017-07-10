using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureDisplayAnimation : CombinedAnimation
{

    [SerializeField]
    GameObject measurePrefab;

    [SerializeField]
    private Transform parentTransform;

    private GameObject instance;

    protected override void OnAnimationStarted()
    {
        instance = Instantiate(measurePrefab);
        instance.transform.SetParent(parentTransform, false);
        instance.GetComponentInChildren<Animator>().enabled = true;
    }

    protected override void OnAnimationRunning() { }
    protected override void OnAnimationEnded() {
        Destroy(instance);
        instance = null;
    }

}
