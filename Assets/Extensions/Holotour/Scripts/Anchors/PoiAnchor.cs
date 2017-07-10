using System;
using UnityEngine;
using UnityEngine.VR.WSA;

public class PoiAnchor : AbstractAnchor
{
    [Header("POI Parameters")]
    [SerializeField]
    private string poiId;
    public string PoiId { get { return poiId; } }

    [Tooltip("Range the user needs to get into to be able to interact with this anchor")]
    [SerializeField] private float rangeToUser = 3;
    public bool PlayerInRange { get { return Vector3.Distance(CameraHelper.Stats.camPos, transform.position) < rangeToUser; } }


    public override bool IsActive
    {
        get { return flagChild.activeSelf; }

        set
        {
//#if UNITY_EDITOR
            // We want to see the POIs in Unity Editor   (EDIT : Always!)
            flagChild.SetActive(true);  
/*#else
            flagChild.SetActive(value); 
#endif*/
        }
    }

    public virtual bool NeedsConfirmationToProceed { get { return true; }}

    public override Vector3 AvatarTargetPosition
    { get { return  transform.position; }}

}

