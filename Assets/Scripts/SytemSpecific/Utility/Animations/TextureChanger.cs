using UnityEngine;
using System.Collections;

/// <summary>
/// This class switchs between textures of a gameObject. It always changes the hidden texture and then smoothly fades the shown one.
/// If you don't wish to switch between textures, simply set second texture like the first one.
/// </summary>
public class TextureChanger : MonoBehaviour {
	public float animationSpeed = 1;

	public Texture texutre1;
	public Texture texture2;

	private int actualTexture = 1; // 1 : first, 2 : second
 	private float cutoffState = 0;

	bool textureInitialized = false;
	bool isPerforming = false;

	Material myMaterial;


	void Awake () 
	{
		myMaterial = GetComponent<Renderer> ().material;

		if (myMaterial == null) {
			Debug.LogError ("Fatal error! GameObject has no material!");
			textureInitialized = false;
		}

	}

	void Start () 
	{
		if ((texutre1 == null) || (texture2 == null)) {
			Debug.LogError ("Fatal error! Textures have to be initialized!");
			textureInitialized = false;
		} else {
			textureInitialized = true;
		}
	}


	void Update () 
	{
		if (Input.GetKeyDown ("o"))
			StartCoroutine (FadeOutCoroutine ());

		if (Input.GetKeyDown ("p"))
			StartCoroutine (FadeInCoroutine ());

		if (Input.GetKeyDown ("i"))
			SwitchBetweenTextures ();
	}


	public void SwitchBetweenTextures () 
	{

	}


	public void SwitchToTexture (Texture newTexture) 
	{
		if (!CheckConditions ()) return;

		// Note : actualTexture updated in FadeIn/Out methods
		if (actualTexture == 1) { // first
			myMaterial.SetTexture ("_MainTex2", newTexture);
			FadeIn ();

		} else { // second
			myMaterial.SetTexture ("_MainTex1", newTexture);
			FadeOut ();
		}

	}


	public void SwitchToTexture (Material textureFromMaterial) 
	{
		if (!CheckConditions ()) return;

		Texture newTexture = textureFromMaterial.mainTexture;
		SwitchToTexture (newTexture);
	}



	public void FadeIn () 
	{
		if (!CheckConditions ()) return;

		if (actualTexture == 1)
			actualTexture = 2;
		else
			actualTexture = 1;
		
		StartCoroutine (FadeInCoroutine ());
	}

	public void FadeOut () 
	{
		if (!CheckConditions ()) return;

		if (actualTexture == 1)
			actualTexture = 2;
		else
			actualTexture = 1;
		
		StartCoroutine (FadeOutCoroutine ());
	}

	IEnumerator FadeInCoroutine() 
	{
		isPerforming = true;
		while (cutoffState < 1) {
			cutoffState += 0.01f * animationSpeed;
			GetComponent<Renderer> ().material.SetFloat ("_Cutoff", cutoffState);
			yield return new WaitForSeconds (0.001f);
		}
		isPerforming = false;
	}

	IEnumerator FadeOutCoroutine() 
	{
		isPerforming = true;
		while (cutoffState > 0) {
			cutoffState -= 0.01f * animationSpeed;
			GetComponent<Renderer> ().material.SetFloat ("_Cutoff", cutoffState);
			yield return new WaitForSeconds (0.001f);
		}
		isPerforming = false;
	}


	/// <summary>
	/// Checks wether a texture switch is allowed.
	/// </summary>
	/// <returns><c>true</c>, if allowed, <c>false</c> otherwise.</returns>
	bool CheckConditions () 
	{
		if ((textureInitialized) && (!isPerforming))
			return true;
		else {
			if (!textureInitialized)
				Debug.LogError ("Texture Changer won't work when textures (or material) have not been initialized!");
			return false;
		}
	}
}
