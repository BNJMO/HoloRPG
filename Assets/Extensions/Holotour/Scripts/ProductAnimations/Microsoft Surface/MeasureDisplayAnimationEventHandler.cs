using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureDisplayAnimationEventHandler : MonoBehaviour {

    [SerializeField]
    private AudioSource openTapeSoundFx;

    [SerializeField]
    private AudioSource closeTapeSoundFx;

    [SerializeField]
    private Transform tape;
    private Vector3 tapeScale;

    void Start()
    {
        tapeScale = tape.localScale;
    }

    public void OnOpenTape()
    {
        openTapeSoundFx.Play();
        StartCoroutine(OpenTape());
     /*   AnimateThis.With(tape)
            .Transformate()
            .FromScale(tapeScale)
            .ToScale(9.704131f)
            .Duration(0.6f)
            .Ease(AnimateThis.EasePow2)
            .Start();*/
    }

    public void OnCloseTape()
    {
        closeTapeSoundFx.Play();
       /* AnimateThis.With(tape)
            .Transformate()
            .FromScale(9.704131f)
            .ToScale(tapeScale)
            .Duration(0.6f)
            .Ease(AnimateThis.EasePow2)
            .Start();*/
        StartCoroutine(CloseTape());
    }


    IEnumerator OpenTape()
    {
        Vector3 nowScale = tape.localScale;
        while (nowScale.x < 9.704131f)
        {
            float x = nowScale.x + Time.deltaTime * 20;
            Vector3 newScale = new Vector3 (x, nowScale.y, nowScale.z);
            tape.localScale = newScale;
            nowScale = newScale;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CloseTape()
    {
        Vector3 nowScale = tape.localScale;
        while (nowScale.x > tapeScale.x)
        {
            float x = nowScale.x - Time.deltaTime * 40;
            Vector3 newScale = new Vector3 (x, nowScale.y, nowScale.z);
            tape.localScale = newScale;
            nowScale = newScale;
            yield return new WaitForEndOfFrame();
        }
    }

}
