using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayable : MonoBehaviour {

    [SerializeField]
    private MonoPlayable playableToStart;

	void Update () {
        if (playableToStart == null)
        {
            playableToStart = GetComponent<MonoPlayable>();
        }
        if (playableToStart != null && !playableToStart.IsPlaying)
        {
            playableToStart.Play();
        }
	}

}
