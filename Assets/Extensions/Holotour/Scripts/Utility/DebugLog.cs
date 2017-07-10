using UnityEngine.UI;

public class DebugLog : HoloToolkit.Unity.Singleton<DebugLog>
{
    private Text myUIText;
    private string myText;

    private new void Awake()
    {
        base.Awake();
        myUIText = GetComponent<Text>();
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

    public void LogIntern(string newText)
    {
        myText += "\n" + newText;
    }

    public static void Log(string newText)
    {
        Instance.LogIntern(newText);
    }
}
