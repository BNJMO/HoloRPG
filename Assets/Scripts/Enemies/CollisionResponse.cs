using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResponse : MonoBehaviour {

    // make sure not both methods get called (for any reason)
    private bool collisionTriggered = false;

	void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            collisionTriggered = true;  
            int attackDamge = other.gameObject.GetComponentInParent<ISpell>().AD;
            GetComponentInParent<IEnemy>().GetHit(attackDamge);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "PlayerProjectile") && (collisionTriggered == false))
        {
            int attackDamge = other.gameObject.GetComponentInParent<ISpell>().AD;
            GetComponentInParent<IEnemy>().GetHit(attackDamge);
        }
    }
}
