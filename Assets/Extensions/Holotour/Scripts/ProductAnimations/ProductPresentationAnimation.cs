using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductPresentationAnimation : CombinedAnimation
{

    [SerializeField]
    private Transform objectToPresent;

    [SerializeField]
    private bool animateApparanceanimateAppearance = true;

    private void Start()
    {
        if (Application.isPlaying)
        {
            if (animateApparanceanimateAppearance)
            {
                objectToPresent.localScale = Vector3.zero;
            }
            objectToPresent.gameObject.SetActive(false);
        }

    }

    protected override void OnAnimationStarted() {
        if (animateApparanceanimateAppearance)
        {
            if (animateApparanceanimateAppearance)
            {
                objectToPresent.localScale = Vector3.zero;
            }
            
            AnimateThis.With(objectToPresent).CancelAll()
                .Transformate()
                .FromScale(0)
                .ToScale(1)
                .Ease(AnimateThis.EaseOutQuintic)
                .Duration(2)
                .Start();
        }
        objectToPresent.gameObject.SetActive(true);
    }
    
    protected override void OnAnimationEnded() {
        if (animateApparanceanimateAppearance)
        {
            AnimateThis.With(objectToPresent).CancelAll()
                .Transformate()
                .FromScale(1)
                .ToScale(0)
                .Ease(AnimateThis.EasePow2)
                .Duration(1)
                .OnEnd(() => objectToPresent.gameObject.SetActive(false))
                .Start();
        } else
        {

            objectToPresent.gameObject.SetActive(false);
        }

    }
}