using UnityEngine;

public class CustomDurationAudioPlayable : MonoPlayable
{
    [Header("Audio Snippet")]

    [SerializeField]
    private AudioClip audioClip;

    [Tooltip("this is the amount of time the animation will be \"playing\", regardless of how long the audio clip is.")]
    [SerializeField]
    private float customDuration = 0f;

    public override float Duration
    { get { return paddingBefore + customDuration + paddingAfter; } protected set { } }

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

    private bool funkyOverrideBool = false;

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
        StoppedPlaying = (wasPlaying == true && isPlaying == false);
        wasPlaying = isPlaying;
    }

    public override bool Play()
    {
        if (!IsPlaying)
        {
            if (FindObjectOfType<AvatarAgent>() == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {

                audioSource = FindObjectOfType<AvatarAgent>().GetComponent<AudioSource>();
            }
            Prepare();
            played = false;
            TimeStarted = Time.time;
            timeStartPlaying = TimeStarted + paddingBefore;
            timeFinishPlaying = TimeStarted + paddingBefore + customDuration + paddingAfter;
            return true;
        }
        else
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
        }
        else
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
