using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : HoloToolkit.Unity.Singleton<UIManager> {

	[SerializeField] private LifeBar playerLifeBar;
    [SerializeField] private LifeBar enemyLifeBar;
    [SerializeField] private Text enemyNameText;

    public bool EnemyLifeBarIsVisible { get { return enemyLifeBar.IsVisible; } }
    private bool hidingEnemyLifeBar = false;

    // ENEMY BAR
    public void ShowEnemyLifeBar(string enemyName, float percentage)
    {
        hidingEnemyLifeBar = false; 

        enemyLifeBar.IsVisible = true;
        enemyNameText.text = enemyName;

        SetEnemyLife(percentage);  
    }

    public void HideEnemyLifeBarImmediate()
    {
        hidingEnemyLifeBar = true;
        HideEnemyLifeBar();
    }

    public void HideEnemyLifeBarAfterDelay()
    {
        hidingEnemyLifeBar = true;
        Invoke("HideEnemyLifeBar", 2);
    }

    private void HideEnemyLifeBar()
    {
        if (hidingEnemyLifeBar == true)
        {
            enemyLifeBar.IsVisible = false;
            enemyNameText.text = ""; 
            hidingEnemyLifeBar = false;
        }
    }

    public void SetEnemyLife(float percentage)
    {
        enemyLifeBar.SetPercentage(percentage);
    }

    public void SetEnemyLife(int percentage)
    {
        enemyLifeBar.SetPercentage(percentage);
    }



    // PLAYER BAR
    public void SetPlayerLife(float percentage)
    {
        playerLifeBar.SetPercentage(percentage);
    }

    public void SetPlayerLife(int percentage)
    {
        playerLifeBar.SetPercentage(percentage);
    }

}
