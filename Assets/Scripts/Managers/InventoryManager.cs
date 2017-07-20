using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : HoloToolkit.Unity.Singleton<InventoryManager> {


    private AbstractItem[] allItems;

    private Dictionary<string, int> collectedItems;

  //  private event Action InventoryUpdated;

    protected override void Awake()
    {
        base.Awake();
        // load all items 
        allItems = Resources.LoadAll<AbstractItem>("Items\\Resources");
        // set for each item that the player has 0 from that item
        collectedItems = new Dictionary<string, int>();
        foreach(IITem item in allItems)
        {
            collectedItems[item.ItemID] = 0;
        }
        Debug.Log ("dic count : " + collectedItems.Count);
    }

    void Start()
    {
        GameManger.Instance.ItemCollected += OnItemCollected;
        GameManger.Instance.EnemyKilled += OnEnemyKilled; 
    }

    #region Item logistic

    public void DropItem(string itemID, Vector3 position)
    {
        Debug.Log ("Dropping " + itemID);
        Vector3 relativePosition = new Vector3
        (
            position.x,
            position.y +  CameraHelper.Stats.eyeHeight - 0.5f,
            position.z
        );
        GameObject item = Instantiate(Resources.Load<GameObject>(itemID), position, Quaternion.identity);
        item.transform.parent = transform;
    }

    public List<string> GetCollectedItemsDescription()
    {
        List<string> result = new List<string>();
        foreach(IITem item in allItems)
        {
            if (HasItem(item))
            {
                result.Add(item.ItemName + " (" + collectedItems[item.ItemID] + ")");
            }
        }
        if (result.Count == 0)
        {
            result.Add("Your inventory is empty.");
        }
        return result;
    }

    /// <summary>
    /// Add new item to inventory.
    /// </summary>
    private void AddItem(IITem newItem)
    {
        collectedItems[newItem.ItemID]++;
        GameManger.Instance.InvokeUpdateWorldUI();

        Notify.Show("<b>" + newItem.ItemName + "</b> collected", 3);
    }

    private bool HasItem(IITem item)
    {
        return collectedItems[item.ItemID] > 0;
    }

    private void RemoveItem(IITem item)
    {
        collectedItems[item.ItemID] = 0;
        GameManger.Instance.InvokeUpdateWorldUI();
    }

    

    
    #endregion

    #region events callback methods
    public void OnEnemyKilled(IEnemy killedEnemey)
    {
        Debug.Log ("InventoryManager : OnEnemyKilled ");
        IITem dropItem = GetRandomItem(killedEnemey.Level);
        if (dropItem == null) return;
        DropItem(dropItem.ItemID, killedEnemey.EnemyPosition);
    }

    public void OnItemCollected(IITem collectedItem)
    {
        AddItem(collectedItem);
    }
    #endregion

    // private methods
    private IITem GetRandomItem(int level)
    {
        Debug.Log ("allItems length : " + allItems.Length);
        if (allItems.Length == 0) return null;
        List<IITem> levelItems = new List<IITem>();
        foreach(IITem item in allItems)
        {
            if (item.Level == level)
            {
                levelItems.Add(item);
            }
        }
        Debug.Log("levelItems : " + levelItems.Count);
        if (levelItems.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, levelItems.Count);
        return levelItems[index];
    }

    private IITem GetRandomItem()
    {
        if (allItems.Length == 0) return null;
        int index = UnityEngine.Random.Range(0, allItems.Length);
        return allItems[index];
    }


}
