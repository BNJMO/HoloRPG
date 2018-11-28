using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeEnemy : AbstractEnemy
{
    [Tooltip("Long Range")]
    [SerializeField] private AbstractSpell mySpell;
    [SerializeField] private Transform spellBeginPosition;
    private string spellID;

    protected override void Start()
    {
        base.Start();
        spellID = mySpell.SpellID;
    }

    protected override void Attack()
    {
        Vector3 direction = (Camera.main.transform.position - spellBeginPosition.position).normalized;
        Vector3 position = spellBeginPosition.position + transform.forward * 0.25f;
        MakeSpell.InstantiateObj(spellID, position, direction);
    }


}
