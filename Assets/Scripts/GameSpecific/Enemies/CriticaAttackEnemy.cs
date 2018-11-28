using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticaAttackEnemy : AbstractEnemy {

    [Header("CrticalAttack Enemy")]
    [SerializeField] float criticalFactor = 1.5f;
    [Range(0.0f, 1.0f)]
    [SerializeField] float criticalChance = 0.5f;
    [SerializeField] AudioClip criticalSound;

    private AudioSource myAudioSource;

    protected override void Awake()
    {
        base.Awake();

        myAudioSource = gameObject.AddComponent<AudioSource>();
        myAudioSource.clip = criticalSound;
        myAudioSource.spatialBlend = 1.0f;
        myAudioSource.playOnAwake = false;
    }

    protected override void Attack()
    {
        float attackDamage = AD;
        float chance = UnityEngine.Random.Range(0, 1.0f);
        if (chance >= 1.0f - criticalChance)
        {
            attackDamage *= criticalFactor;
        }
        GameManger.Instance.InvokePlayerGotHit((int)attackDamage);
        myAudioSource.Play();
    }

}
