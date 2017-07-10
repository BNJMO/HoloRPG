using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimation : CombinedAnimation {

    [Header("Particlesystem")]
    [SerializeField]
    private ParticleSystem particleSystem;

    protected override void OnAnimationStarted() {
        particleSystem.Play();
    }
    protected override void OnAnimationEnded() {
        particleSystem.Stop();
    }
}
