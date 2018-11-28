using UnityEngine;
using System;


using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GamePersistenceManager : Singleton<GamePersistenceManager>, IKeywordCommandProvider
{
	private const string filename = "game_savedstate";

	private AnchorManager anchorManager;
	private UnityEngine.XR.WSA.Persistence.WorldAnchorStore worldAnchorStore;

	private bool pendingSaveAttempt;
	private bool pendingLoadAttempt;

	private bool isEditorMode;

    public event Action OnNoGameStored;

    private new void Awake()
    {
        base.Awake();
    }

    
    private void Start()
    {
		KeywordCommandManager.Instance.AddKeywordCommandProvider (this);
		UnityEngine.XR.WSA.Persistence.WorldAnchorStore.GetAsync(OnStoreReady);
		anchorManager = AnchorManager.Instance;
#if UNITY_EDITOR
        isEditorMode = true;
#endif
        ApplicationStateManager.Instance.OnStateWillChage += OnStateWillChage;
        
        // Check if a game has already been loaded
        StartCoroutine (CheckForSavedGameWithDely());
    }

    System.Collections.IEnumerator CheckForSavedGameWithDely()
    {
        yield return new WaitForEndOfFrame();
        if ((ApplicationStateManager.IsUserMode) && (SavedGameAvailable() == false))
        {
            // Redirect to Edit Mode if no game has been created yet.
            if (OnNoGameStored != null) OnNoGameStored.Invoke();
        }
        else if ((ApplicationStateManager.IsUserMode) && (Instance.SavedGameAvailable() == true))
        {
            LoadGameState();
            if (anchorManager.GetAnchorsCount() == 0)
            {
                if (OnNoGameStored != null) OnNoGameStored.Invoke();
            }
        }
        else
        {
            LoadGameState();
        }
    }

    private void OnStateWillChage()
    {
        if (ApplicationStateManager.IsEditMode)
        {
            GamePersistenceManager.Instance.SaveGameState();
        }
    }

    private void OnStoreReady(UnityEngine.XR.WSA.Persistence.WorldAnchorStore store) {
		this.worldAnchorStore = store;
		if (pendingSaveAttempt) {
			SaveGameState ();
		}
        if (pendingLoadAttempt) {
            LoadGameState();
        }
	}

	public void SaveGameState()
    {
		if (!isEditorMode && worldAnchorStore == null) {
			pendingSaveAttempt = true;
			return;
		}
		pendingSaveAttempt = false;
		List<IAnchor> anchors = anchorManager.AnchorList;
		AnchorSaveState state = new AnchorSaveState ();

        foreach (IAnchor a in anchors)
        {
            state.savedObjects.Add(new ObjectState(anchorManager, a));
        }


		if (state.SaveToJson(filename, true))
        {
			if (!isEditorMode) {
				worldAnchorStore.Clear ();
				foreach (IAnchor a in anchors)
				{
					worldAnchorStore.Save ("" + anchorManager.GetIndexOf (a), a.GameObject.GetComponent<UnityEngine.XR.WSA.WorldAnchor> ());
				}
			}
        }
        else
        {
			Debug.LogError("Saving failed, couldn't write file \"" + filename + "\".");
        }
			
		Notify.Show ("Game Saved");
    }

	public void LoadGameState() {
		if (!isEditorMode && worldAnchorStore == null) {
			pendingLoadAttempt = true;
			return;
		}
		pendingLoadAttempt = false;

		AnchorSaveState state;

		try {
			state = JSONObject<AnchorSaveState>.CreateFromJSON (filename);
		} catch(Exception) {
			Notify.Show ("Unable to load Game.");
			return;
		}

		AnchorManager.Instance.DeleteAllAnchors ();
		foreach (ObjectState anchorState in state.savedObjects) {
            IAnchor a;

          /*  if (!anchorState.isPoi) {
                a = anchorManager.AddWaypoint(Vector3.zero);
            } else
            {*/
                a = anchorManager.AddAnchor(anchorState.poiId, Vector3.zero);
           // }

			// attach worldAnchorStore anchor
			if (!isEditorMode) {
				DestroyImmediate (a.GameObject.GetComponent<UnityEngine.XR.WSA.WorldAnchor> ());
				worldAnchorStore.Load ("" + anchorState.anchorIndex, a.GameObject);
			} else {
				// Setting the position is only usefull in edit mode.
				// in HoloLens mode, the position is handled by the world anchors.
				a.AnchorPosition = new Vector3(anchorState.x, anchorState.y, anchorState.z);
			}
		}
  /*      if (AnchorEditorUi.Instance != null)
        {
            AnchorEditorUi.Instance.NotifyModelChanged();
        }*/
        Notify.Show ("Game Loaded.");
	}

    public bool SavedGameAvailable()
    {
        return JSONObject<AnchorSaveState>.FileExists(filename);
    }

	public List<KeywordCommand> GetSpeechCommands() {
        Condition condEditMode = Condition.New(() => ApplicationStateManager.IsEditMode);
     //   Condition condNotRecording = Condition.New(() => WaypointTracingManager.Instance != null && !WaypointTracingManager.Instance.IsRecording);

        return new List<KeywordCommand> {
            new KeywordCommand(() => { SaveGameState(); }, condEditMode/*.And(condNotRecording)*/, "Save Game", KeyCode.K),
            new KeywordCommand(() => { LoadGameState(); }, condEditMode/*.And(condNotRecording)*/, "Load Game", KeyCode.L)
        };
    }

}
 
/// <summary>
/// Data Representation of a Save State. Allows for different save states or "versions".
/// </summary>
public class AnchorSaveState : JSONObject<AnchorSaveState>
{
    public List<ObjectState> savedObjects = new List<ObjectState>();
}

/// <summary>
/// Container Class for formatting and funneling of data to be serialized into json. Edit or Inherit from this to customize the information to be saved.
/// </summary>
[Serializable]
public class ObjectState
{
    public int anchorIndex;
    public bool isPoi;
    public bool isEntertainmentPoi;
    public string poiId;
	public float x, y, z;

    public ObjectState() {}
	public ObjectState(AnchorManager anchorManager, IAnchor a)
    {
        anchorIndex = anchorManager.GetIndexOf (a);
        poiId = a is PoiAnchor ? (a as PoiAnchor).PoiId : "";
        isPoi = a is PoiAnchor;
        isEntertainmentPoi = a is EntertainmentAnchor;
		Vector3 pos = a.AnchorPosition;
		x = pos.x;
		y = pos.y;
		z = pos.z;
    }
}