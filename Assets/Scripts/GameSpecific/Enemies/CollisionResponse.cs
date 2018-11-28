using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used on enemies to inform them they got hit
/// </summary>
public class CollisionResponse : MonoBehaviour
{

    [SerializeField] private bool isPlayer = false;


    // make sure not both methods get called (for any reason)
    private bool collisionTriggered = false;

    void OnCollisionEnter(Collision other)
    {
        if (isPlayer == true)
        {
            if (other.gameObject.tag == "EnemyProjectile")
            {
                collisionTriggered = true;
                int attackDamage = other.gameObject.GetComponentInParent<ISpell>().AD;
                GameManger.Instance.InvokePlayerGotHit(attackDamage);
            }
        }
        else //isEnemy == true
        {
            if (other.gameObject.tag == "PlayerProjectile")
            {
                collisionTriggered = true;
                int attackDamage = other.gameObject.GetComponentInParent<ISpell>().AD;
                GetComponentInParent<IEnemy>().GetHit(attackDamage);
            }
            else if (other.gameObject.tag == "PlayerMeelee")
            {
                collisionTriggered = true;
                IWeapon weapon = other.gameObject.GetComponentInParent<IWeapon>();

                int attackDamage = weapon.AD;
                GetComponentInParent<IEnemy>().GetHit(attackDamage);
            }
        }
        

    }

    void OnTriggerEnter(Collider other)
    {
        
        if (collisionTriggered == false)
        {
            if (isPlayer == true)
            {
                if (other.gameObject.tag == "EnemyProjectile")
                {
                    int attackDamage = other.gameObject.GetComponentInParent<ISpell>().AD;
                    GameManger.Instance.InvokePlayerGotHit(attackDamage);
                }
            }
            else // isEnemy
            {
                if (other.gameObject.tag == "PlayerProjectile")
                {
                    int attackDamage = other.gameObject.GetComponentInParent<ISpell>().AD;
                    GetComponentInParent<IEnemy>().GetHit(attackDamage);
                }
                else if (other.gameObject.tag == "PlayerMeelee")
                {
                    // int attackDamage = other.gameObject.GetComponentInParent<IWeapon>().AD;
                    int attackDamage = 15;
                    GetComponentInParent<IEnemy>().GetHit(attackDamage);
                }
                else if (other.gameObject.tag == "EnemyProjectile")
                {
                    int attackDamage = other.gameObject.GetComponentInParent<ISpell>().AD;
                    GameManger.Instance.InvokePlayerGotHit(attackDamage);
                }
            }
        }

    }
}
