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

    public static Quaternion GetRndStandRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }
    
    /// <summary>
    /// Plays a random clip from a given clip soundbank on a given AudioSource component
    /// </summary>
    public static void PlayRandomSound(AudioSource source, AudioClip[] clips)
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
    
    public static void PlaySound(AudioSource source, AudioClip clip)
    {
        if (source != null)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource attached!");
        }
    } 
    
    public static void StopSound(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
        }
        else
        {
            Debug.LogWarning("No AudioSource attached!");
        }
    } 


    /// <summary>
    /// Gets the distance between two objects, set a the same height.
    /// </summary>
    public static float GetRelativeDistance(Vector3 position1, Vector3 position2)
    {
        // set the height of pos1 as for pos2
        Vector3 relativePosition1 = new Vector3
        (
            position1.x,
            position2.y,
            position1.z
        );

        return Vector3.Distance(relativePosition1, position2);
    }


    #region AddAudioListener Overrides
    public static AudioSource AddAudioListener(GameObject toGameObject)
    {
        AudioSource aS = toGameObject.AddComponent<AudioSource>();
        aS.playOnAwake = false;
        return aS;
    }

    public static AudioSource AddAudioListener(GameObject toGameObject, bool is3D)
    {
        AudioSource aS = toGameObject.AddComponent<AudioSource>();
        aS.playOnAwake = false;
        if (is3D == true)
        {
            aS.spatialBlend = 1.0f;
        }
        else
        {
            aS.spatialBlend = 0.0f;
        }
        return aS;
    }

    public static AudioSource AddAudioListener(GameObject toGameObject, bool is3D, float volume)
    {
        AudioSource aS = toGameObject.AddComponent<AudioSource>();
        aS.playOnAwake = false;
        if (is3D == true)
        {
            aS.spatialBlend = 1.0f;
        }
        else
        {
            aS.spatialBlend = 0.0f;
        }
        aS.volume = volume;
        return aS;
    }

    public static AudioSource AddAudioListener(GameObject toGameObject, bool is3D, float volume, bool isLoop)
    {
        AudioSource aS = toGameObject.AddComponent<AudioSource>();
        aS.playOnAwake = false;
        if (is3D == true)
        {
            aS.spatialBlend = 1.0f;
        }
        else
        {
            aS.spatialBlend = 0.0f;
        }
        aS.volume = volume;
        aS.loop = isLoop;
        return aS;
    }

#endregion
}
