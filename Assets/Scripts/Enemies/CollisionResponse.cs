using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResponse : MonoBehaviour {

	void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            int attackDamge = other.gameObject.GetComponentInParent<IProjectile>().AD;
            GetComponentInParent<IEnemy>().GetHit(attackDamge);
        }
    }
}
