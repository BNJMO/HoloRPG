using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : PoiAnchor, IEnemy {
    
    public string Name { get { return enemyName; } }
    public int HP { get { return hP; } }
    public float RelativeHP { get { return (hP * 1.0f) / initialHP; } }
    public int AD { get { return aD; } }
    public int AS { get { return aS; } }
    public float AttackRate { get { return 5 / Mathf.Sqrt(aS); } }
    public bool IsPassive { get { return isPassive; } }
    public float AttackRange { get { return attackRange; } }
    public float ChaseRange { get { return chaseRange; } }
    public bool PlayerInAttackRange { get { return (Vector3.Distance(transform.position, GetRelativePosition(CameraHelper.Stats.camPos)) <= attackRange); } }
    public bool AttackTimeElapsed { get { return ((Time.time - timeSinceLastAttack) > AttackRate); } }
    public bool IsOutterChaseRange { get { return (Vector3.Distance(restPosition, transform.position) > chaseRange); } }
    public bool IsInRestPosition { get { return (Vector3.Distance(transform.position, restPosition) < 0.5f); } }

    private Animator myAnimator;
    [Header("Enemy Parameters")]
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private int hP = 100;
    private int initialHP;
    [SerializeField] private int aD = 10;
    [SerializeField] private int aS = 5;
    [SerializeField] private bool isPassive = false;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float chaseRange = 8;

    [SerializeField] private float movementSpeed = 0.35f;
    private float turnSpeed = 5;
    private Vector3 restPosition;
    private float timeSinceLastAttack;
    
    [Header("Soundbank")]
    [SerializeField] private AudioClip[] attackSounds;
    private AudioSource myAudioSource;

    public enum State
    {
        RESTING,
        CHASING,
        ATTACKING,
        GOING_TO_REST,
        DEAD
    }
    [HideInInspector]
    public State state = State.RESTING;


    #region Unity methods

    protected virtual void Awake()
    {
        timeSinceLastAttack = Time.time;
        initialHP = hP;

        myAnimator = GetComponentInChildren<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }
    
    protected override void Start()
    {
        base.Start();
        restPosition = transform.position;
    }

    protected void Update()
    {
     /*   if (IsActive)
        {

        }*/
        switch(state)
        {
            case State.RESTING:
                if ((PlayerInRange == true) && (IsPassive == false))
                {
                    ChangeState(State.CHASING);
                    myAnimator.SetBool("isWalking", true);
                }
                else // rest
                {
                    
                }
                break;


            case State.CHASING:
                if (PlayerInAttackRange == true)
                {
                    ChangeState(State.ATTACKING);
                    myAnimator.SetBool("isWalking", false);
                }
                else if (IsOutterChaseRange == true)
                {
                    ChangeState(State.GOING_TO_REST);
                    myAnimator.SetBool("isWalking", true);
                }
                else // chase
                {
                    GoTo(CameraHelper.Stats.camPos);
                }
                break;


            case State.ATTACKING:
                if (PlayerInAttackRange == true)
                {
                    TurnTo(CameraHelper.Stats.camPos);
                    if (AttackTimeElapsed == true)
                    {
                        Attack();
                        myAnimator.SetTrigger("Attack");
                        Utils.PlaySound(myAudioSource, attackSounds);
                        timeSinceLastAttack = Time.time;
                    }
                }
                else // Chase 
                {
                    ChangeState(State.CHASING);
                    myAnimator.SetBool("isWalking", true);
                }
                break;


            case State.GOING_TO_REST:
                // state schyzophrenia between this and Chasing at edge of chaseRange
              /*  if (PlayerInRange == true)
                {
                    ChangeState(State.CHASING);
                    myAnimator.SetBool("isWalking", true);
                }
                else */if (IsInRestPosition == false)
                {
                    GoTo(restPosition);
                }
                else // rest position reached
                {
                    ChangeState(State.RESTING);
                    myAnimator.SetBool("isWalking", false);
                }
                break;

            case State.DEAD:


                break;
        }
        DebugLog.Log("state : " + state);
    }


    #endregion

    #region IGazable
    public override void OnGazeEnter(RaycastHit hitinfo)
	{
        base.OnGazeEnter(hitinfo);
        UIManager.Instance.ShowEnemyLifeBar(enemyName, RelativeHP);
	}

    public override void OnGazeExit(RaycastHit hitinfo)
	{
        base.OnGazeExit(hitinfo);
        UIManager.Instance.HideEnemyLifeBarAfterDelay();
	}
    #endregion

    #region local methods
    public virtual void GetHit(int attackDamge)
    {
        ReduceHP(attackDamge);
    }

    private void ReduceHP(int byAmount)
    {
        hP -= byAmount;
        if (hP <= 0) // Die
        {
            state = State.DEAD;
            myAnimator.SetTrigger("Die");
            UIManager.Instance.SetEnemyLife(0);
            GetComponentInChildren<Collider>().enabled = false;
            Invoke("Disappear", 3);
            // TODO : dying sound
        }
        else
        {
            myAnimator.SetTrigger("GetHit");
            UIManager.Instance.SetEnemyLife(RelativeHP);
            // TODO : hit sound
        }
    }

    protected virtual void Disappear()
    {
        gameObject.SetActive(false);
    }

    private void ChangeState(State newState)
    {
        state = newState;
        Debug.Log ("new State : " + newState);
    }

    private void GoTo(Vector3 position)
    {
        // position
        Vector3 finalDestination = GetRelativePosition(position);
        AnchorPosition = Vector3.MoveTowards(transform.position, finalDestination, movementSpeed * Time.deltaTime);
        // rotation
        Quaternion finalRotation = Quaternion.LookRotation((finalDestination - transform.position).normalized);
        AnchorRotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    private void TurnTo(Vector3 position)
    {
        Vector3 finalDestination = GetRelativePosition(position);
        Quaternion finalRotation = Quaternion.LookRotation((finalDestination - transform.position).normalized);
        AnchorRotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    

    protected abstract void Attack();

    #endregion

}
