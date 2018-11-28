using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : Singleton<TimeScaler> {

    [Range (0, 5)]
	[SerializeField] float timeScale = 1;

    void Update()
    {
        Time.timeScale = timeScale;
    }
}
