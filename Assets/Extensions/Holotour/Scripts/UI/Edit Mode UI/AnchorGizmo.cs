using UnityEngine;

public interface IAnchorGizmo
{
    Vector3 Position
    { get;  set; }

    Quaternion Rotation
    { get; set; }

    Vector3 Forward
    { get; }

    string Hint
    { get; set; }

    bool Highlighted
    { get; set;  }


    bool Visible
    { get; set; }
}

public class AnchorGizmo : MonoBehaviour, IAnchorGizmo
{
    public virtual Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public virtual Quaternion Rotation
    {
        get
        {
            return transform.rotation;
        }

        set
        {
            transform.rotation = value;
        }
    }

    public Vector3 Forward
    {
        get {
            return transform.forward;
        }
    }

    public string Hint
    {
        get
        {
            return textMesh != null ? textMesh.text : "";
        }
        set
        {
            if (textMesh != null)
            {
                textMesh.text = value;
            }
        }
    }

    public virtual bool Highlighted
    { get; set; }

    public bool Visible
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }


    private TextMesh textMesh;

    void Start () {
        textMesh = GetComponentInChildren<TextMesh>();
	}
}
