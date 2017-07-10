using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

public abstract class AbstractProjectile : MonoBehaviour, IProjectile {

    // from IProjectile
    public int AD { get { return attackDamage; } set { attackDamage = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public Vector3 Direction { get { return direction; } set { direction = value; } }
    public float LifeDuration { get { return lifeDuration; } set { lifeDuration = value; } }

    public Vector3 ProjectilePosition { get { return transform.position; }
        set
        {
            DestroyImmediate(projectileGameObject.GetComponent<WorldAnchor>());
            transform.position = value;
            projectileGameObject.AddComponent<WorldAnchor>();
        }
    }

    public Quaternion ProjectileRotation { get { return transform.rotation; }
        set
        {
            DestroyImmediate(projectileGameObject.GetComponent<WorldAnchor>());
            transform.rotation = value;
            projectileGameObject.AddComponent<WorldAnchor>();
        }
    }

    [Header("Parameters")]
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
    [SerializeField] private ParticleSystem shootEffect;
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

        Utils.PlaySound(shootAudioSource, shootSounds);
        Utils.PlaySound(lifeAudioSource, lifeSounds);

        projectileGameObject.GetComponent<ProjectileCollision>().OnPositiveCollision += AbstractProjectile_OnPositiveCollision;
    }

    private void AbstractProjectile_OnPositiveCollision()
    {
        OnEndProjectile(true);
    }

    protected virtual void Update()
    {
        // update position
        DestroyImmediate(gameObject.GetComponent<WorldAnchor>());
        transform.Translate(Direction * Speed * Time.deltaTime);
        gameObject.AddComponent<WorldAnchor>();

        if ((Time.time - creationTime > lifeDuration) && (isActive == true))
        {
            OnEndProjectile(false); 
            isActive = false;
        }
    }

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
            Utils.PlaySound(collisionAudioSource, collisionSounds);
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
