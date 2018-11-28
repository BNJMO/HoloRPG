using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a line of red dotts following the user while recording waypoints.
/// </summary>
public class TracesVisualization : MonoBehaviour {
/*
	private float MarkerDistance = 0.25f;

	private WaypointTracingManager waypointTracingManager;
	private Transform makerContainer;
	private GameObject waypointRecordingMarkerPrefab;
	private Vector3 lastMarkerPosition = Vector3.zero;

	void Start () {
		waypointTracingManager = WaypointTracingManager.Instance;
		waypointRecordingMarkerPrefab = Resources.Load<GameObject> ("WaypointRecordingMarker");
	}

	void Update () {
		if (waypointTracingManager) {
			if (waypointTracingManager.IsRecording) {
				Vector3 currentPosition = Camera.main.transform.position;
				if (Vector3.Distance (lastMarkerPosition, currentPosition) >= MarkerDistance) {
					lastMarkerPosition = currentPosition;
					GameObject newMarker = Instantiate (waypointRecordingMarkerPrefab, CameraHelper.Stats.groundPos, Quaternion.identity);
					newMarker.transform.SetParent (GetMarkerContainer ());
				}
			} else if (makerContainer) {
				RemoveContainer ();
			}
		}
	}

	private Transform GetMarkerContainer()
	{
		string name = "WaypointRecordingDottedLine";
		if (makerContainer == null) {
			GameObject obj = GameObject.Find (name);
			makerContainer = obj ? obj.transform : null;
		}	
		if (makerContainer != null) {
			return makerContainer;
		}
		makerContainer = new GameObject (name).transform;
		return makerContainer;
	}

	private void RemoveContainer()
	{
		if (makerContainer != null) {
			GameObject.Destroy(makerContainer.gameObject);
			makerContainer = null;
		}
	}*/
}
