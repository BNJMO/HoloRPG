using UnityEngine;
using UnityEngine.VR.WSA;

public interface IAnchor 
{

    bool IsActive
    { get; set; }

    bool IsVisible
    { get; }

    Vector3 AvatarTargetPosition
    { get; }

    Vector3 AnchorPosition
    { get;  set; }

    Quaternion AnchorRotation
    { get; set; }

    GameObject GameObject
    { get; }

}

