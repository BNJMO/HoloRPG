using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell {
    string SpellID { get; }
    SpellType Type { get; }
    int AD { get; set; }
    float FireRate { get; set; }
    /// <summary>
    /// Speed at which this spell moves
    /// </summary>
    float Speed { get; set; }
    /// <summary>
    /// Direction to which it moves
    /// </summary>
    Vector3 Direction { get; set; }
    /// <summary>
    /// Time before this spell gets detroyed (when hadn't collided before)
    /// </summary>
    float LifeDuration { get; set; }
}
