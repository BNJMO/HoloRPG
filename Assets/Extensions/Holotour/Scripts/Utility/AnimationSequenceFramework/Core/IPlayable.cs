using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayable {

    bool IsPlaying
    { get; }

    bool StoppedPlaying
    { get; }

    float Duration
    { get; }

    float TimeStarted
    { get;  }

    float MarginBefore
    { get; }

    float MarginAfter
    { get; }

    float PaddingBefore
    { get; }

    float PaddingAfter
    { get; }

    bool Play();

    bool Stop();

    void Prepare();

    void Free();

    void Update();
}
