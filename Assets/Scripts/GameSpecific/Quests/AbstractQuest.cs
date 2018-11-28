using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractQuest : MonoBehaviour, IQuest {

    public string QuestID { get { return questID; } }
    public string ShortDescription { get { return questDescription; } }
    public int XPReward { get { return xpReward; } }
    public IITem Reward { get { return reward; } }
    public int QuestProgression { get { return GetQuestProgression(); } }
    public bool IsAvailable { get { return QuestAvailable(); } }
    public bool IsCompleted { get { return QuestCompleted(); } }
    


    private AudioSource npcAudioSource;
    private AbstracNpc boundNpc;

    [SerializeField] private string questID = "Q_objectif";
    [SerializeField] private string questDescription = "Save the world.";
    [SerializeField] private int xpReward = 50;
    [Tooltip("Leave empty if no rewarded is given")]
    [SerializeField] private AbstractItem reward;

    [Header("Bank")]
    [SerializeField] public AudioClip welcomeClip;
    [SerializeField] public AudioClip mainQuestClip;
    [SerializeField] public AudioClip pendingClip;
    [SerializeField] public AudioClip completedClip;
    [SerializeField] public AudioClip confirmationClip;
    
    // abstract methods 
    protected abstract bool QuestAvailable();
    protected abstract bool QuestCompleted();
    protected abstract int GetQuestProgression();


    /// <summary>
    /// Populates needed components from npc
    /// </summary>
    /// <param name="npc"></param>
    public virtual void Populate(AbstracNpc npc)
    {
        boundNpc = npc;
        npcAudioSource = npc.GetComponent<AudioSource>();
    }
}
