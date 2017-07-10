using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppStartPosition : MonoBehaviour {
    [SerializeField] Transform paulaStartTransform;
    [SerializeField] Transform noTourPanel;

    [Header ("Fade Out Animation")]
    [SerializeField] float fadeOutSpeed = 1;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] Image[] images;

    void Awake()
    {
        AvatarManager.Instance.OnTourStarted += HideStartScreen;
        GamePersistenceManager.Instance.OnNoGameStored += OnNoTourStored;
    }

    void Update()
    {
        List<IAnchor> anchors = AnchorManager.Instance.AnchorList;
        if (anchors.Count == 1)
        {
            transform.position = anchors[0].AvatarTargetPosition + Vector3.down * 0.25f;
        }
        else if (anchors.Count > 1)
        {
            Vector3 dir = (anchors[1].AvatarTargetPosition - anchors[0].AvatarTargetPosition);
            dir.y = 0; // Should always be aligned
            dir = dir.normalized;
            transform.position = anchors[0].AvatarTargetPosition;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

	private void HideStartScreen()
	{
        StartCoroutine (FadeOutCo());
	}

    IEnumerator FadeOutCo ()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= 0.1f * Time.deltaTime * fadeOutSpeed;
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            } 
            foreach (Image image in images)
            {
                image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
            }
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive (false);
    }

    private void OnNoTourStored()
    {
        noTourPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
