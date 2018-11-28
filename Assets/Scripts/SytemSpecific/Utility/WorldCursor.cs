using UnityEngine;

public class WorldCursor : MonoBehaviour
{

	public static bool CursorHitSth {
		get;
		private set;
	}


	public static bool CursorHitSpatialCollider {
		get;
		private set;
	}

	public static Vector3 CursorPosition {
		get;
		private set;
	}

	public static Vector3 CursorNormal {
		get;
		private set;
	}

	[SerializeField]
    private LayerMask layerMask;

	private MeshRenderer meshRenderer;

	private void Start()
	{
		// Grab the mesh renderer that's on the same object as this script.
		meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
	}

	private void Update()
	{
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var stats = CameraHelper.Stats;

		RaycastHit hitInfo;

		if (Physics.Raycast(stats.camPos, stats.camLookDir, out hitInfo))
		{
			// If the raycast hit a hologram...
			// Display the cursor mesh.
			meshRenderer.enabled = true;

			// Move thecursor to the point where the raycast hit.
			transform.position = hitInfo.point;

			// Rotate the cursor to hug the surface of the hologram.
			transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
			WorldCursor.CursorHitSth = true;
			WorldCursor.CursorPosition = hitInfo.point;
			WorldCursor.CursorNormal = hitInfo.normal;
			WorldCursor.CursorHitSpatialCollider = hitInfo.collider.gameObject.layer == 31;
		}
		else
		{
			// If the raycast did not hit a hologram, hide the cursor mesh.
			meshRenderer.enabled = false;
			WorldCursor.CursorHitSth = false;
			WorldCursor.CursorHitSpatialCollider = false;
			WorldCursor.CursorPosition = Vector3.zero;
			WorldCursor.CursorNormal = Vector3.zero;
		}
	}
}