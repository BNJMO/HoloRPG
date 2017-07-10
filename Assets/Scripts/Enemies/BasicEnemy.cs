using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : AbstractEnemy
{
    protected override void Attack()
    {
        GameManger.Instance.ReducePlayerLife(AD);
    }
}
