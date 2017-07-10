using UnityEngine;

public class AudioPlayable : MonoPlayable {


    [Header("Audio Snippet")]

    [SerializeField]
    private AudioClip audioClip;

    public override float Duration
    { get { return audioClip.length + paddingBefore + paddingAfter; } protected set { } }

    public override bool IsPlaying
    { get { return Time.time >= TimeStarted && Time.time <= timeFinishPlaying; } protected set { } }

    public override bool StoppedPlaying
    { get; protected set; }

    public override float TimeStarted
    { get; protected set; }

    private AudioSource audioSource;
    private float timeStartPlaying = float.MinValue;
    private float timeFinishPlaying = float.MinValue;
    private bool wasPlaying;
    private bool played;

    public override void Update()
    {
        StoppedPlaying = false;
        bool isPlaying = IsPlaying;
        if (isPlaying && !played && Time.time >= timeStartPlaying)
        {
            played = true;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (!isPlaying && wasPlaying) {
            audioSource.Stop();
            audioSource.clip = null;
        }
        StoppedPlaying = (wasPlaying == true && isPlaying == false);
        wasPlaying = isPlaying;
    }

    public override bool Play()
    {
        if (!IsPlaying) {
            if (FindObjectOfType<AvatarAgent>() == null) {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
            } else
            {
                audioSource = FindObjectOfType<AvatarAgent>().GetComponent<AudioSource>();
            }
            Prepare();
            played = false;
            TimeStarted = Time.time;
            timeStartPlaying = TimeStarted + paddingBefore;
            timeFinishPlaying = TimeStarted + paddingBefore + audioClip.length + paddingAfter;
            return true;
        } else
        {
            return false;
        }
    }

    public override bool Stop()
    {
        if (IsPlaying)
        {
            timeStartPlaying = float.MinValue;
            timeFinishPlaying = float.MinValue;
            return true;
        } else
        {
            return false;
        }
    }

    public override void Prepare()
    {
        // Makes HoloLens stutterting
        //audioClip.LoadAudioData();
    }

    public override void Free()
    {
        // Makes HoloLens stutterting
        //audioClip.UnloadAudioData();
    }
}
