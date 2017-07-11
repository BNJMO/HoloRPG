using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour {

	private TextMesh myTextMesh;


    void Awake()
    {
        myTextMesh = GetComponentInChildren<TextMesh>();
    }

    public void SetQuestWaiting()
    {
        myTextMesh.text = "?";
    }

    public void SetQuestCompleted()
    {
        myTextMesh.text = "!";
    }

    public void SetNoQuest()
    {
        myTextMesh.text = "";
    }
}
