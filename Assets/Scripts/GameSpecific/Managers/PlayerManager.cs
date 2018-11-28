using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {

    public int HP { get { return playerLife; } }
    public float RelativeHP { get { return (playerLife * 1.0f) / initialPlayerLife; } }
    public bool IsDead { get { return  HP <= 0; } }

    public int Level { get { return playerLevel; } }
    public int XP { get { return playerExperience; } }
    public float RelativeXP { get { return (playerExperience * 1.0f) / GetLevelMaxXP(playerLevel); } }

    public float RelativePower { get { return SpellsManager.Instance.RelativePower; } }

    [SerializeField] private int initialPlayerLife = 200;
    private int playerLife;
    private int playerLevel;
    private int playerExperience;
    private int powerPercentage;

    protected override void Awake()
    {
        base.Awake();
        playerLife = initialPlayerLife;
        playerLevel = 1; 
        playerExperience = 0;
    }

    void Start()
    {
        GameManger.Instance.PlayerGotHit += ReducePlayerLife;
        GameManger.Instance.EnemyKilled += OnEnemyKilled;
        GameManger.Instance.QuestCompleted += OnQuestCompleted;
        GameManger.Instance.RegionEntered += OnRegionEntered;
    }

    private void OnRegionEntered(IRegion region, bool firstTime)
    {
        if (firstTime == true)
        {
            IncreaseXP(50);
        }
    }

    private void OnEnemyKilled(IEnemy enemy)
    {
        IncreaseXP(enemy.XPReward);
    }

    private void OnQuestCompleted(IQuest quest)
    {
        IncreaseXP(quest.XPReward);
    }

    private void ReducePlayerLife(int byAmount)
    {
        playerLife -= byAmount;
        
        if (playerLife < 0)
        {
            playerLife = 0;
         //   Notify.Show("You have died!", 7);
        }
        GameManger.Instance.InvokeUpdateCanvasUI();   
    }

    private void IncreaseXP(int byAmout)
    {
        playerExperience += byAmout;
        GameManger.Instance.InvokePlayerGainedXP(byAmout);
        if (playerExperience >= GetLevelMaxXP(playerLevel))
        {
            LevelUP();
        }
        // recheck if player's xp can reach another level
        if (playerExperience >= GetLevelMaxXP(playerLevel))
        {
            IncreaseXP(0);
        }
    }

    private void LevelUP()
    {
        playerLevel++;
        // Power up player's health
        initialPlayerLife = (int) (initialPlayerLife * 1.2f);
        // Restore full player's health
        playerLife = initialPlayerLife;

        Notify.Show("You are now level " + playerLevel + " !", 4);

        GameManger.Instance.InvokePlayerLevelUp(playerLevel);
        GameManger.Instance.InvokeUpdateCanvasUI();
    }


    // Hard coded required xp for each level
    private int GetLevelMaxXP(int level)
    {
        int xp = 0;
        switch (level)
        {
            case 1: // 100
                xp = 100;
                break;

            case 2: //150
                xp = 250;
                break;

            case 3: // 250
                xp = 500;
                break;

            case 4: // 350
                xp = 850;
                break;

            case 5: // 500
                xp = 1350;
                break;

            case 6: // 750
                xp = 2100;
                break;

            case 7: // 1000
                xp = 3100;
                break;

            case 8: // 1250
                xp = 4350;
                break;

        }
        return xp;
    }

    



}
