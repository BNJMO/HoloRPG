using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : HoloToolkit.Unity.Singleton<GameManger> {

    [SerializeField] private int initialPlayerLife = 200;
	public int PlayerLife { get { return playerLife; } }
    private int playerLife;

    void Start()
    {
        playerLife = initialPlayerLife;
        UIManager.Instance.SetPlayerLife(100);
    }


    public void ReducePlayerLife(int byAmount)
    {
        playerLife -= byAmount;
        if (PlayerLife <= 0)
        {
            Notify.Debug("You Are DEAD! (lotf 3lik)");
            UIManager.Instance.SetPlayerLife(0);
        }
        else
        {
            float percentage = (playerLife * 1.0f) / initialPlayerLife;
            UIManager.Instance.SetPlayerLife(percentage);
        }

    }
}
