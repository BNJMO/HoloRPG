using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClap : MonoBehaviour {

    [Tooltip ("Only to check when Paula has apeeared")]
    [SerializeField] private Transform paula;
    [SerializeField] private AudioClip[] clapSounds;
    private AudioSource myAudioSource;

    void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

	void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "PaulaHand") && (paula.localScale.x >= 0.13f))
        {
            int i = Random.Range(0, clapSounds.Length - 1);
            myAudioSource.clip = clapSounds[i];
            myAudioSource.Play();
        }
    }
}
