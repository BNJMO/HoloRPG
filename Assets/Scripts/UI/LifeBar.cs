using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

	[SerializeField] private Image barImage;
    [SerializeField] private Image contentImage;

    public bool IsVisible
    {
        get
        {
            return isVisible;
        }
        set
        {
            barImage.enabled = value;
            contentImage.enabled = value;   
            isVisible = value;
        }
    }
    private bool isVisible;

    public void SetPercentage(float newPercentage)
    {
        contentImage.fillAmount = newPercentage;
    }

    public void SetPercentage(int newPercentage)
    {
        contentImage.fillAmount = newPercentage / 100.0f;
    }
}
