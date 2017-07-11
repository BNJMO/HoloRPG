using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSelectionAnchor : PoiAnchor {

    
    public enum State
    {
        genreSelection,

        level1,
        level2

    }
    public State state;

    

    public override void OnGazeEnter(RaycastHit hitinfo)
	{
        if (AnchorEditorUi.Instance != null)
        {
            AnchorEditorUi.Instance.GazedAnchor = this;
            AnchorEditorUi.Instance.IsGazedAnchorNPC =  true;
        }
	}

    public override void OnGazeExit(RaycastHit hitinfo)
	{
        if (AnchorEditorUi.Instance != null)
        {
            AnchorEditorUi.Instance.GazedAnchor = null;
            AnchorEditorUi.Instance.IsGazedAnchorNPC = false;
        }
    }
}
