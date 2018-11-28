using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour {

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update () {
  /*      LineRenderer bb = GetComponent<LineRenderer>();

        Vector3[] points = {
            new Vector3(0.5f, 0.5f, 0.5f),  new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
        };
        bb.SetPositions(points);
        bb.positionCount = points.Length;*/
	}
}
