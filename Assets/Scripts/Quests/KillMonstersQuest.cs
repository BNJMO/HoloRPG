using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMonstersQuest : AbstractQuest
{
    [Header("Kill Monsters")]
    [SerializeField] private AbstractEnemy enemyToKill;
    [SerializeField] private int numberToKill = 5;

    public int KilledMonsters { get; set; }

    void Start()
    {
       GameManger.Instance.EnemyKilled += OnEnemyKilled;
    }

    protected override int GetQuestProgression()
    {
        return (int)(((KilledMonsters * 1.0f) / numberToKill) * 100);
    }

    protected override bool QuestAvailable()
    {
        return true; // TODO
    }

    protected override bool QuestCompleted()
    {
        return KilledMonsters >= numberToKill;
    }

    private void OnEnemyKilled(IEnemy enemy)
    {
        if (enemy.EnemyName == enemyToKill.EnemyName)
        {
            KilledMonsters++;
         /*   if (QuestCompleted() == true)
            {
                QuestManager.Instance.OnQuestCompleted(this);
            }*/
        }
    }

}
