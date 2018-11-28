using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

/// <summary>
/// Updates the list of valid keyword commands.
/// </summary>
public class ListRegisteredKeywords : MonoBehaviour {

	private Text text;
    private TextMesh textMesh;

	void OnEnable() {
		text = GetComponent<Text> ();
        textMesh = GetComponent<TextMesh> ();	
		if ((text) || (textMesh)) {
			StartCoroutine ("OnTick");
		}
	}

	void OnDisable() {
		StopAllCoroutines ();
	}

	private IEnumerator OnTick()
	{
		while (gameObject.activeInHierarchy)
		{
            yield return new WaitForSeconds(0.25f);

			StringBuilder sb = new StringBuilder();
			int line = 1;
			foreach (KeywordCommand keyword in KeywordCommandManager.Instance.GetKeywordCommands().FindAll(c => c.IsActive && c.IsVisible))
			{
				sb.Append (line++);
				sb.Append (" - ");
				sb.Append (keyword.Keyword);
				sb.Append ("\n");
			}
            if (text) {
                text.text = sb.ToString ();
            } else if (textMesh) {
                textMesh.text = sb.ToString ();
            }
			
		}
	}
}
