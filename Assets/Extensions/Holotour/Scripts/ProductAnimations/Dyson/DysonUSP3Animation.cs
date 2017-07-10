using UnityEngine;

public class DysonUSP3Animation : CombinedAnimation {

    [Header("Dyson")]
    [SerializeField] private Transform dysonContainer;
    [SerializeField] private Transform endPosition;

    [SerializeField] private Transform plane;

    void Start()
    {
        dysonContainer.gameObject.SetActive(false);
        plane.position = CameraHelper.Stats.groundPos;
    }

    protected override void OnAnimationStarted() {
        dysonContainer.gameObject.SetActive(true);
        dysonContainer.localScale = Vector3.zero;

        AnimateThis.With(dysonContainer).Transformate()
            .FromScale(0)
            .ToScale(1)
            .Ease(AnimateThis.EaseInOutSmooth)
            .Delay(6)
            .Duration(6)
            .Start();

        AnimateThis.With(dysonContainer).Transformate()
            .FromPosition(Vector3.zero)
            //.ToPosition(Vector3.up * 0.5f)
            .ToPosition(endPosition.localPosition)            
            .Ease(AnimateThis.EaseInOutSmooth)
            .Delay(6)
            .Duration(8)
            .OnEnd(() => dysonContainer.gameObject.GetComponentInChildren<ParticleSystem>().Play())
            .Start();

        AnimateThis.With(dysonContainer).Transformate()
            .FromRotation(Quaternion.identity)
            .ToRotation(Quaternion.Euler(0, 120, 0))
            .Ease(AnimateThis.EaseInOutSmooth)
            .Delay(6)
            .Duration(20)
            .Start();

    }
    protected override void OnAnimationRunning() { }
    protected override void OnAnimationEnded() {
        AnimateThis.With(dysonContainer).Transformate()
            .ToScale(0)
            .Ease(AnimateThis.EaseInOutSmooth)
            .Duration(0.5f)
            .OnEnd(() => dysonContainer.gameObject.SetActive(false))
            .Start();
    }

}
