using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGS8USP2Animation : CombinedAnimation
{
    [Header("Specific Properties & Dependencies")]
    [SerializeField]
    private Transform animatedSdCardSlotPrefab;

    private Transform assembly;
    private Transform enclosure;
    private Transform sdCard;

    [SerializeField]
    private Transform sdCardSlot = null;

    [SerializeField]
    private Transform deviceInside = null;

    [SerializeField]
    private GameObject occluder;

    private ParticleSystem particles;

    [SerializeField] private AudioSource cardInsertionSound;
    [SerializeField] private ParticleSystem startEffect;

    private void Start()
    {
        if (sdCardSlot == null || deviceInside == null)
        {
            throw new UnassignedReferenceException("sdCardSlot & deviceInside transforms not assigned in editor!!!11!");
        }
    }

    protected override void OnAnimationStarted()
    {
        assembly = Instantiate(animatedSdCardSlotPrefab);
        enclosure = assembly.Find("Origin/SimSdSlot");
        sdCard = assembly.Find("Origin/MicroSdCard");
        occluder.SetActive(true);

        try
        {
            particles = assembly.Find("FlatStarsExplodeSmall").GetComponent<ParticleSystem>();
            particles.Play();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("USP2Animation Error - Particle effect failed: " + e.ToString());
        }

        assembly.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.7f;
        assembly.localScale = Vector3.zero;
        

        AnimateThis.With(assembly)
            .Transformate()
            .FromScale(0)
            .ToScale(2)
            .Duration(1)
            .Ease(AnimateThis.EaseOutElastic)
            .Start();


        AnimateThis.With(enclosure)
            .Transformate()
            .FromRotation(Quaternion.AngleAxis(500, Vector3.right))
            .ToRotation(Quaternion.identity)
            .Delay(0)
            .Duration(3)
            .Ease(AnimateThis.EaseSmooth)
            .Start();

        AnimateThis.With(sdCard)
            .Transformate()
            .FromRotation(Quaternion.AngleAxis(-500, Vector3.right))
            .ToRotation(Quaternion.identity)
            .Delay(0)
            .Duration(5)
            .Ease(AnimateThis.EaseSmooth)
            .OnEnd(AssembleAssembly)
            .Start();


        Vector3 forwardVec = assembly.position - Camera.main.transform.position;
        AnimateThis.With(assembly)
            .Transformate()
            .ToRotation(Quaternion.LookRotation(forwardVec, Vector3.up))
            .Duration(14)
            .Ease(AnimateThis.EaseSmooth)
            .Start();
    }

    private void AssembleAssembly()
    {
        AnimateThis.With(sdCard)
            .CancelAll()
            .Transformate()
            .ToPosition(Vector3.zero)
            .Duration(2.5f)
            .Ease(AnimateThis.EaseSmooth)
            .Start();

        AnimateThis.With(enclosure)
            .CancelAll()
            .Transformate()
            .ToPosition(Vector3.zero)
            .Duration(2.5f)
            .Ease(AnimateThis.EaseSmooth)
            .OnEnd(MoveAssemblyInFrontOfSlot)
            .Start();
    }

    private void MoveAssemblyInFrontOfSlot()
    {
         AnimateThis.With(assembly)
            .CancelAll()
            .Transformate()
            .FromScale(2)
            .ToScale(1)
            .Duration(2.5f)
            .Ease(AnimateThis.EaseInOutSmooth)
            .Start();

        AnimateThis.With(assembly)
            .Transformate()
            .ToPosition(sdCardSlot.position)
            .ToRotation(sdCardSlot.rotation)
            .Duration(4)
            .Ease(AnimateThis.EaseSmooth)
            .OnEnd(InsertAssemblyIntoPhone)
            .Start();
    }

    private void InsertAssemblyIntoPhone()
    {
        Invoke("PlayCardInsertionSound", 1.4f);
        startEffect.Play();
        AnimateThis.With(assembly)
            .CancelAll()
            .Transformate()
            .ToPosition(deviceInside.position)
            .Duration(3f)
            .Ease(AnimateThis.EaseSmooth)
            .OnEnd(KillAll)
            .Start();

    }

    void PlayCardInsertionSound()
    {
        cardInsertionSound.Play();
    }

    private void KillAll()
    {
        Destroy(assembly.gameObject);
        assembly = null;
        enclosure = null;
        sdCard = null;
        occluder.SetActive(false);
    }
}
