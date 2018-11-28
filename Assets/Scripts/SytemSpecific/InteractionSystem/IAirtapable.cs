using UnityEngine;


public interface IAirtapable
{
    void OnTap(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay);
    void OnHoldStart(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);
    void OnHoldFinish(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);
}