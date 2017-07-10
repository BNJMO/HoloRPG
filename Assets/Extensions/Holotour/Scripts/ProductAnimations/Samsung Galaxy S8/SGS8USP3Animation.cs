using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGS8USP3Animation : CombinedAnimation
{
   

    [SerializeField] private Transform cloudPrefab;
    [SerializeField] private Transform cloudDestination1;
    [SerializeField] private Transform cloudDestination2;
    [SerializeField] private Transform productTransform;
    [SerializeField] private ButterflyBehaviour[] butterflies;

    // animation particles
    [SerializeField] private EllipsoidParticleEmitter globalRain;
    [SerializeField] private EllipsoidParticleEmitter globalRainFall;
    [SerializeField] private EllipsoidParticleEmitter cloudRain;
    [SerializeField] private EllipsoidParticleEmitter phoneFall;
    [SerializeField] private AudioSource rainSound;
    [SerializeField] private AudioSource birdsSound;
    [SerializeField] private AudioSource butterfliesSound;
    [SerializeField] private ParticleSystem butterflesAppearenceEffect;

   
    private Vector3 cloudOriginalScale = Vector3.one;

    private void Start()
    {
        cloudPrefab.gameObject.SetActive(false);
    }

    protected override void OnAnimationStarted()
    {
        cloudPrefab.gameObject.SetActive(true);
        cloudRain.emit = true;

        globalRain.emit = true;
        //globalRainFall.emit = true;

        rainSound.Play();

        cloudOriginalScale = cloudPrefab.localScale;
        // scale up and bring cloud to the middle
        AnimateThis.With(cloudPrefab)
            .CancelAll()
            .Transformate()
            .FromScale(0)
            .ToScale(cloudOriginalScale)
            .Duration(1.5f)
            .Ease(AnimateThis.EaseOutElastic)
            .Start();
        AnimateThis.With(cloudPrefab)
            .Transformate()
            .ToPosition(cloudDestination1.localPosition)
            .Duration(4f)
            .Ease(AnimateThis.EaseSmooth)
            .OnEnd(() => { phoneFall.emit = true; })
            .Start();

        

    }

    protected override void OnAnimationEnded()
    {
        // bring cloud to the left and scale it down
        AnimateThis.With(cloudPrefab)
            .CancelAll()
            .Transformate()
            //.FromPosition(cloudDestination1.position)
            .ToPosition(cloudDestination2.localPosition)
            .Duration(3.5f)
            .Ease(AnimateThis.EaseSmooth)
            .OnEnd(() => { cloudPrefab.gameObject.SetActive(false); phoneFall.gameObject.SetActive(false); })
            .Start();
        AnimateThis.With(cloudPrefab)
            .Transformate()
            .Delay(2)
            .FromScale(cloudOriginalScale)
            .ToScale(0)
            .Duration(1.5f)
            .Ease(AnimateThis.EaseSmooth)
            .Start();

        butterfliesSound.Play();
        butterflesAppearenceEffect.Emit(3);
        foreach(ButterflyBehaviour butterfly in butterflies)
        {
            butterfly.gameObject.SetActive(true);
            butterfly.OnAnimationStart(productTransform.localPosition, productTransform.position);
        }

        // fade out rain
        globalRain.emit = false;
        globalRainFall.emit = false;
        cloudRain.emit = false;
        phoneFall.emit = false;

        StartCoroutine(FadeOutSound(rainSound, 8, 0));
        StartCoroutine(FadeInSound(birdsSound, 5, 0));
        StartCoroutine(FadeOutSound(birdsSound, 2, 3));

    }

    IEnumerator FadeOutSound(AudioSource sound, float speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        while (sound.volume > 0)
        {
            sound.volume -= 0.1f * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        sound.Stop();
    }

    IEnumerator FadeInSound(AudioSource sound, float speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        sound.Play();
        while (sound.volume < 1)
        { 
            sound.volume += 0.1f * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }



}
