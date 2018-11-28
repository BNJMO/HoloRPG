using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon {

    string WeaponName { get; }

    int AD { get; }

    void OnAttacked();
}
