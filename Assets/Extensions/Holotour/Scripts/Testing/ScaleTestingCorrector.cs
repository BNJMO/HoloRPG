using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Counter the effect of AnimateThis on Paula and rescale her to her original size
/// </summary>
public class ScaleTestingCorrector : MonoBehaviour {


    void LateUpdate ()
    {
        transform.localScale = new Vector3 (0.1358763f, 0.1358763f, 0.1358763f);
    }
}
