using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRestartManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
    //    AvatarManager.Instance.OnTourEnded += OnTourEnded;
    }

    private void OnTourEnded()
    {
        Invoke("DoRestart", 8);
    }

    private void DoRestart()
    {
        ApplicationStateManager.Instance.ReloadCurrentScene();
    }
}
