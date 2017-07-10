using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulaSoundbank : MonoBehaviour {

	[Header ("Avatar")]
	[SerializeField]
	private AvatarAgent paula;

	[Header ("Sound Bank - Welcome")]

	[SerializeField] public AudioClip[] welcomeTracks;
    [SerializeField] public AudioClip[] welcomeEnforcementTracks;
    [SerializeField] public AudioClip[] acceptTracks;

    [Header("Sound Bank - Not Accept Tour")]

    [SerializeField] public AudioClip[] confirmTracks;
    [SerializeField] public AudioClip[] confirmEnforcementTracks;

    [Header("Sound Bank - Navigation")]

    [SerializeField] public AudioClip[] halfwayWaitingTracks;
    [SerializeField] public AudioClip[] nextHighlightTracks;
	[SerializeField] public AudioClip[] lastHighlightTracks;
    [SerializeField] public AudioClip[] nextHighlightEnforcementTracks;
    [SerializeField] public AudioClip[] lastHighlightEnforcementTracks;
    [SerializeField] public AudioClip[] tourIsOverTracks;


    [Header("Sound Bank - Cancelling Tour")]

    [SerializeField] public AudioClip[] confirmTourCancelingTracks;
    [SerializeField] public AudioClip[] changedMindAcceptedProceedingTracks;
    [SerializeField] public AudioClip[] tourIsCancelledTracks;
    [SerializeField] public AudioClip[] tourContinueAcceptedTracks;
    [SerializeField] public AudioClip[] cancelTrakcs;

    [Header("Sound Bank - Goodbye")]

    [SerializeField] public AudioClip[] goodbyeTracks;


	public void PlayNextTrack(AudioClip clip) //FIXME
	{
		paula.PlayClip(clip);
	}
}
