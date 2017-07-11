using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuest {
    /// <summary>
    /// Unique ID that defines the quest.
    /// </summary>
    string QuestID { get; }
    /// <summary>
    /// A short description of the quest.
    /// </summary>
    string ShortDescription { get; }
    /// <summary>
    /// XP the player will be rewarded with when he completes this quest
    /// </summary>
    int XPReward { get; }
    /// <summary>
    /// Item the player will be rewarded with when he completes this quest
    /// </summary>
    IITem Reward { get; }
    /// <summary>
    /// Percentage of the quest progression (given in a range between 0 and 100)
    /// </summary>
    int QuestProgression { get; }
    /// <summary>
    /// Wether the conditions to start the quest are met or not.
    /// </summary>
    bool IsAvailable { get; }
    /// <summary>
    /// Wether the conditions to complete the quest are met or not.
    /// </summary>
	bool IsCompleted { get; }
    
}
