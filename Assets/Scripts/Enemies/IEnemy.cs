using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {

    /// <summary>
    /// Enemy name
    /// </summary>
    string EnemyName { get; }
    /// <summary>
    /// Defines the enemy categroy (i.e. for item drop)
    /// </summary>
    int Level { get; } 
    /// <summary>
    /// Enemy world position
    /// </summary>
    Vector3 EnemyPosition { get; }
    /// <summary>
    /// Health
    /// </summary>
    int HP { get; }
    /// <summary>
    /// Health percentage.
    /// </summary>
    float RelativeHP { get; }
    /// <summary>
    /// Attack Damage (- health per attack)
    /// </summary>
    int AD { get; }
    /// <summary>
    /// Time between two consecutive attacks
    /// </summary>
    float AS { get; }
    /// <summary>
    /// XP the player will be rewarded with when he kills this monster
    /// </summary>
    int XPReward { get; }
    /// <summary>
    /// Time before respawn (infinity for no respawn)
    /// </summary>
    float RespawnTime { get; }
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
