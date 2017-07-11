using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.VR.WSA;

public abstract class AbstractItem : MonoBehaviour, IITem {

    public string ItemID { get { return itemID; } }
    public string ItemName { get { return itemName; } }
    public Sprite Icon { get { return icon; } }
    public int Level { get { return level; } }

    private float visibilityRange = 15;
    public bool IsVisible { get { return myVisibiliter.IsVisible; } set { myVisibiliter.IsVisible = value; } }
    
    public bool PlayerInVisibilityRange { get { return Utils.GetRelativeDistance(transform.position, CameraHelper.Stats.camPos) < visibilityRange; } }

    public bool PlayerInRage { get { return  Utils.GetRelativeDistance(transform.position, CameraHelper.Stats.camPos) < collectRange; } }

    private VisibiliterMesh myVisibiliter;

    [Header ("Item parameters")]

    [SerializeField] private string itemID = "I_name";
    [SerializeField] private string itemName = "Holy Item";
    [SerializeField] private Sprite icon;
    [SerializeField] private int level = 1;
    [SerializeField] private float collectRange = 1.5f;


    private bool hasBeenCollected = false;

    void Awake()
    {
        // need to add WorldAnchor ?
      //  gameObject.AddComponent<WorldAnchor>();

        myVisibiliter = gameObject.AddComponent<VisibiliterMesh>();
        myVisibiliter.VisibilityCondition = Condition.New(() => PlayerInVisibilityRange == true);
    }
  


    void Update()
    {
        // visible if in range


        // collect if player in range
        if ((PlayerInRage == true) && (hasBeenCollected == false))
        {
            Collect();
        }
    }

    private void Collect()
    {
        hasBeenCollected = true;
        GameManger.Instance.InvokeItemCollected(this);
        // make sure some frames pass before destroying gameObject 
        Invoke("Disappear", 0.5f);
    }

    private void Disappear()
    {
        Notify.Beep();
        Destroy(gameObject);
    }

}
