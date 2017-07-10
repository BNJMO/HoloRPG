using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTracingSampleIndicator : MonoBehaviour {


	[SerializeField]
	private float PulsingScaleMin = 0.025f;
	[SerializeField]
	private float PulsingScaleMax = 0.05f;
	[SerializeField]
	private float PulsingFrequency = 0.5f;

	private float startTime;

	void Start () {
		startTime = Time.time;	
	}

	void Update () {
		transform.localScale = Vector3.one * (PulsingScaleMin + ((PulsingScaleMax - PulsingScaleMin) * ((Mathf.Cos ((startTime - Time.time) * 2 * Mathf.PI) + 1) / 2)));
	}
}
