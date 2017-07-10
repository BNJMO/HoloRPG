using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {

    /// <summary>
    /// Enemy name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Health
    /// </summary>
    int HP { get; }
    /// <summary>
    /// Attack Damage (- health per attack)
    /// </summary>
    int AD { get; }
    /// <summary>
    /// Attack Speed
    /// </summary>
    int AS { get; }
    /// <summary>
    /// Doesn't charges towards enemy if in range
    /// </summary>
    bool IsPassive { get; }
    /// <summary>
    /// Distance from this enemy to the player to perform an attack
    /// </summary>
    float AttackRange { get; }
    /// <summary>
    /// Distance this enemy chases the player until deciding to return back to rest point
    /// </summary>
    float ChaseRange { get; }

    /// <summary>
    /// Collision response with a projectile from player
    /// </summary>
    /// <param name="aD">attack damage from the projectile</param>
    void GetHit(int aD);
}
