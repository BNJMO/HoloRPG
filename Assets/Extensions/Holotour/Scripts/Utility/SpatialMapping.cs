using UnityEngine;


public class SpatialMapping : Singleton<SpatialMapping>
{
    [HideInInspector]
    public static int PhysicsRaycastMask;

    [Tooltip("The material to use when rendering Spatial Mapping data.")]
    public Material DrawMaterial;

    [Tooltip("If true, the Spatial Mapping data will be rendered.")]
    private bool drawVisualMeshes = false;

    // If true, Spatial Mapping will be enabled. 
    private bool mappingEnabled = true;

    // Handles rendering of spatial mapping meshes.
    private UnityEngine.XR.WSA.SpatialMappingRenderer spatialMappingRenderer;

    // Creates/updates environment colliders to work with physics.
    private UnityEngine.XR.WSA.SpatialMappingCollider spatialMappingCollider;

	private RaycastHit hitInfo;

    /// <summary>
    /// Determines if the spatial mapping meshes should be rendered.
    /// </summary>
    public bool DrawVisualMeshes
    {
        get
        {
            return drawVisualMeshes;
        }
        set
        {
            drawVisualMeshes = value;

            if (drawVisualMeshes)
            {
                spatialMappingRenderer.visualMaterial = DrawMaterial;
                spatialMappingRenderer.renderState = UnityEngine.XR.WSA.SpatialMappingRenderer.RenderState.Visualization;
            }
            else
            {
                spatialMappingRenderer.renderState = UnityEngine.XR.WSA.SpatialMappingRenderer.RenderState.None;
            }
        }
    }
 
    /// <summary>
    /// Enables/disables spatial mapping rendering and collision.
    /// </summary>
    public bool MappingEnabled
    {
        get
        {
            return mappingEnabled;
        }
        set
        {
            mappingEnabled = value;
            spatialMappingCollider.freezeUpdates = !mappingEnabled;
            spatialMappingRenderer.freezeUpdates = !mappingEnabled;
            gameObject.SetActive(mappingEnabled);
        }
    }

    private new void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        spatialMappingRenderer = gameObject.GetComponent<UnityEngine.XR.WSA.SpatialMappingRenderer>();
        spatialMappingRenderer.surfaceParent = this.gameObject;
        spatialMappingCollider = gameObject.GetComponent<UnityEngine.XR.WSA.SpatialMappingCollider>();
        spatialMappingCollider.surfaceParent = this.gameObject;
		PhysicsRaycastMask = (1 << spatialMappingCollider.layer);
        DrawVisualMeshes = drawVisualMeshes;
        MappingEnabled = mappingEnabled;
    }

	/// <summary>
	/// Runs another time the Start method when a new SpatialMapping object is instantiated.
	/// </summary>
	public void Restart()
	{
		spatialMappingRenderer = gameObject.GetComponent<UnityEngine.XR.WSA.SpatialMappingRenderer>();
		spatialMappingRenderer.surfaceParent = this.gameObject;
		spatialMappingCollider = gameObject.GetComponent<UnityEngine.XR.WSA.SpatialMappingCollider>();
		spatialMappingCollider.surfaceParent = this.gameObject;
		PhysicsRaycastMask = (1 << spatialMappingCollider.layer);
		DrawVisualMeshes = drawVisualMeshes;
		MappingEnabled = mappingEnabled;
	}

	public static Vector3 ProjectOnFloor(Vector3 point)
	{
		RaycastHit groundingHit;
		if (Physics.Raycast (point, Vector3.down, out groundingHit, Mathf.Infinity, SpatialMapping.PhysicsRaycastMask)) {
			return groundingHit.point;
		}
		return point;
	}

	public static bool ProjectOnFloor(Vector3 point, out Vector3 pointOut)
	{
		RaycastHit groundingHit;
		if (Physics.Raycast (point, Vector3.down, out groundingHit, Mathf.Infinity, SpatialMapping.PhysicsRaycastMask)) {
			pointOut = groundingHit.point;
			return true;
		}
		pointOut = point;
		return false;
	}
}