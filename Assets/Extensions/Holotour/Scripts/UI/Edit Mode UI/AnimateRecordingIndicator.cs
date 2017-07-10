using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple animator that lets the recording button blink when recording is active.
/// </summary>
public class AnimateRecordingIndicator : MonoBehaviour {
    /*
	private float frequency = 0.5f;
	private WaypointTracingManager waypointTracingManager;
	private Graphic[] graphics;

	void Start () {
		waypointTracingManager = WaypointTracingManager.Instance;
		graphics = GetComponentsInChildren<Graphic> ();
	}

	void Update () {
		if (WaypointTracingManager.Instance && graphics.Length != 0) {
			if (WaypointTracingManager.Instance.IsRecording) {
				for (int i = 0, j = graphics.Length; i < j; i++) {
					graphics [i].enabled = true;
					Color col = graphics [i].color;
					col.a = ((Mathf.Cos (Time.time * Mathf.PI * 2 * frequency) + 1) / 2);
					graphics [i].color = col;
				}
			} else if (graphics[0].enabled) {
				for (int i = 0, j = graphics.Length; i < j; i++) {
					graphics [i].enabled = false;
				}
			}
		}
	}*/
}
