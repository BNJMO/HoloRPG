using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : HoloToolkit.Unity.Singleton<UIManager>, IKeywordCommandProvider {

    [SerializeField] private Panel mainUIPanel;
	[SerializeField] private LifeBar playerHPBar;
    [SerializeField] private LifeBar playerXPBar;
    [SerializeField] private LifeBar playerPowerBar;
    [SerializeField] private LifeBar enemyHPBar;
    [SerializeField] private Text playerLevelText;
    [SerializeField] private Text enemyNameText;
    [SerializeField] private Text enemyLevelText;
    [SerializeField] private Text regionText;


    private IEnemy currentEnemyGazedAt;

    public bool EnemyLifeBarIsVisible { get { return enemyHPBar.IsVisible; } }
    private bool hidingEnemyLifeBar = false;

    enum State
    {
        NONE,
        QUEST,
        SPELLS,
        INVENTORY
    }
    State state = State.NONE;


    void Start()
    {
        
        hidingEnemyLifeBar = true;
        HideEnemyLifeBar();

        OnUpdateCanvasUI();

        GameManger.Instance.UpdateCanvasUI += OnUpdateCanvasUI;
        GameManger.Instance.UpdateWorldUI += OnUpdateWorldUI;
        GameManger.Instance.EnemyGazedEnter += OnEnemyGazedAt;
        GameManger.Instance.EnemyGazedExit += OnEnemyGazedExit;
        GameManger.Instance.EnemyHit += OnEnemyHit;

        KeywordCommandManager.Instance.AddKeywordCommandProvider(this);
    }

    void Update()
    {
        SetPlayerPower(PlayerManager.Instance.RelativePower);
    }

    private void OnEnemyGazedExit(IEnemy obj)
    {
        HideEnemyLifeBarAfterDelay();
    }

    private void OnEnemyGazedAt(IEnemy enemy)
    {
        currentEnemyGazedAt = enemy;
        ShowEnemyLifeBar(enemy.EnemyName, enemy.Level, enemy.RelativeHP);
    }

    private void OnEnemyHit(IEnemy enemy)
    {
        if (enemy == currentEnemyGazedAt)
        {
            SetEnemyLife(enemy.RelativeHP);
        }
    }


    #region CANVAS UI

    private void OnUpdateCanvasUI()
    {
        SetPlayerHP(PlayerManager.Instance.RelativeHP);
        SetPlayerXP(PlayerManager.Instance.RelativeXP);
        // Player Power updated in Update method
        playerLevelText.text = "LEVEL  " + PlayerManager.Instance.Level;
        regionText.text = RegionManager.Instance.CurrentRegionName;
    }

    // ENEMY BAR
    private void ShowEnemyLifeBar(string enemyName, int enemyLevel, float lifePercentage)
    {
        hidingEnemyLifeBar = false; 

        enemyHPBar.IsVisible = true;
        enemyNameText.text = enemyName;
        enemyLevelText.text = "LEVEL  " + enemyLevel;
        SetEnemyLife(lifePercentage);  
    }

    private void HideEnemyLifeBarImmediate()
    {
        hidingEnemyLifeBar = true;
        HideEnemyLifeBar();
    }

    private void HideEnemyLifeBarAfterDelay()
    {
        hidingEnemyLifeBar = true;
        Invoke("HideEnemyLifeBar", 2);
    }

    private void HideEnemyLifeBar()
    {
        if (hidingEnemyLifeBar == true)
        {
            enemyHPBar.IsVisible = false;
            enemyNameText.text = ""; 
            enemyLevelText.text = "";
            hidingEnemyLifeBar = false;
        }
    }

    private void SetEnemyLife(float percentage)
    {
        enemyHPBar.SetPercentage(percentage);
    }


    // PLAYER HP BAR
    private void SetPlayerHP(float percentage)
    {
        playerHPBar.SetPercentage(percentage);
    }


    // PLAYER XP BAR
    private void SetPlayerXP(float percentage)
    {
        playerXPBar.SetPercentage(percentage);
    }

    // PLAYER POWER BAR
    private void SetPlayerPower(float percentage)
    {
        playerPowerBar.SetPercentage(percentage);
    }


    #endregion

    #region WORLD UI

    /// <summary>
    /// Update UI when a change occurs depening on the state where the UI is currently.
    /// </summary>
    private void OnUpdateWorldUI()
    {
        switch(state)
        {
            case State.QUEST:
                ShowQuests();
                break;

            case State.SPELLS:
                ShowSpells();
                break;

            case State.INVENTORY:
                ShowInventory();
                break;
        }
    }


    public List<KeywordCommand> GetSpeechCommands()
    {
        List<KeywordCommand> result = new List<KeywordCommand>();
        Condition condIsUserMode    = Condition.New(() => GameManger.Instance.IsInUserMode == true);


        result.Add(new KeywordCommand(() => { ShowQuests(); },      condIsUserMode, "Show Quests", KeyCode.Q));
        result.Add(new KeywordCommand(() => { ShowSpells(); },      condIsUserMode, "Show Spells", KeyCode.Y));
        result.Add(new KeywordCommand(() => { ShowInventory(); },   condIsUserMode, "Show Inventory", KeyCode.X));

        return result;
    }

    private void ShowPanel()
    {
        // TODO
    }

    private void HidePanel()
    {
        // TODO
    }

    private void ShowQuests()
    {
        state = State.QUEST;
        mainUIPanel.Write("Quests", QuestManager.Instance.GetAcceptedQuestsProgression());
    }

    private void ShowSpells()
    {
        state = State.SPELLS;
        List<string> spells = new List<string>();
        spells.Add ("Fire");
        spells.Add ("Ice");
        mainUIPanel.Write("Spells", spells);
    }

    private void ShowInventory()
    {
        state = State.INVENTORY;
        mainUIPanel.Write("Inventory", InventoryManager.Instance.GetCollectedItemsDescription());
    }
    #endregion
}
