using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpText : MonoBehaviour {

    private Text myText;

    [SerializeField] private float textUpSpeed = 10;
    [SerializeField] private float textUpSmoothness = 100;
    [SerializeField] private float textFadeSpeed = 5;


    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    private void Update()
    {
        Vector2 actualPos = myText.rectTransform.localPosition;
        Vector2 newPos = new Vector2(actualPos.x, actualPos.y + 0.1f * textUpSpeed);

        Color actualCol = myText.color;
        actualCol = new Color(actualCol.r, actualCol.g, actualCol.b, actualCol.a - 0.1f * textFadeSpeed * Time.deltaTime);

        myText.rectTransform.localPosition = Vector2.Lerp(actualPos, newPos, textUpSmoothness * Time.deltaTime);
        myText.color = actualCol;

        if (actualCol.a <= 0)
        {
            Destroy(gameObject);
        }
        

    }


}
