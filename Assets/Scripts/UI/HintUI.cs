using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintUI : MonoBehaviour {

    private TextMesh myTextMesh;

    private float timeCounter;
    private float timeToHide;

    void Awake()
    {
        myTextMesh = GetComponentInChildren<TextMesh>();
        timeToHide = Mathf.Infinity;
    }

    private void Update()
    {
        if (Time.time - timeCounter > timeToHide)
        {
            HideHintText();
        }
    }

    public void SetHintText(string hint)
    {
        myTextMesh.text = hint;
    }

    public void SetHintText(string hint, float displayTime)
    {
        myTextMesh.text = hint;
        timeCounter = Time.time;
        timeToHide = displayTime;
    }


    public void HideHintText()
    {
        myTextMesh.text = "";
    }
}
