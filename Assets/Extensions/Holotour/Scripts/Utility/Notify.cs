using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notify : HoloToolkit.Unity.Singleton<Notify>
{
    
	struct Message {
		internal string text;
		internal float displaytime;

		internal Message(string text, float displaytime) {
			this.text = text;
			this.displaytime = displaytime;
		}
	}

	[SerializeField]
	private AudioClip ShowMessageSoundFx;

	private AudioSource audioSource;
	private TextMesh text;
	private List<Message> stack = new List<Message>();
	private float busyUntil = 0;

    new void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMesh>();
		audioSource = GetComponent<AudioSource> ();
    }

	public static void Show(string text) {
		Notify.Show (text, 2);
	}

	public static void Beep() {
		Notify.Instance.BeepInternal ();
	}

	public static void Show(string text, float displaytime) {
		Notify.Instance.ShowInternal (text, displaytime);
	}

    public static void Debug(string text)
    {
        Notify.Instance.DebugInternal(text);
    }

    private void ShowInternal(string text, float displaytime) {
		stack.Add(new Message(text, displaytime));
	}

    private void DebugInternal(string text)
    {
        stack.Clear();
        busyUntil = Time.time + 10;
        this.text.text = text;
    }

    private void BeepInternal() {
		if (audioSource != null && ShowMessageSoundFx != null) {
            audioSource.Stop();
			audioSource.clip = ShowMessageSoundFx;
			audioSource.Play ();
		}
	}

	void Update() {
		if (Time.time > busyUntil) {
			if (stack.Count == 0) {
				text.text = "";
				busyUntil = 0;
			} else {
				Message nextMessage = stack [0];
				stack.RemoveAt(0);
				busyUntil = Time.time + nextMessage.displaytime;
				text.text = nextMessage.text;

				AnimateThis.With (this).CancelAll ();
				AnimateThis.With (this)
					.Transformate ()
					.FromScale (0)
					.ToScale (1)
					.Duration (2)
					.Ease (AnimateThis.EaseOutElastic)
					.Start ();
				
				BeepInternal ();
			}
		}
	}
}
