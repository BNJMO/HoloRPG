using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItemQuest : AbstractQuest
{

    [Header("Collect Items")]
    [SerializeField] private AbstractItem itemToCollect;
    [SerializeField] private int numberToCollect = 3;

    public int CollectedItems { get; set; }

    void Start()
    {
        GameManger.Instance.ItemCollected += OnItemCollected;
    }

    protected override int GetQuestProgression()
    {
        return (int)(((CollectedItems * 1.0f) / numberToCollect) * 100);
    }

    protected override bool QuestAvailable()
    {
        return true;
    }

    protected override bool QuestCompleted()
    {
        return CollectedItems >= numberToCollect;
    }

    private void OnItemCollected(IITem collectedItem)
    {
        if (collectedItem.ItemName == itemToCollect.ItemName)
        {
            CollectedItems++;
          /*  if (QuestCompleted() == true)
            {
                QuestManager.Instance.OnQuestCompleted(this);
            }*/
        }
    }
}
