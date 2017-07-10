using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileObject : ScriptableObject {

    public string projectileName = "projectile";
    public float speed = 1;
	public GameObject projectilePrefab;
    public AudioClip[] fireSounds;
    public AudioClip[] lifeSounds;
    public AudioClip[] hitSounds;

}
