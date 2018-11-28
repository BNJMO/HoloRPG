using UnityEngine;
using UnityEngine.VR.WSA.Input;

public interface IAirtapable
{
    void OnTap(InteractionSourceKind source, int tapCount, Ray headRay);
    void OnHoldStart(InteractionSourceKind source, Ray headRay);
    void OnHoldFinish(InteractionSourceKind source, Ray headRay);
}