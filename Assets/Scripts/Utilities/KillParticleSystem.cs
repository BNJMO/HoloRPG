using UnityEngine;
using System.Collections;

public class KillParticleSystem : MonoBehaviour {

    ParticleSystem myParticleSystem;
    AudioSource myAudioSource;

    void OnEnable()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        myAudioSource = GetComponent<AudioSource>();    
    }

	void Update () {

		if (myParticleSystem.IsAlive() == false)
        {
            if (myAudioSource != null)
            {
                if (myAudioSource.isPlaying == false)
                {
                    Destroy(gameObject);
                }
            }
            else // no audio track
            {             
                Destroy(gameObject);
            }
        }

	}
}
