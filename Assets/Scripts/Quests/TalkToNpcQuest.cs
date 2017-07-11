using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNpcQuest : AbstractQuest {

    
    public bool HasTalkedToNPC { get; set; }

    void Awake()
    {
        HasTalkedToNPC = false;
    }

    protected override int GetQuestProgression()
    {
        return HasTalkedToNPC == true ? 100 : 0;
    }

    protected override bool QuestAvailable()
    {
        return true;
    }

    protected override bool QuestCompleted()
    {
        return HasTalkedToNPC == true;
    }

}
