using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTagAlong : MonoBehaviour {
    [SerializeField] Transform anchor;
    [SerializeField] float speed = 1;

    [SerializeField] bool fixRotation = false;

	void Update () {
	    transform.position = Vector3.Lerp (transform.position, anchor.position, Time.deltaTime * speed);
        
        if (fixRotation == false)
        {
            Vector3 anchorRotation = Camera.main.transform.rotation.eulerAngles;
            Vector3 myRotation = transform.rotation.eulerAngles;
            Quaternion newRotation =  Quaternion.Euler
                (
                    anchorRotation.x,
                    myRotation.y,
                    myRotation.z
                );
            transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, Time.deltaTime * speed);
        }	
	}
}
