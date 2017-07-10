using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : HoloToolkit.Unity.Singleton<TimeScaler> {

    [Range (0, 5)]
	[SerializeField] float timeScale = 1;

    void Update()
    {
        Time.timeScale = timeScale;
    }
}
