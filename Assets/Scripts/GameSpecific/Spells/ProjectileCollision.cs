using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileCollision : MonoBehaviour {

    public event Action OnPositiveCollision;

    [SerializeField] private bool shotFromPlayer = true;

    void OnCollisionEnter(Collision other)
    {
        if (((shotFromPlayer == true) && (other.gameObject.tag != "MainCamera"))
            || ((shotFromPlayer == false) && (other.gameObject.tag == "MainCamera")))
        {
            if (OnPositiveCollision != null)
            {
                OnPositiveCollision.Invoke();
                //Notify.Debug(other.gameObject.name);
                
            }
        }
        
    }
}
