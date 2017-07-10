using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterZoneAnchor : PoiAnchor {
    public override void OnGazeEnter(RaycastHit hitinfo)
	{
        if (AnchorEditorUi.Instance != null)
        {
            AnchorEditorUi.Instance.GazedAnchor = this;
            AnchorEditorUi.Instance.IsGazedAnchorMonster =  true;
        }
	}

    public override void OnGazeExit(RaycastHit hitinfo)
	{
        if (AnchorEditorUi.Instance != null)
        {
            AnchorEditorUi.Instance.GazedAnchor = null;
            AnchorEditorUi.Instance.IsGazedAnchorMonster = false;
        }
    }
	
}
