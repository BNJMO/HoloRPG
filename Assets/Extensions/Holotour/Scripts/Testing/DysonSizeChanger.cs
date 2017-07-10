using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DysonSizeChanger : MonoBehaviour, IKeywordCommandProvider {
    float factor = 0.05f;
	

    void OnEnable()
    {
        Debug.Log ("hello");
        KeywordCommandManager.Instance.AddKeywordCommandProvider (this);
    }
    public void IncreaseX()
    {
        Vector3 oldScale = transform.localScale;
        float newX = oldScale.x + factor;
        transform.localScale = new Vector3 (newX, oldScale.y, oldScale.z);
        Debug.Log ("X");
    }

    public void IncreaseY()
    {
        Vector3 oldScale = transform.localScale;
        float newY = oldScale.y + factor;
        transform.localScale = new Vector3 (oldScale.x, newY, oldScale.z);
    }

    public void IncreaseZ()
    {
        Vector3 oldScale = transform.localScale;
        float newZ = oldScale.z + factor;
        transform.localScale = new Vector3 (oldScale.x, oldScale.y, newZ);
    }

    public void DecreaseX()
    {
        Vector3 oldScale = transform.localScale;
        float newX = oldScale.x - factor;
        transform.localScale = new Vector3 (newX, oldScale.y, oldScale.z);
        Debug.Log ("X");
    }

    public void DecreaseY()
    {
        Vector3 oldScale = transform.localScale;
        float newY = oldScale.y - factor;
        transform.localScale = new Vector3 (oldScale.x, newY, oldScale.z);
    }

    public void DecreaseZ()
    {
        Vector3 oldScale = transform.localScale;
        float newZ = oldScale.z - factor;
        transform.localScale = new Vector3 (oldScale.x, oldScale.y, newZ);
        Debug.Log ("here");
    }

    public List<KeywordCommand> GetSpeechCommands()
    {
        List<KeywordCommand> result = new List<KeywordCommand> ();

        result.Add(new KeywordCommand(() => { IncreaseX(); }, "plus X", KeyCode.G));
        result.Add(new KeywordCommand(() => { IncreaseY(); }, "plus Y", KeyCode.H));
        result.Add(new KeywordCommand(() => { IncreaseZ(); }, "plus Z", KeyCode.J));
        result.Add(new KeywordCommand(() => { DecreaseX(); }, "minus X", KeyCode.Alpha4));
        result.Add(new KeywordCommand(() => { DecreaseY(); }, "minus Y", KeyCode.Alpha5));
        result.Add(new KeywordCommand(() => { DecreaseZ(); }, "minus Z", KeyCode.Alpha6));

        return result;
    }
}
