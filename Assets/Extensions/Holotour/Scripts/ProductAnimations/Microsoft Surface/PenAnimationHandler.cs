using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenAnimationHandler : MonoBehaviour
{
    [SerializeField] TrailRenderer[] trailRenderers;
    [SerializeField] Transform activeTrailRenderersContainer;
    [SerializeField] Transform inactiveTrailRenderersContainer;
    [SerializeField] GameObject saturnPanel;
    [SerializeField] Animator penAnimator;

    private Color saturnColor;


    void Start()
    {
        if (trailRenderers.Length != 3)
        {
            Debug.LogError ("Trail Renderers not properly set up");
        }
        saturnColor = trailRenderers[0].startColor;
    }

    public void OnSegmentStarted(int segmentID)
    {
        trailRenderers[segmentID].transform.SetParent(activeTrailRenderersContainer);
        trailRenderers[segmentID].transform.localPosition = Vector3.zero;
        trailRenderers[segmentID].Clear();
        trailRenderers[segmentID].enabled = true;
    }

    public void OnSegmentCompleted(int segmentID)
    {
        trailRenderers[segmentID].transform.SetParent (inactiveTrailRenderersContainer);
    }

    internal void Play()
    {
        penAnimator.enabled = true;
    }

    public IEnumerator FadeInOriginalImage(float speed)
    {
        Material mat = saturnPanel.GetComponent<MeshRenderer>().material;
        Color color = saturnColor;
        color.a = 0;
        while (color.a < 0.1f)
        {
            float alpha = color.a + (0.01f * speed * Time.deltaTime);
            color = new Color
                (
                color.r,
                color.g,
                color.b,
                alpha
                );
            mat.SetColor ("_TintColor", color);
            yield return new WaitForEndOfFrame();
        }
    }

    public /* IEnumerator */ void FadeOutDrawnImage(float speed)
    {
        /*Color color = saturnColor;
        while (color.a > 0)
        {
            foreach (TrailRenderer trail in trailRenderers)
            {
                float alpha = color.a - (0.05f * speed * Time.deltaTime);
                color = new Color
                (
                color.r,
                color.g,
                color.b,
                alpha
                );
                trail.startColor = color;
                trail.endColor = color;
                yield return new WaitForEndOfFrame();  
            }
        }*/
    }
}
