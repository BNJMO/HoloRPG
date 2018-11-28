using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProductGizmo : AnchorGizmo {

    private bool highlighed;

    public override Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            DestroyImmediate(GetComponent<UnityEngine.XR.WSA.WorldAnchor>());
            transform.position = value;
            gameObject.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
        }
    }

    public override Quaternion Rotation
    {
        get
        {
            return transform.rotation;
        }

        set
        {
            DestroyImmediate(GetComponent<UnityEngine.XR.WSA.WorldAnchor>());
            transform.rotation = value;
            gameObject.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
        }
    }

    public override bool Highlighted
    {
        get
        {
            return highlighed;
        }
        set
        {
            highlighed = value;
            LineRenderer line = GetComponentInChildren<LineRenderer>();
            if (line != null)
            {
                Color col = highlighed ? Color.red : Color.white;
                line.startColor = col;
                line.endColor = col;
            }
        }
    }
}
