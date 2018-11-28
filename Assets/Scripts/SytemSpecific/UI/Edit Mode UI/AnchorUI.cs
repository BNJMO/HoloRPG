using UnityEngine;

public class AnchorUI : MonoBehaviour
{
    /// <summary>
    /// offset distance from the top of the anchor 
    /// </summary>
    public Vector3 offset = new Vector3(0f, 0.35f, 0f);

    [SerializeField]
    private TextMesh textMesh;

    private IAnchor attachedAnchor;

    private LookAtPlayer lookAtPlayer;

    private void Start()
    {
        if(attachedAnchor == null) attachedAnchor = transform.GetComponentInParent<IAnchor>();

        if(lookAtPlayer == null) gameObject.AddComponent<LookAtPlayer>();

        InitializeUIText();
    }

	void Update(){
        if (AnchorManager.Instance != null)
        {
            textMesh.color = Color.white;
            int index = AnchorManager.Instance.AnchorList.FindIndex(x => x == attachedAnchor);// FIXME Make more efficient
            if (index == 0)
            {
                SetUIText("Start");
            }
            else
            {
                SetUIText(index + "");
            }
        }
        
	}

    private void InitializeUIText()
    {
        if (textMesh == null)
        {
            textMesh = gameObject.AddComponent<TextMesh>();
            if (textMesh)
            {
                textMesh.characterSize = 0.025f;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;
                textMesh.fontSize = 32;
                // make the index hover just above the flag. assumes the flag is the first child.
                // a better solution would probably be to encapsulate all child mesh bounds and set the position to y-max + offset instead
                transform.position = transform.InverseTransformPoint(attachedAnchor.GameObject.transform.GetChild(0).position + offset);
            }
            else
            {
                Debug.LogError(attachedAnchor.GameObject.name + "couldn't initialize UI Text: failed to create TextMesh component.");
            }
        }
    }

    public void SetUIText(string newText)
    {
        textMesh.text = newText;
    }
}
