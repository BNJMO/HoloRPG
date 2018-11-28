using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

	private TextMesh myTextMesh;

    void Awake()
    {
        myTextMesh = GetComponentInChildren<TextMesh>();
        if (myTextMesh == null) Debug.LogError("No TextMesh component found");
    }

    public void Write(string title, List<string> list)
    {
        myTextMesh.text = "<b>   " + title + "</b>";
        for (int i = 0; i < list.Count; i++)
        {
            myTextMesh.text += "\n- " + list[i];
        }
    }

    public void Write(string title)
    {
        myTextMesh.text = "<b>   " + title + "</b>";
    }
}
