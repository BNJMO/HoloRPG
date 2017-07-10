using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBounce : MonoBehaviour {

    [SerializeField] float bouncingSpeed = 1;
    [SerializeField] float smoothingFactor = 1;

	void Update () {
        Vector3 newPos = new Vector3
            (
            transform.localPosition.x,
            transform.localPosition.y + Mathf.Sin(Time.time * bouncingSpeed),
            transform.localPosition.z
            );	
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * smoothingFactor);
	}
}
