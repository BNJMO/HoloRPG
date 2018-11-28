using UnityEngine;


//FIXME @Dominik: Die Anchor-Funktionalität funktioniert nicht.
// Außerdem soll dieser Manager ja generisch sein. Implementiere die
// Anchor-Funktionalität bitte in den IGazeables oder IAirtapables.
// Funktioniert nach einigen Tests nicht anständig. 
public class GazeGestureManager :  Singleton<GazeGestureManager>
{
	private AnchorManager anchorManager;

    // Gazing
    private RaycastHit hitInfo;

    public GameObject FocusedObject { get; private set; }

    private IGazeable newGaze;
    private IGazeable oldGaze;

    // Gestures & Input
	public UnityEngine.XR.WSA.Input.GestureRecognizer recognizer;

    private IAirtapable tapable;

    public bool IsHolding { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        // Set up a GestureRecognizer to detect Select gestures.
		recognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
		recognizer.StartCapturingGestures();
    }

	private void Start()
	{
		anchorManager = AnchorManager.Instance;
		
        recognizer.HoldStartedEvent += GestureRecognizer_HandleHoldStart;
        recognizer.HoldCanceledEvent += GestureRecognizer_HandleHoldFinish;
        recognizer.HoldCompletedEvent += GestureRecognizer_HandleHoldFinish;
	
	}

	private void Update()
	{
		Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red, 0, true);
		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity, ~SpatialMapping.PhysicsRaycastMask)) {
			HandleGaze (FindGazeable(hitInfo.collider.transform), hitInfo);
		} else {
			HandleGaze (null, hitInfo);
		}
	}

	private IGazeable FindGazeable(Transform t)
	{
		IGazeable result = t.GetComponentInChildren<IGazeable> ();
		if (result == null) {
			result = t.GetComponentInParent<IGazeable> ();
		}	
		return result;
	}

	private void HandleGaze(IGazeable newGaze, RaycastHit hit)
    {
		if (newGaze != oldGaze) {
			if (oldGaze != null) {
				oldGaze.OnGazeExit (hit);
			}
			if (newGaze != null) {
				newGaze.OnGazeEnter (hit);
			}
		} else if (newGaze != null) {
			newGaze.OnGazeStay (hit);
		}
		oldGaze = newGaze;
    }

    private void GestureRecognizer_HandleHoldStart(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
    {
        if (source == UnityEngine.XR.WSA.Input.InteractionSourceKind.Controller)
        {
            IsHolding = true;
        }
    }

    private void GestureRecognizer_HandleHoldFinish(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
    {
        if (source == UnityEngine.XR.WSA.Input.InteractionSourceKind.Controller)
        {
            IsHolding = false;
        }    
    }
}
