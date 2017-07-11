using UnityEngine;
using UnityEngine.VR.WSA;

public interface IAnchor 
{

    string Region { get; }

    bool IsVisible { get; set; }

    Vector3 AvatarTargetPosition { get; }

    Vector3 AnchorPosition { get;  set; }

    Quaternion AnchorRotation { get; set; }

    GameObject GameObject { get; }

}

