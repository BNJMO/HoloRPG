using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component with which the visibility of a GameObject can be controller
/// </summary>
public class VisibiliterParticle : MonoBehaviour {

    public bool IsVisible { get { return isVisible; } set { SetVisibility(value); } }

    private ParticleSystem[] myParticleSystems;
    
    public Condition VisibilityCondition;

    private bool isVisible = true;

	void Awake()
    {
        myParticleSystems = GetComponentsInChildren<ParticleSystem>();

    }

    void Start()
    {
        if (VisibilityCondition == null) Debug.LogError("Warning! Visibiliter needs a VisibilityCondition");
    }

    void Update()
    {
        // Update visibility only on the frame when it changed
        if (isVisible != VisibilityCondition.IsTrue)
        {
            IsVisible = VisibilityCondition.IsTrue;
        } 
    }

    public void SetVisibility(bool visible)
    {
        if (myParticleSystems.Length != 0)
        {
            foreach(ParticleSystem particleSystem in myParticleSystems)
            {
                if ((visible == false) && (isVisible == true))
                {
                    particleSystem.Stop();
                }
                else if ((visible == true) && (isVisible == false))
                {
                    particleSystem.Play();
                }
                
            }
        }
        isVisible = visible;
    }
}
