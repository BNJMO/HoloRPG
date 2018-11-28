using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireBall : AbstractSpell {

    protected override void UpdatePosition()
    {
       // DestroyImmediate(gameObject.GetComponent<WorldAnchor>());
        transform.Translate(Direction * Speed * Time.deltaTime);
       // gameObject.AddComponent<WorldAnchor>();
    }

}
