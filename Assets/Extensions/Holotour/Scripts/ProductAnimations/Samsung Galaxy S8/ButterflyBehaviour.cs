using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyBehaviour : MonoBehaviour {

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Vector3 phonePos;

    void OnEnable()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
    }

	public void OnAnimationStart(Vector3 fromPosition, Vector3 phonePosition)
    {
        phonePos = phonePosition;
        AnimateThis.With(this)
            .Transformate()
            .FromScale(0)
            .ToScale(originalScale)
            .Ease(AnimateThis.EaseSmooth)
            .Duration(1f)
            .Start();
        AnimateThis.With(this)
            .Transformate()
            .FromPosition(fromPosition)
            .ToPosition(originalPosition)
            .Duration(13.5f)
            .Ease(AnimateThis.EaseSmooth)
            .Start();

        //disappear after delay
        AnimateThis.With(this)
            .Transformate()
            .Delay(10)
            .FromScale(originalScale)
            .ToScale(0)
            .Duration(1.5f)
            .Ease(AnimateThis.EaseInOutSmooth)
            .OnEnd(() => { gameObject.SetActive(false); })
            .Start();

    }

    void Update()
    {
        transform.LookAt (phonePos);

    }

}
