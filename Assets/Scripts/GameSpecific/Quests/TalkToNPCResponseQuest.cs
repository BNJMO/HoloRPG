using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Response quest to original TalkToNPCQuest. 
/// Make sure this shares the same id as the original quest + "_Response".
/// This Quest must declare its completion manually (not from NPC)
/// </summary>
public class TalkToNPCResponseQuest : AbstractQuest {

    // questSourceID not declared locally due to usage of the quests as interface

    void Awake()
    {
        // This should be instantianted anyway only when QuestAvailable is true
        string questSourceID = QuestID.Substring(0, QuestID.Length - "_Response".Length);
        TalkToNpcQuest questSource = (TalkToNpcQuest) QuestManager.Instance.GetQuestInstance(questSourceID);
        questSource.HasTalkedToNPC = true;
    }

    void Start()
    {
        GameManger.Instance.QuestCompleted += OnQuestCompleted;
    }

    protected override int GetQuestProgression()
    {
        return 0;
    }

    protected override bool QuestAvailable()
    {
        // make sure soure quest is running
        string questSourceID = QuestID.Substring(0, QuestID.Length - "_Response".Length);
        return QuestManager.Instance.HasTakenQuest(questSourceID) == true;
    }

    protected override bool QuestCompleted()
    {
        // directly complete quest after recieving it
        return false;
    }

    private void OnQuestCompleted(IQuest completedQuest)
    {
        string questSourceID = QuestID.Substring(0, QuestID.Length - "_Response".Length); 
        if (completedQuest.QuestID == questSourceID)
        {
            QuestManager.Instance.OnQuestCompleted(this);
        }
    }
}
