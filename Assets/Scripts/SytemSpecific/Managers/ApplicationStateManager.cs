using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Globally accessible Singleton class that allows to interact with the Application's state.
/// Copyright (c) BNJMO 2017
/// </summary>
public class ApplicationStateManager : HoloToolkit.Unity.Singleton<ApplicationStateManager>, IKeywordCommandProvider
{
    public enum EAppState { USER_MODE = 0, EDIT_MODE = 1 }

    public Action OnStateWillChage;

	void Start() {
		KeywordCommandManager.Instance.AddKeywordCommandProvider (this);
		InitScene ();
	}
    
    public static bool IsUserMode
    { get { return SceneManager.GetActiveScene().buildIndex == (int)EAppState.USER_MODE; } }

    public static bool IsEditMode
    { get { return SceneManager.GetActiveScene().buildIndex == (int)EAppState.EDIT_MODE; } }

    private void InitScene()
    {
        CameraHelper.Instance.FadeIn(() => { });
    }

    public void ReloadCurrentScene()
    {
        CameraHelper.Instance.FadeOut(() => LoadScene(SceneManager.GetActiveScene().buildIndex, true));
    }

    public void LoadUserModeScene()
    {
        if (IsUserMode == true)
        {
            Notify.Show ("You are already in User Mode!");
        }
        else
        {
            CameraHelper.Instance.FadeOut(() => LoadScene((int)EAppState.USER_MODE, false));
        }
        
    }

    public void LoadEditModeScene()
    {
        if (IsEditMode == true)
        {
            Notify.Show ("You are already in Edit Mode!");
        }
        else
        {
            CameraHelper.Instance.FadeOut(() => LoadScene((int)EAppState.EDIT_MODE, false));
        }
    }

    private void LoadScene(int sceneIndex, bool overwrite)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (overwrite || currentSceneIndex != sceneIndex)
        {
            if (OnStateWillChage != null)
            {
                OnStateWillChage.Invoke();
            }
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public List<KeywordCommand> GetSpeechCommands() {
		List<KeywordCommand> result = new List<KeywordCommand> ();

        Condition condUserMode = Condition.New(() => ApplicationStateManager.IsUserMode);
        
        result.Add(new KeywordCommand(() => { LoadEditModeScene(); }, Condition.TRUE,     "Set Edit Mode", KeyCode.Alpha1));
        result.Add(new KeywordCommand(() => { LoadUserModeScene(); }, Condition.TRUE,     "Set User Mode", KeyCode.Alpha2));

        result.Add(new KeywordCommand(() => {ReloadCurrentScene(); }, condUserMode,                  "Reload Experience", KeyCode.Alpha5));

 		return result;
	}
}

