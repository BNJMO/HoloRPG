using System;
using UnityEngine;
using UnityEngine.VR.WSA;

public abstract class AbstractAnchor : MonoBehaviour, IAnchor, IGazeable
{
    protected AnchorManager anchorManager; 

    
    [SerializeField]
    protected AnchorUI anchorUI { get; private set; }

    [Header("Anchor Parameters")]
    [SerializeField]
    protected GameObject flagChild;

    [SerializeField]
    private float visibilityDistance = 5;
    
    public virtual bool IsActive { get { return flagChild.activeSelf; } set { flagChild.SetActive(value); } }
    public virtual bool IsVisible { get { return Vector3.Distance(transform.position, GetRelativePosition(CameraHelper.Stats.camPos)) < visibilityDistance; } }
    public abstract Vector3 AvatarTargetPosition { get; }

    public Vector3 AnchorPosition { get { return transform.position; }
        set
        {
            DestroyImmediate(gameObject.GetComponent<WorldAnchor>());
            transform.position = value;
            gameObject.AddComponent<WorldAnchor>();
        }
    }

    public Quaternion AnchorRotation { get { return transform.rotation; }
        set
        {
            DestroyImmediate(gameObject.GetComponent<WorldAnchor>());
            transform.rotation = value;
            gameObject.AddComponent<WorldAnchor>();
        }
    }

    public GameObject GameObject { get { return gameObject; }}


    protected virtual void Start()    
    {
		anchorManager = AnchorManager.Instance;

        if (anchorUI == null)
        {
			GameObject newObject = new GameObject ("AnchorUI");
			newObject.transform.SetParent (flagChild.transform);
			anchorUI = newObject.AddComponent<AnchorUI>();
        }

        IsActive = !ApplicationStateManager.IsUserMode;
    }


    //                                      TODO : implement visibility !!!!
   /* protected void Update()
    {

    }*/

	public virtual void OnGazeEnter(RaycastHit hitinfo)
	{
        if (AnchorEditorUi.Instance != null)
        {
            AnchorEditorUi.Instance.GazedAnchor = this;
        }
	}

	public virtual void OnGazeStay(RaycastHit hitinfo)
	{

	}

	public virtual void OnGazeExit(RaycastHit hitinfo)
	{
        if (AnchorEditorUi.Instance != null)
        {
            AnchorEditorUi.Instance.GazedAnchor = null;
        }
    }

    /// <summary>
    /// Returns the given position with the height of this anchor
    /// </summary>
    protected Vector3 GetRelativePosition(Vector3 originalPosition)
    {
        return new Vector3
        (
            originalPosition.x,
            transform.position.y,
            originalPosition.z      
        );
    }
}

