using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

    /// <summary>
    /// Gets a random index for a given array
    /// </summary>
	public static int GetRndIndex(int arrayLength)
    {
        return Random.Range(0, arrayLength);
    } 
    
    /// <summary>
    /// Plays a random clip from a given clip soundbank on a given AudioSource component
    /// </summary>
    public static void PlaySound(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length != 0)
        {
            if (source != null)
            {
                source.clip = clips[Utils.GetRndIndex(clips.Length)];
                source.Play();
            }
            else
            {
                Debug.LogWarning("No AudioSource attached!");
            }
            
        }
        else
        {
            Debug.LogWarning("No audio clip attached!");
        }
    }  
}
