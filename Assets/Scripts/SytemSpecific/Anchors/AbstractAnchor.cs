using System;
using UnityEngine;


public abstract class AbstractAnchor : MonoBehaviour, IAnchor, IGazeable
{
    public string Region { get { return region; } }

    public virtual bool IsVisible { get { return flagChild.activeSelf; } set { flagChild.SetActive(value); } }
  
    protected AnchorManager anchorManager; 

    public abstract Vector3 AvatarTargetPosition { get; }

    public Vector3 AnchorPosition { get { return transform.position; }
        set
        {
            DestroyImmediate(gameObject.GetComponent<UnityEngine.XR.WSA.WorldAnchor>());
            transform.position = value;
            gameObject.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
        }
    }

    public Quaternion AnchorRotation { get { return transform.rotation; }
        set
        {
            DestroyImmediate(gameObject.GetComponent<UnityEngine.XR.WSA.WorldAnchor>());
            transform.rotation = value;
            gameObject.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
        }
    }

    public GameObject GameObject { get { return gameObject; }}


    // Inspector variables
    [Header("Anchor Parameters")]
    [Tooltip("Leave it as it is if this anchor should not be associated to a region")]
    [SerializeField] protected string region = "NoRegion";
    [SerializeField] protected GameObject flagChild;
    /// <summary>
    /// Range the user needs to get into to be able to interact with this anchor
    /// </summary>
    [SerializeField] protected float rangeToUser = 4;
    /// <summary>
    /// Range the user needs to get into to be able to see this anchor
    /// </summary>
    [SerializeField] protected float visibilityRange = 10;
    [SerializeField] protected AnchorUI anchorUI { get; private set; }


    // Methods

    protected virtual void Start()    
    {
		anchorManager = AnchorManager.Instance;

        // No Anchor UI

   /*     if (anchorUI == null)
        {
			GameObject newObject = new GameObject ("AnchorUI");
			newObject.transform.SetParent (flagChild.transform);
			anchorUI = newObject.AddComponent<AnchorUI>();
        }*/

    //    IsVisible = !ApplicationStateManager.IsUserMode;
    }

    


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

