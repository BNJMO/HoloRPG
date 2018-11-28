using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireBall : AbstractSpell {

    protected override void UpdatePosition()
    {
        DestroyImmediate(gameObject.GetComponent<UnityEngine.XR.WSA.WorldAnchor>());
        transform.Translate(Direction * Speed * Time.deltaTime);
        gameObject.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
    }

}
