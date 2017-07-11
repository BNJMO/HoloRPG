using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpTextCreator : HoloToolkit.Unity.Singleton<UpTextCreator> {


    [SerializeField] private Text damageTextRef;
    [SerializeField] private Text xpTextRef;


    private void Start()
    {
        GameManger.Instance.PlayerGotHit += OnPlayerGotHit;
        GameManger.Instance.PlayerGainedXP += OnPlayerGainedXP;
    }

    private void OnPlayerGainedXP(int xp)
    {
        NewXpText(xp.ToString() + " XP");
    }

    private void OnPlayerGotHit(int attackDamage)
    {
        NewDamageText(attackDamage.ToString());
    }

    public void NewDamageText(string damagePoints)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("DamageText"));
        Text txt = obj.GetComponent<Text>();
        txt.text = damagePoints;
        txt.rectTransform.parent = damageTextRef.transform.parent;
        txt.rectTransform.localPosition = damageTextRef.transform.localPosition;
        txt.rectTransform.localRotation = damageTextRef.rectTransform.localRotation;
        txt.rectTransform.localScale = damageTextRef.rectTransform.localScale;
    }


    public void NewXpText(string xpPoints)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("XpText"));
        Text txt = obj.GetComponent<Text>();
        txt.text = xpPoints;
        txt.rectTransform.parent = xpTextRef.transform.parent;
        txt.rectTransform.localPosition = xpTextRef.transform.localPosition;
        txt.rectTransform.localRotation = xpTextRef.rectTransform.localRotation;
        txt.rectTransform.localScale = xpTextRef.rectTransform.localScale;
    }

}
