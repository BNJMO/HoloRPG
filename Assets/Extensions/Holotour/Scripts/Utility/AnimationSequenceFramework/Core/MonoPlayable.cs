using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoPlayable : MonoBehaviour, IPlayable
{

    [Header("Margin / Padding")]

    [SerializeField]
    protected float marginBefore;
    public float MarginBefore
    { get { return marginBefore; } }

    [SerializeField]
    protected float marginAfter;
    public float MarginAfter
    { get { return marginAfter; } }

    [SerializeField]
    protected float paddingBefore;
    public float PaddingBefore
    { get { return paddingBefore; } }

    [SerializeField]
    protected float paddingAfter;
    public float PaddingAfter
    { get { return paddingAfter; } }

    public abstract bool IsPlaying { get; protected set; }
    public abstract bool StoppedPlaying { get; protected set; }
    public abstract float Duration { get; protected set; }
    public abstract float TimeStarted { get; protected set; }

    public abstract bool Play();
    public abstract bool Stop();
    public abstract void Prepare();
    public abstract void Free();
    public abstract void Update();
}
