using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothHoverAnimation : MonoBehaviour {

    [SerializeField]
    private float hoverInterval = 1;
    [SerializeField]
    private float hoverRange = 0.1f;

    void Update () {
        Vector3 pos = transform.localPosition;
        pos.y = Mathf.Sin(Time.time * Mathf.PI / hoverInterval) * hoverRange;
        transform.localPosition = pos;
    }
}
