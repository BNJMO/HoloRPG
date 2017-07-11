using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundAndEffectManager : HoloToolkit.Unity.Singleton<SoundAndEffectManager> {

    public const string MIXER_NAME = "HoloRPG_Mixer";

    private GameObject levelUp_Effect;
    private GameObject newRegion_Effect;

    private AudioSource background_AudioSource;
    private AudioSource combat_AudioSource;

    [System.Serializable]
    public class RegionSoundtrack
    {
        [SerializeField] public string regionName;
        [SerializeField] public AudioClip regionSoundTrack;
    }

    [SerializeField] RegionSoundtrack[] regionSoundtracks;

    protected override void Awake()
    {
        base.Awake();

        var mixer = Resources.Load<UnityEngine.Audio.AudioMixer>(SoundAndEffectManager.MIXER_NAME);
        var group = mixer.FindMatchingGroups("Background")[0];

        background_AudioSource  = Utils.AddAudioListener(gameObject, false, 0.3f, true, group);
        combat_AudioSource      = Utils.AddAudioListener(gameObject); // not used yet

        levelUp_Effect = Resources.Load<GameObject>("LevelUP_effect");
        if (levelUp_Effect == null) Debug.LogError("levelUp_effect not found");

        newRegion_Effect = Resources.Load<GameObject>("NewRegion_effect");
        if (newRegion_Effect == null) Debug.LogError("newRegion_Effect not found");
    }

	void Start()
    {
        GameManger.Instance.PlayerLevelUp += OnPlayerLevelUp;
        GameManger.Instance.RegionEntered += OnRegionEntered;
    }

    private void OnPlayerLevelUp(int level)
    {
        TriggerLevelUpEffect();
    }

    private void OnRegionEntered(IRegion region, bool firstTime)
    {
        Debug.Log("S&A entered : " + region.RegionName);
        if (firstTime == true)
        {
            TriggerRegionDiscoveredEffect();
        }

        // find and play appropriate region soundtrack
        bool found = false;
        foreach(RegionSoundtrack rst in regionSoundtracks)
        {
            if (rst.regionName == region.RegionName)
            {
                background_AudioSource.Stop();
                background_AudioSource.clip = rst.regionSoundTrack;
                background_AudioSource.Play();
                found = true;
                break;
            }
        }
        if (found == false) Debug.Log("Region Track " + region.RegionName + " not found");
    }

    private void TriggerLevelUpEffect()
    {
         // play new region effect and sound
        Vector3 effectPos = CameraHelper.Stats.camPos + CameraHelper.Stats.camLookDir.normalized;
        Instantiate(newRegion_Effect, effectPos, Quaternion.identity).transform.parent = Camera.main.transform;
    }

    private void TriggerRegionDiscoveredEffect()
    {
        // play level up effect and sound
        Vector3 effectPos = CameraHelper.Stats.camPos + CameraHelper.Stats.camLookDir.normalized;
        Instantiate(levelUp_Effect, effectPos, Quaternion.identity);
    }
}
