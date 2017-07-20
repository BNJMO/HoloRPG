using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour {

	private TextMesh myTextMesh;


    void Awake()
    {
        myTextMesh = GetComponentInChildren<TextMesh>();
    }

    public void SetQuestAvailable()
    {
        myTextMesh.text = "?";
    }

    public void SetQuestPending()
    {
        myTextMesh.text = "!";
    }

    public void SetNoQuest()
    {
        myTextMesh.text = "";
    }
}
