using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INpc  {
    /// <summary>
    /// Array of all the quests this npc has to offer
    /// </summary>
    IQuest[] Quests { get; }
    /// <summary>
    /// Last given quest
    /// </summary>
    IQuest CurrentQuest { get; }
}
