using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbstractEnemy : PoiAnchor, IEnemy {
    
    public string EnemyName { get { return enemyName; } }
    public int Level { get { return level; } }
    public Vector3 EnemyPosition { get { return transform.position; } }
    public int HP { get { return hP; } }
    public float RelativeHP { get { return (hP * 1.0f) / initialHP; } }
    public int AD { get { return aD; } }
    public float AS { get { return aS; } }
    public int XPReward { get { return xpReward; } }
    public float AttackRate { get { return aS; } }
    public float RespawnTime { get { return respawnTime; } }
    // TODO : make effective !
    public bool IsPassive { get { return isPassive; } } 
    public float AttackRange { get { return attackRange; } }
    public float ChaseRange { get { return chaseRange; } }
    public bool PlayerInAttackRange { get { return (Utils.GetRelativeDistance(transform.position, CameraHelper.Stats.camPos) <= attackRange); } }
    public bool ReadyToAttack { get { return ((Time.time - timeSinceLastAttack) > AttackRate); } }
    public bool IsOutterChaseRange { get { return (Vector3.Distance(restPosition, transform.position) > chaseRange); } }
    public bool IsOutterSafeChaseRange { get { return (Vector3.Distance(restPosition, transform.position) > chaseRange - 2); } }
    public bool IsInRestPosition { get { return (Vector3.Distance(transform.position, restPosition) < 0.5f); } }



    protected Animator myAnimator;
    [Header("Enemy Parameters")]
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private int level = 1;
    [SerializeField] private int hP = 100;
    private int initialHP;
    [SerializeField] private int aD = 10;
    [SerializeField] private float aS = 5;
    [SerializeField] private int xpReward = 10;
    [SerializeField] private bool isPassive = false;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float chaseRange = 8;
    [SerializeField] protected float movementSpeed = 0.35f;
    [SerializeField] private float respawnTime = 10;

    private float turnSpeed = 5;
    private Vector3 restPosition;
    protected float timeSinceLastAttack;
    
    
    [Header("Soundbank")]
    [SerializeField] protected AudioClip[] attackSounds;
    [SerializeField] protected AudioClip[] chaseSounds;
    [SerializeField] protected AudioClip walkSoundLoop;
    [SerializeField] protected AudioClip[] dieSounds;
    [SerializeField] protected AudioClip[] respawnSounds;

    protected AudioSource attack_AudioSource;
    protected AudioSource chase_AudioSource;
    protected AudioSource walk_AudioSource;
    protected AudioSource die_AudioSource;
    protected AudioSource respawn_AudioSource;

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

    protected override void Awake()
    {
        base.Awake();

        timeSinceLastAttack = Time.time;
        initialHP = hP;

        myAnimator = GetComponentInChildren<Animator>();

        CreateAudioSources();

    }

    
    
    protected override void Start()
    {
        base.Start();

        restPosition = transform.position;
        
    //    EnemyKilled += GameManger.Instance.OnEnemyKilled;
    }

    protected override void Update()
    {
        base.Update();

        if (GameManger.Instance.IsInUserMode == true)
        {
            switch(state)
            {
                case State.RESTING:
                    UpdateState_Resting();
                    break;

                case State.CHASING:
                    UpdateState_Chasing();
                    break;


                case State.ATTACKING:
                    UpdateState_Attacking();
                    break;


                case State.GOING_TO_REST:
                    UpdateState_GoingToRest();
                    break;

                case State.DEAD:
                    UpdateState_Dead();
                    break;
            }
        }    
    }
    #endregion

    #region STATE_MACHINE
    protected virtual void UpdateState_Resting()
    {
        if ((PlayerInRange == true) && (IsPassive == false))
        {
            ChangeState(State.CHASING);
            myAnimator.SetBool("isWalking", true);
            Utils.PlaySound(walk_AudioSource, walkSoundLoop);
            Utils.PlayRandomSound(chase_AudioSource, chaseSounds);
        }
        else // rest
        {
                    
        }
        
    }

    protected virtual void UpdateState_Chasing()
    {
        if (PlayerInAttackRange == true)
        {
            ChangeState(State.ATTACKING);
            myAnimator.SetBool("isWalking", false);
            Utils.StopSound(walk_AudioSource);
        }
        else if (IsOutterChaseRange == true)
        {
            ChangeState(State.GOING_TO_REST);
            myAnimator.SetBool("isWalking", true);
            Utils.PlaySound(walk_AudioSource, walkSoundLoop);
        }
        else // chase
        {
            GoTo(CameraHelper.Stats.camPos);
        }
    }


    protected virtual void UpdateState_Attacking()
    {
        if (PlayerInAttackRange == true)
        {
            TurnTo(CameraHelper.Stats.camPos);
            if (ReadyToAttack == true)
            {
                Attack();
                myAnimator.SetTrigger("Attack");
                Utils.PlayRandomSound(attack_AudioSource, attackSounds);
                timeSinceLastAttack = Time.time;
            }
        }
        else // Chase 
        {
            ChangeState(State.CHASING);
            myAnimator.SetBool("isWalking", true);
            Utils.PlaySound(walk_AudioSource, walkSoundLoop);
            // not chasing sound needed here
        }
    }

    protected virtual void UpdateState_GoingToRest()
    {
        if ((PlayerInRange == true) && (IsOutterSafeChaseRange == false))
        {
            ChangeState(State.CHASING);
            myAnimator.SetBool("isWalking", true);
            Utils.PlaySound(walk_AudioSource, walkSoundLoop);
            Utils.PlayRandomSound(chase_AudioSource, chaseSounds);
        }
        else if (IsInRestPosition == false)
        {
            GoTo(restPosition);
        }
        else // rest position reached
        {
            ChangeState(State.RESTING);
            myAnimator.SetBool("isWalking", false);
            Utils.StopSound(walk_AudioSource);
        }
    }

    protected virtual void UpdateState_Dead()
    {

    }
    #endregion

    #region IGazable
    public override void OnGazeEnter(RaycastHit hitinfo)
	{
        base.OnGazeEnter(hitinfo);
        GameManger.Instance.InvokeEnemyGazedEnter(this);
        //UIManager.Instance.ShowEnemyLifeBar(enemyName, RelativeHP);
	}

    public override void OnGazeExit(RaycastHit hitinfo)
	{
        base.OnGazeExit(hitinfo);
        GameManger.Instance.InvokeEnemyGazedExit(this);
        //UIManager.Instance.HideEnemyLifeBarAfterDelay();
	}
    #endregion

    #region LOCAL_METHODS

    protected abstract void Attack();


    public virtual void GetHit(int attackDamge)
    {
        ReduceHP(attackDamge);

        // let the
        if ((state == State.RESTING) || (state == State.GOING_TO_REST))
        {
            ChangeState(State.CHASING);
            myAnimator.SetBool("isWalking", true);
        }
    }

    private void ReduceHP(int byAmount)
    {
        hP -= byAmount;
        if (hP <= 0) 
        {
            Die();
        }
        else // get hit
        {
            myAnimator.SetTrigger("GetHit");
            GameManger.Instance.InvokeEnemyHit(this);
            //UIManager.Instance.SetEnemyLife(RelativeHP);
            // TODO : hit sound
        }
    }

    protected virtual void Die()
    {
        ChangeState(State.DEAD);
        myAnimator.SetBool("isWalking", false);
        Utils.PlayRandomSound(die_AudioSource, dieSounds);
        GameManger.Instance.InvokeEnemyKilled(this);
        myAnimator.SetTrigger("Die");
        GameManger.Instance.InvokeEnemyHit(this);
        GetComponentInChildren<Collider>().enabled = false;
        Invoke("Disappear", 7);
        // TODO : dying sound
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        AnchorPosition = restPosition;
        hP = initialHP;
        myAnimator.Play("Idle");
        GetComponentInChildren<Collider>().enabled = true;
        state = State.RESTING;
        attack_AudioSource.Stop(); // respawn attack sound
        Utils.PlayRandomSound(respawn_AudioSource, respawnSounds);
    }

    protected void ChangeState(State newState)
    {
        state = newState;
    }

    // MOTION
    protected void GoTo(Vector3 position)
    {
        // position
        Vector3 finalDestination = GetRelativePosition(position);
        AnchorPosition = Vector3.MoveTowards(transform.position, finalDestination, movementSpeed * Time.deltaTime);
        // rotation
        Quaternion finalRotation = Quaternion.LookRotation((finalDestination - transform.position).normalized);
        AnchorRotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    protected void TurnTo(Vector3 position)
    {
        Vector3 finalDestination = GetRelativePosition(position);
        Quaternion finalRotation = Quaternion.LookRotation((finalDestination - transform.position).normalized);
        AnchorRotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    
    // INIT
    private void CreateAudioSources()
    {
        var mixer = Resources.Load<UnityEngine.Audio.AudioMixer>(SoundAndEffectManager.MIXER_NAME);
        var group = mixer.FindMatchingGroups("Enemies")[0];

        attack_AudioSource  = Utils.AddAudioListener(gameObject, true, 1.0f, false, group);
        chase_AudioSource   = Utils.AddAudioListener(gameObject, true, 1.0f, false, group);
        walk_AudioSource    = Utils.AddAudioListener(gameObject, true, 0.75f, true, group);
        die_AudioSource     = Utils.AddAudioListener(gameObject, true, 1.0f, false, group);
        respawn_AudioSource = Utils.AddAudioListener(gameObject, true, 1.0f, false, group);
    }

    #endregion

}
