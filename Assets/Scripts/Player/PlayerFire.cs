using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class PlayerFire : MonoBehaviour {
    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileDuration = 4;


    void Start()
    {
        InteractionManager.SourcePressed += InteractionManager_SourcePressed;
        InteractionManager.SourceReleased += InteractionManager_SourceReleased;
    }


    void Update()
    {
        if (Input.GetKeyDown("f"))
            Fire();
    }


    // from Interaction Manager
    private void InteractionManager_SourcePressed(InteractionSourceState state)
    {
      /*  if (state.source.kind == InteractionSourceKind.Controller)
        {


        }*/
        Fire();
    }

    private void InteractionManager_SourceReleased(InteractionSourceState state)
    {
        if (state.source.kind == InteractionSourceKind.Controller)
        {

        }
        
    }



    private void Fire()
    {
        MakePorjectile.InstantiateObj("FireBall", 5, 3);
    }
	
}
