using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


public enum SpellType
{
    FIRE,
    ICE,
    ROCK,
    DARK,
    LIGHT
}

   // TODO Bind with UI

public class SpellsManager : Singleton<SpellsManager>, IKeywordCommandProvider
{

    public float RelativePower { get { return GetPowerRechargeProgression(); } }

    private List<ISpell> fire_Spells;
    private List<ISpell> ice_Spells;
    private List<ISpell> rock_Spells;
    private List<ISpell> dark_Spells;
    private List<ISpell> light_Spells; 

    private ISpell fire_equipedSpell;
    private ISpell ice_equipedSpell;
    private ISpell rock_equipedSpell;
    private ISpell dark_equipedSpell;
    private ISpell light_equipedSpell;

    private ISpell currentSpell;

    private Vector3 handPosition;


    private GameObject fire_HandObject;
    private GameObject ice_HandObject;
    private GameObject rock_HandObject;
    private GameObject dark_HandObject;
    private GameObject light_HandObject;

    private GameObject currentHandObject;



    private float timeSinceLastFire;
    private float timeSinceLastPress;


    private float fireRate;

    SpellType spellType;

    protected override void Awake()
    {
        base.Awake();

        CreateHandObjects();
        DisableAllHandObjects();

        fire_Spells = new List<ISpell>();
        ice_Spells = new List<ISpell>();
        rock_Spells = new List<ISpell>();
        dark_Spells = new List<ISpell>();
        light_Spells = new List<ISpell>();
    }

    void Start()
    {
        // add 2 basic spells
        fire_Spells.Add(Resources.Load<GameObject>("S_FireBall").GetComponent<ISpell>());
        fire_equipedSpell = fire_Spells[0];

       /* rock_Spells.Add(Resources.Load<GameObject>("S_RockZone").GetComponent<ISpell>());
        rock_equipedSpell = rock_Spells[0];*/

        /*dark_Spells.Add(Resources.Load<GameObject>("S_DarkBall").GetComponent<ISpell>());
        dark_equipedSpell = dark_Spells[0];*/
        currentSpell = fire_equipedSpell;


        // Gesture
        InteractionManager.InteractionSourcePressed += InteractionManager_SourcePressed;
        InteractionManager.InteractionSourceReleased += InteractionManager_SourceReleased;
        InteractionManager.InteractionSourceDetected += InteractionManager_SourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_SourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_SourceLost;

        GameManger.Instance.ItemCollected += OnItemCollected;
        
        
        // Voice Command
        KeywordCommandManager.Instance.AddKeywordCommandProvider(this);
    }

    private void OnItemCollected(IITem item)
    {
        if (item is SpellItem)
        {
            ISpell newSpell = (item as SpellItem).SpellToLearn;
            AddSpell(newSpell);
        }
    }

    void Update()
    {
        currentHandObject.transform.position = handPosition;

        // Debug
        if (Input.GetKeyDown("f"))
            Fire();
    }

    // Learn a new Spell and equip it ( TODO : remove aumatic equip when Spell UI is implemented)
    private void AddSpell(ISpell newSpell)
    {
        switch(newSpell.Type)
        {
            case SpellType.FIRE:
                fire_Spells.Add(newSpell);
                fire_equipedSpell = newSpell;
                break;

            case SpellType.ICE:
                ice_Spells.Add(newSpell);
                ice_equipedSpell = newSpell;
                break;

            case SpellType.ROCK:
                rock_Spells.Add(newSpell);
                rock_equipedSpell = newSpell;
                break;

            case SpellType.DARK:
                dark_Spells.Add(newSpell);
                dark_equipedSpell = newSpell;
                break;

            case SpellType.LIGHT:
                light_Spells.Add(newSpell);
                light_equipedSpell = newSpell;
                break;
        }
    }
    

    private void InteractionManager_SourcePressed(InteractionSourcePressedEventArgs eventInfo)
    {
        timeSinceLastPress = Time.time;
    }

    private void InteractionManager_SourceReleased(InteractionSourceReleasedEventArgs eventInfo)
    {
        Fire();
    }

    private void InteractionManager_SourceDetected(InteractionSourceDetectedEventArgs eventInfo)
    {
        eventInfo.state.sourcePose.TryGetPosition(out handPosition);
        ShowHand();
    }

    private void InteractionManager_SourceUpdated(InteractionSourceUpdatedEventArgs eventInfo)
    {
        eventInfo.state.sourcePose.TryGetPosition(out handPosition);
    }

    private void InteractionManager_SourceLost(InteractionSourceLostEventArgs eventInfo)
    {
        HideHand();
    }

    private void ShowHand()
    {
        currentHandObject.SetActive(true);
    }

