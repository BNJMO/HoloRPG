using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile {
    int AD { get; set; }
    float Speed { get; set; }
    Vector3 Direction { get; set; }
    float LifeDuration { get; set; }
}
