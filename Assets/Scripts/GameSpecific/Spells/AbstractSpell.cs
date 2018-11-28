using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

public abstract class AbstractSpell : MonoBehaviour, ISpell {

    // from interface
    public string SpellID { get { return spellID; } }
    public SpellType Type { get { return type; } }
    public int AD { get { return attackDamage; } set { attackDamage = value; } }
    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public Vector3 Direction { get { return direction; } set { direction = value; } }
    public float LifeDuration { get { return lifeDuration; } set { lifeDuration = value; } }


    [Header("Parameters")]
    [SerializeField] private string spellID = "P_name";
    [SerializeField] private SpellType type;
    [SerializeField] private float fireRate = 3;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float speed = 1;
    private Vector3 direction = Vector3.forward;
    [SerializeField] private float lifeDuration = 3;


    private float creationTime;
    [Header("Components")]
    [SerializeField] private GameObject projectileGameObject;
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioSource lifeAudioSource;
    [SerializeField] private AudioSource collisionAudioSource;

    [Header("Databank")]
    [SerializeField] private AudioClip[] shootSounds;
    [SerializeField] private AudioClip[] lifeSounds;
    [SerializeField] private AudioClip[] collisionSounds;
    [SerializeField] private ParticleSystem shootEffect; // not used yet
    [SerializeField] private ParticleSystem collisionEffect;


    private bool isActive;

    void Awake()
    {
        creationTime = Time.time;
        isActive = true;

        // make sure projectileGameObject is being referenced to
        if (projectileGameObject == null)
        {
            projectileGameObject = transform.GetChild(0).gameObject;
            if (projectileGameObject == null) Debug.LogError ("projectile GameObject not found !");
        }

        if (shootEffect != null)
            {
                ParticleSystem effect = Instantiate<ParticleSystem>(shootEffect, transform.position, transform.rotation);
                effect.transform.parent = transform;
            }

        Utils.PlayRandomSound(shootAudioSource, shootSounds);
        Utils.PlayRandomSound(lifeAudioSource, lifeSounds);

        projectileGameObject.GetComponent<ProjectileCollision>().OnPositiveCollision += AbstractProjectile_OnPositiveCollision;
    }

    protected virtual void AbstractProjectile_OnPositiveCollision()
    {
        OnEndProjectile(true);
    }

    protected virtual void Update()
    {
        UpdatePosition();

        // kill after lifeDuration
        if ((Time.time - creationTime > lifeDuration) && (isActive == true))
        {
            OnEndProjectile(false); 
            isActive = false;
        }
    }

    protected abstract void UpdatePosition();
    

    // Doesn't work ?
    void OnCollisionEnter(Collision collision)
    {
        // testing
        Notify.Debug("Collision from parent");
        Debug.Log("Collision from parent");
        OnEndProjectile(true);
    }

    protected void OnEndProjectile(bool hasCollided)
    {
        if (hasCollided)
        {
            Utils.PlayRandomSound(collisionAudioSource, collisionSounds);
            if (collisionEffect != null)
            {
                ParticleSystem effect = Instantiate<ParticleSystem>(collisionEffect, transform.position, transform.rotation);
                effect.transform.parent = transform;
            }
        }
        
        projectileGameObject.SetActive(false);
        lifeAudioSource.Stop();
        shootAudioSource.Stop();
        DestroyProjectile();
    }
    private void DestroyProjectile()
    {
        if (collisionEffect != null)
        {
            if ((collisionEffect.IsAlive() == false) && (collisionAudioSource.isPlaying == false))
            {
                Destroy(gameObject);
            }
            else
            {
                // Wait then try to destroy again
                Invoke("DestroyProjectile", 1);
            }
        }
        else // no collision effect used
        {
            if (collisionAudioSource.isPlaying == false)
            {
                Destroy(gameObject);
            }
            else
            {
                // Wait then try to destroy again
                Invoke("DestroyProjectile", 1);
            }
        }
    }




     
}
