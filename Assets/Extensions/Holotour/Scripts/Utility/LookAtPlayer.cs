//
// Author: Dominik Philp
// Description: aligns object this script is attached to towards the player or specified target
//
using System;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [Tooltip("Use this to override the target if you want the object to orientate towards something other than the player.")]
    public GameObject overrideTarget;
    [SerializeField]
    private bool mirror = false;

    [SerializeField]
    private bool doOnceOnSpawn = false;
    [SerializeField]
    private bool flipOnce = false;

    private GameObject target;

    void Start()
    {
        Init();

        if (flipOnce)
        {
            FlipOrientation();
        }

        if (doOnceOnSpawn)
        {
            FaceUser();
        }
    }

    private void Init()
    {
        if (overrideTarget != null)
        {
            target = overrideTarget;
        }
        else
        {
            target = Camera.main.gameObject;
        }
    }

    private void OnEnable()
    {
        Init();

        if (flipOnce)
        {
            FlipOrientation();
        }

        if (doOnceOnSpawn)
        {
            FaceUser();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!doOnceOnSpawn)
        {
            FaceUser();
        }
    }

    public void FaceUser()
    {
        if (mirror)
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        }
        else {
            transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
        }
    }

    public void FlipOrientation()
    {
        transform.rotation = Quaternion.LookRotation(-transform.forward);
    }
}
