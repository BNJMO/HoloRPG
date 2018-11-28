using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

public class BasicSpell : AbstractSpell {

	protected override void UpdatePosition()
    {
       // DestroyImmediate(gameObject.GetComponent<WorldAnchor>());
        transform.Translate(Direction * Speed * Time.deltaTime);
      //  gameObject.AddComponent<WorldAnchor>();
    }
}