    private void HideHand()
    {
        currentHandObject.SetActive(false);
    }

    private void Fire()
    {
        if ((Time.time - timeSinceLastFire) >= currentSpell.FireRate)
        {
            MakeSpell.InstantiateObj(currentSpell.SpellID, currentHandObject.transform.position);
            timeSinceLastFire = Time.time;
        }
    }

    private float GetPowerRechargeProgression()
    {
        float elapsedTime = Time.time - timeSinceLastFire;
        if (elapsedTime >= currentSpell.FireRate)
        {
            return 1.0f;
        }
        else // still recharging
        {
            return elapsedTime / currentSpell.FireRate;
        }
    }

    public List<KeywordCommand> GetSpeechCommands()
    {
        List<KeywordCommand> result = new List<KeywordCommand>();
        Condition condIsUserMode = Condition.New(() => ApplicationStateManager.IsUserMode == true);
        Condition condFireNotEmpty = Condition.New(() => fire_Spells.Count != 0);
        Condition condIceNotEmpty = Condition.New(() => ice_Spells.Count != 0);
        Condition condRockNotEmpty = Condition.New(() => rock_Spells.Count != 0);
        Condition condDarkNotEmpty = Condition.New(() => dark_Spells.Count != 0);
        Condition condLightNotEmpty = Condition.New(() => light_Spells.Count != 0);

        result.Add(new KeywordCommand(() => { SetFire(); }, condIsUserMode.And(condFireNotEmpty), "Fire", KeyCode.G));
        result.Add(new KeywordCommand(() => { SetIce(); }, condIsUserMode.And(condIceNotEmpty), "Ice"));
        result.Add(new KeywordCommand(() => { SetRock(); }, condIsUserMode.And(condRockNotEmpty), "Rock"));
        result.Add(new KeywordCommand(() => { SetDark(); }, condIsUserMode.And(condDarkNotEmpty), "Dark", KeyCode.H));
        result.Add(new KeywordCommand(() => { SetLight(); }, condIsUserMode.And(condLightNotEmpty), "Light"));

        return result;
    }

    private void SetFire()
    {
        spellType = SpellType.FIRE;
        currentSpell = fire_equipedSpell;
        DisableAllHandObjects();
        currentHandObject = fire_HandObject;
        timeSinceLastFire = 0;

        GameManger.Instance.InvokeSpellEquiped(currentSpell);

        
    }

    private void SetIce()
    {
        spellType = SpellType.ICE;
        currentSpell = ice_equipedSpell;
        DisableAllHandObjects();
        currentHandObject = ice_HandObject;
        timeSinceLastFire = 0;
        Notify.Show("You have equiped ICE spell", 3);
    }

    private void SetRock()
    {
        spellType = SpellType.ROCK;
        currentSpell = rock_equipedSpell;
        DisableAllHandObjects();
        currentHandObject = rock_HandObject;
        timeSinceLastFire = 0;
        Notify.Show("You have equiped ROCK spell", 3);
    }

    private void SetDark()
    {
        spellType = SpellType.DARK;
        currentSpell = dark_equipedSpell;
        DisableAllHandObjects();
        currentHandObject = dark_HandObject;
        timeSinceLastFire = 0;
        Notify.Show("You have equiped DARK spell", 3);
    }

    private void SetLight()
    {
        spellType = SpellType.LIGHT;
        currentSpell = light_equipedSpell;
        DisableAllHandObjects();
        currentHandObject = light_HandObject;
        timeSinceLastFire = 0;
        Notify.Show("You have equiped LIGHT spell", 3);
    }


    private void DisableAllHandObjects()
    {
        fire_HandObject.SetActive(false);
        ice_HandObject.SetActive(false);
        rock_HandObject.SetActive(false);
        light_HandObject.SetActive(false);
        dark_HandObject.SetActive(false);
    }

    private void CreateHandObjects()
    {
        fire_HandObject = Instantiate(Resources.Load<GameObject>("Fire_HandObject"), transform);
        fire_HandObject.transform.parent = transform;

        ice_HandObject = Instantiate(Resources.Load<GameObject>("Ice_HandObject"), transform);
        ice_HandObject.transform.parent = transform;

        rock_HandObject = Instantiate(Resources.Load<GameObject>("Rock_HandObject"), transform);
        rock_HandObject.transform.parent = transform;

        dark_HandObject = Instantiate(Resources.Load<GameObject>("Dark_HandObject"), transform);
        dark_HandObject.transform.parent = transform;

        light_HandObject = Instantiate(Resources.Load<GameObject>("Light_HandObject"), transform);
        light_HandObject.transform.parent = transform;


        currentHandObject = fire_HandObject;
        currentHandObject.SetActive(false);
    }
}
