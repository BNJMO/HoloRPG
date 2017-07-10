using System;
using UnityEngine;

public class AnimationSequencePlayer : MonoPlayable
{

    private const int NONE = -1;

    [Header("Animation Sequences To Play")]
    [SerializeField]
    private MonoPlayable[] animationSequences;

    private IPlayable[] playables;

    private int currentSequence = NONE;

    private float duration;

    public override bool IsPlaying
    { get; protected set; }

    public override bool StoppedPlaying
    { get; protected set; }

    public override float Duration
    { get; protected set; }

    public override float TimeStarted
    { get; protected set; }

    private bool wasPlaying;
    private float timeToProceed;

    void Start () {
        playables = GetPlayables();
        float duration = marginBefore + marginAfter + paddingBefore + paddingAfter;
        foreach (IPlayable p  in playables)
        {
            duration += p.Duration + +p.MarginBefore + p.MarginAfter;
        }
        Duration = duration;
	}

    protected virtual IPlayable[] GetPlayables()
    {
        IPlayable thisObj = this;
        return Array.FindAll(animationSequences, (x) => (IPlayable) x != thisObj);
    }

    public override void Update () {
        StoppedPlaying = false;
        if (IsPlaying)
        {
            if (Time.time > timeToProceed)
            {
                if (currentSequence == NONE || !playables[currentSequence].IsPlaying)
                {
                    if (currentSequence == playables.Length - 1)
                    {
                        currentSequence = NONE;
                        IsPlaying = false;
                    }
                    else
                    {
                        currentSequence++;
                        playables[currentSequence].Play();
                        timeToProceed = Time.time + playables[currentSequence].Duration + playables[currentSequence].MarginAfter +
                            (playables.Length - 1 != currentSequence ? playables[currentSequence + 1].MarginBefore : 0);
                    }

                }
            }
        } else
        {
            if (wasPlaying) {
                Free();
            }
        }
        StoppedPlaying = (wasPlaying == true && IsPlaying == false);
        wasPlaying = IsPlaying;
	}

    public override bool Play() {
        if (IsPlaying)
        {
            return false;
        }
        Prepare();
        IsPlaying = true;
        TimeStarted = Time.time;
        timeToProceed = TimeStarted + marginBefore + paddingBefore + playables[0].MarginBefore;
        currentSequence = NONE;
        return true;
    }

    public override bool Stop()
    {
        if (!IsPlaying)
        {
            return false;
        }
        Free();
        IsPlaying = false;
        return playables[currentSequence].Stop();
    }

    public override void Prepare()
    {
        foreach(IPlayable p in playables)
        {
            p.Prepare();
        }
    }

    public override void Free()
    {
        foreach (IPlayable p in playables)
        {
            p.Free();
        }
    }
}
