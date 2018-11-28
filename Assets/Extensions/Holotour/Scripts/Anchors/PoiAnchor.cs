using System;
using UnityEngine;
using UnityEngine.XR.WSA;

public class PoiAnchor : AbstractAnchor
{
    public string PoiId { get { return poiId; } }

    public override bool IsVisible { get { return myVisibiliter.IsVisible; } set { myVisibiliter.IsVisible = value; } }

    public virtual bool PlayerInRange { get { return Utils.GetRelativeDistance(CameraHelper.Stats.camPos, transform.position) < rangeToUser; } }
    public virtual bool PlayerInVisibilityRange { get { return Utils.GetRelativeDistance(CameraHelper.Stats.camPos, transform.position) < visibilityRange; } }
    
    // Useless?
    public virtual bool NeedsConfirmationToProceed { get { return true; }}
    public override Vector3 AvatarTargetPosition { get { return  transform.position; }}


    [Header("POI Parameters")]
    [SerializeField] private string poiId;

    
    private VisibiliterMesh myVisibiliter;

    protected virtual void Awake()
    {
        myVisibiliter = gameObject.AddComponent<VisibiliterMesh>();
        myVisibiliter.VisibilityCondition = Condition.New(() => PlayerInVisibilityRange == true);
    }

    protected override void Start()
    {
        base.Start();
        // make sure visibility range is always bigger than range to user
        if (visibilityRange <= rangeToUser) 
        {
            visibilityRange = rangeToUser + 1;
        } 
    }

    protected virtual void Update()
    {

    }

    

    



}

