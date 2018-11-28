using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : AbstractEnemy {


    private bool PlayerInHitRange { get { return Utils.GetRelativeDistance(transform.position, Camera.main.transform.position) < hitRange; } }
    private bool ReachedDestination { get { return Vector3.Distance(transform.position, destinationPos) < 4f; } }

    [Header("Charge Enemy")]
    [SerializeField] private float chargeSpeedFactor = 3;
    [SerializeField] private float hitRange = 1;

    private bool isCharging = false;
    private bool playerGotHit = false;
    private float initialSpeed;
    Vector3 destinationPos;

    protected override void Awake()
    {
        base.Awake();

        initialSpeed = movementSpeed;
    }


    protected override void UpdateState_Attacking()
    {
       // Debug.Log ("Attack State : ");
        if ((PlayerInAttackRange == true) 
            && (ReadyToAttack == true) 
            && (isCharging == false))
        {
           // Debug.Log ("Launching attack");
            myAnimator.SetBool("isCharging", true);
            Utils.PlayRandomSound(attack_AudioSource, chaseSounds);
            timeSinceLastAttack = Time.time;
            movementSpeed *= chargeSpeedFactor;
            isCharging = true;
            playerGotHit = false;
            Vector3 playerRelativePos = GetRelativePosition(Camera.main.transform.position);
            destinationPos = playerRelativePos + (playerRelativePos - transform.position).normalized * 7;
            Attack();
        }
        else if (isCharging == true)
        {
            //Debug.Log ("Charging");
            Attack();
        }    
        // Wait until Ready to attack
        else if ((PlayerInAttackRange == true)  
            && (ReadyToAttack == false) 
            && (isCharging == false))
        {
            //Debug.Log ("Waiting");
            TurnTo(Camera.main.transform.position);
            myAnimator.SetBool("isCharging", false);
            movementSpeed = initialSpeed;
        }  
        else // Chase 
        {
            //Debug.Log ("Chaging to Chase");
            movementSpeed = initialSpeed;
            playerGotHit = false;
            ChangeState(State.CHASING);
            myAnimator.SetBool("isCharging", false);
            myAnimator.SetBool("isWalking", true);
            Utils.PlaySound(walk_AudioSource, walkSoundLoop);
            // not chasing sound needed here
        }
    }

    protected override void Die()
    {
        base.Die();
        movementSpeed = initialSpeed;
        myAnimator.SetBool("isWalking", false);
        myAnimator.SetBool("isCharging", false);
        isCharging = false;
        playerGotHit = false; 
    }

    // Charges towards the enemy
    protected override void Attack()
    {
        GoTo(destinationPos);

        // Hit the player once if in hitRange
        if ((PlayerInHitRange == true) && (playerGotHit == false))
        {
            GameManger.Instance.InvokePlayerGotHit(AD);
            Utils.PlayRandomSound(attack_AudioSource, attackSounds);
            playerGotHit = true;
        }
        //Debug.Log (Vector3.Distance(transform.position, destinationPos));
        if (ReachedDestination == true)
        {
            //Debug.Log ("Reached destination");
            isCharging = false;
        }
    }

    

}
