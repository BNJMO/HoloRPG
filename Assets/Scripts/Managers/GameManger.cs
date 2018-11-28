using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// More like an event manager ?
public class GameManger : Singleton<GameManger> {

    public bool IsInUserMode { get { return ApplicationStateManager.IsUserMode == true; } }

    #region EVENT

    // Reduce player's HP
    public event Action<int> PlayerGotHit;
    public void InvokePlayerGotHit(int damage)
    {
        if (PlayerGotHit != null)
        {
            PlayerGotHit.Invoke(damage);
            UpdateCanvasUI.Invoke();
        }
    }

    public event Action<int> PlayerGainedXP;
    public void InvokePlayerGainedXP(int xp)
    {
        if (PlayerGainedXP != null)
        {
            PlayerGainedXP.Invoke(xp);
            UpdateCanvasUI.Invoke();
        }
    }

    // Player leveled up
    public event Action<int> PlayerLevelUp;
    public void InvokePlayerLevelUp(int reachedLevel)
    {
        if (PlayerLevelUp != null)
        {
            PlayerLevelUp.Invoke(reachedLevel);
        }
    }

    // Enemy gazet at
    public event Action<IEnemy> EnemyGazedEnter;
    public void InvokeEnemyGazedEnter(IEnemy enemy)
    {
        if (EnemyGazedEnter != null) EnemyGazedEnter.Invoke(enemy);
    }

    // Enemy not gazed at anymore
    public event Action<IEnemy> EnemyGazedExit;
    public void InvokeEnemyGazedExit(IEnemy enemy)
    {
        if (EnemyGazedExit != null) EnemyGazedExit.Invoke(enemy);
    }

    // Player got hit
    public event Action<IEnemy> EnemyHit;
    public void InvokeEnemyHit(IEnemy enemy)
    {
        if (EnemyHit != null) EnemyHit.Invoke(enemy);
    }

    // Enemy Killed Event
    public event Action<IEnemy> EnemyKilled;
    public void InvokeEnemyKilled(IEnemy killedEnemy)
    {
        if (EnemyKilled != null) {
            EnemyKilled.Invoke(killedEnemy);
            UpdateWorldUI.Invoke();                
        }
    }

    // Item Collected Event
    public event Action<IITem> ItemCollected;
    public void InvokeItemCollected(IITem collectedItem)
    {
        if (ItemCollected != null) {
            ItemCollected.Invoke(collectedItem);
            UpdateWorldUI.Invoke();
        }
    }

    // Player took a quest
    public event Action<IQuest> QuestTaken;
    public void InvokeQuestTaken(IQuest takenQuest)
    {
        if (QuestTaken != null)
        {
            QuestTaken.Invoke(takenQuest);
        }
    }

    // Player completed a quest 
    public event Action<IQuest> QuestCompleted;
    public void InvokeQuestCompleted(IQuest completedQuest)
    {
        if (QuestCompleted != null) {
            QuestCompleted.Invoke(completedQuest);
            UpdateWorldUI.Invoke();
        }
    }

    // Player discovered a new region
    public event Action<IRegion, bool> RegionEntered;
    public void InvokeRegionEntered(IRegion region, bool forFirstTime)
    {
        if (RegionEntered != null)
        {
            RegionEntered(region, forFirstTime);
            UpdateCanvasUI.Invoke();
        }
    }

    // Update canvas UI (player's HP, SP...)
    public event Action UpdateCanvasUI;
    public void InvokeUpdateCanvasUI()
    {
        if (UpdateCanvasUI != null) UpdateCanvasUI.Invoke();
    }

    // Update world UI (Quest, Inventory, spells...)
    public event Action UpdateWorldUI;
    public void InvokeUpdateWorldUI()
    {
        if (UpdateWorldUI != null) UpdateWorldUI.Invoke();
    }
    #endregion




}
