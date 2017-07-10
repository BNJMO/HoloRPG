using UnityEngine;

public class DebugLogFor3D : HoloToolkit.Unity.Singleton<DebugLogFor3D>
{
    private TextMesh myUIText;
    private string myText;

    private new void Awake()
    {
        base.Awake();
        myUIText = GetComponent<TextMesh>();
    }

    private void Start()
    {
        myText = "";
    }

    private void LateUpdate()
    {
        myUIText.text = myText;
        myText = "";
    }

    public void Log(string newText)
    {
        myText += "\n" + newText;
    }
}
