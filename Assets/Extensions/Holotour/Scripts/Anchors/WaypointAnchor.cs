using System;
using UnityEngine;
using UnityEngine.XR.WSA;

public class WaypointAnchor : AbstractAnchor
{

    public override Vector3 AvatarTargetPosition
    {
        get
        {
            Vector3 waypointPos = transform.position;
            return new Vector3(waypointPos.x, waypointPos.y +  CameraHelper.Stats.eyeHeight - 0.25f, waypointPos.z);
        }
    }
   
}

