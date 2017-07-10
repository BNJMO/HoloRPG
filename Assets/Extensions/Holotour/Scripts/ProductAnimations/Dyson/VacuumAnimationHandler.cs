using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumAnimationHandler : MonoBehaviour {

	[SerializeField] ParticleSystem particleSys;
    ParticleSystem.Particle[] particles;
    [SerializeField] Transform targetTransform;
    [SerializeField] float transitionDuration = 5;
    [SerializeField] float speed = 1f;
    [SerializeField] float noiseIntensity = 0;
    [SerializeField] float speend_end = 4.5f;
    [SerializeField] float noiseIntensity_end = 7.5f;
    [SerializeField] float startVacuumDelay = 6;

    void OnEnable()
    {
        particleSys.Play();

        speed = 0;
        noiseIntensity = 0;

        AnimateThis.With(particleSys.transform).CancelAll()
            .Do(t => speed = t)
            .To(speend_end)
            .Duration(transitionDuration)
            .Ease(AnimateThis.EaseSmooth)
            .Delay(startVacuumDelay)
            .OnStart(() => GetComponent<AudioSource>().Play())
            .Start();

        AnimateThis.With(particleSys.transform)
            .Do(t => noiseIntensity = t)
            .To(noiseIntensity_end)
            .Duration(transitionDuration)
            .Ease(AnimateThis.EaseSmooth)
            .Delay(startVacuumDelay)
            .Start();
    }

    void OnDisable()
    {
        particleSys.Stop();
    }

    void LateUpdate()
    {
        InitIfNeeded();

        int numParticlesAlive = particleSys.GetParticles(particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 direction = (targetTransform.position - particles[i].position).normalized;
            float remainingDistance = Vector3.Distance (targetTransform.position, particles[i].position);
            if (remainingDistance > 0.25f)
            {
                Vector3 randomDirection = (new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * remainingDistance).normalized;
                particles[i].velocity += ((direction * speed) + (randomDirection * noiseIntensity)) * Time.deltaTime;
            }
            else
            {
                particles[i].remainingLifetime = 0.3f;
            }
            
        }
        particleSys.SetParticles (particles, numParticlesAlive);
        particleSys.transform.position = Camera.main.transform.position + Vector3.down;

    }

    void InitIfNeeded()
    {
        if ((particles == null) || (particles.Length < particleSys.main.maxParticles))
        {
            particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
        }
    }
}
