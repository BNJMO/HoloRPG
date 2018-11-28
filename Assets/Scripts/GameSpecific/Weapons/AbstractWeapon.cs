using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour, IWeapon {

    public string WeaponName { get { return weaponName; } }
    public int AD { get { return attackDamage; } }


    [SerializeField] private string weaponName = "W_name";
    [SerializeField] private int attackDamage = 15;


    void IWeapon.OnAttacked()
    {
        AttackResponse();
    }

    /// <summary>
    /// Sound or effects triggered when attacked with this weapon
    /// </summary>
    protected abstract void AttackResponse();
}
