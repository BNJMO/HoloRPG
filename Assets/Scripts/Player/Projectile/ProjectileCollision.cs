using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileCollision : MonoBehaviour {

    public event Action OnPositiveCollision;

	void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "MainCamera")
        {
            if (OnPositiveCollision != null)
            {
                OnPositiveCollision.Invoke();
                Notify.Debug(other.gameObject.name);
            }
        }
        
    }
}
