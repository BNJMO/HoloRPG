using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbler : MonoBehaviour {

    private Transform tumbler;

    [SerializeField] float forceIntensity = 1;
    [SerializeField] Vector3 forceDirection;
    [SerializeField] float forceDuration = 2;
    [SerializeField] float waitTime = 5;


    private float direction = 1;
    private bool isAddingForce = false;
    private float startedTime;

    private void Start()
    {
        tumbler = GetComponentInChildren<Rigidbody>().transform;
        tumbler.GetComponent<Rigidbody>().sleepThreshold = 0.0001f;
        Invoke ("AddRandomForce", 1);
    }
    
	void Update () {
        // removed due to undesired jumping effect
        float dist = Vector3.Distance(transform.position, tumbler.position);
        if (dist > 0.5f)
        {
            tumbler.position = transform.position;
            tumbler.rotation = Quaternion.identity;
        }
        if (!isAddingForce)
        {
            tumbler.position = Vector3.Lerp(tumbler.position, transform.position, Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (isAddingForce)
        {
            if (Mathf.Abs(Time.time - startedTime) < forceDuration)
            {
                 tumbler.GetComponent<Rigidbody>().AddRelativeTorque(forceDirection * forceIntensity * direction, ForceMode.Force);
            }
            else // time over
            {
                isAddingForce = false;
            }
        }
    }
   

    void AddRandomForce()
    {
        isAddingForce = true;
        startedTime = Time.time;
        direction *= -1;
        Invoke("AddRandomForce", waitTime);
    }

   /* IEnumerator ForceCoroutine()
    {
        float startedTime = Time.time;
        direction *= -1;
        isAddingForce = true;
        while (Mathf.Abs(Time.time - startedTime) < forceDuration)
        {
            tumbler.GetComponent<Rigidbody>().AddRelativeTorque(forceDirection * forceIntensity * direction, ForceMode.Force);
            yield return new WaitForSeconds(0.02f);
        }
        isAddingForce = false;
    }*/

}
