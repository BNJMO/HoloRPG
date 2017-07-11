using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderGods : MonoBehaviour {


    private ParticleSystem myParticleSystem;
    private AudioSource myFX_AudioSource;
    private AudioSource myText_AudioSource;

    [Header("Sound FX")]
    [SerializeField] private AudioClip appearFx_clip;
    [SerializeField] private AudioClip disappearFx_clip;

    [Header("Text")]
    [SerializeField] private AudioClip start_clip;

    private void Awake()
    {
        myParticleSystem    = GetComponent<ParticleSystem>();
        myFX_AudioSource    = Utils.AddAudioListener(gameObject, true);
        myText_AudioSource  = Utils.AddAudioListener(gameObject, true);
    }

    private void Start()
    {
        StartCoroutine(AppearRoutine());
    }

    private IEnumerator AppearRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        var stats = CameraHelper.Stats;
        transform.position = stats.camPos + (stats.camLookDir.normalized * 5);
        myParticleSystem.Play();
        Utils.PlaySound(myFX_AudioSource, appearFx_clip);

        yield return new WaitForSeconds(1.5f);

        Utils.PlaySound(myText_AudioSource, start_clip);

        yield return new WaitForSeconds(start_clip.length - 1.5f);

        Disappear();
    }

    public void Disappear()
    {
        myParticleSystem.Stop();
        Utils.PlaySound(myFX_AudioSource, disappearFx_clip);
    }
}
