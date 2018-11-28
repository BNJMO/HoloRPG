using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

/// <summary>
/// Visual feedback and manager for placing anchors in edit mode.
/// Add prefabs of anchors in editor 
/// TODO: Simplify process
/// 
/// </summary>
public class AnchorEditorUi : HoloToolkit.Unity.Singleton<AnchorEditorUi>, IKeywordCommandProvider
{

    private enum Mode
    {
        ANCHOR_CREATING, ANCHOR_PLACING
    }

    [SerializeField]
    private Transform anchorParent;

    private Mode mode = Mode.ANCHOR_CREATING;

    private ApplicationStateManager appStateManager;
    private AnchorManager anchorManager;

   
    // Gizmos
    private IAnchorGizmo waypointCreatingGizmo;
    private IAnchorGizmo placingGizmo;
    private Dictionary<string, IAnchorGizmo> poiGizmos = new Dictionary<string, IAnchorGizmo>();
    private IAnchor grabbedAnchor;
    private Vector3 grabbedGizmoPositionOffset;
    private Quaternion grabbedGizmoRotationOffset;

    private LineRenderer lineRenderer;

    private int insertIndex = 0;
    private IAnchor lastPossibleInsertModeStartPoint;

    private bool renderSpatialMeshEnabled = false;
    private bool uiHidden;
    
    /// <summary>
    /// The Anchor that is currently being gazed at.
    /// </summary>
    public IAnchor GazedAnchor
    { get;  set; }

    // conditions for keywords command
    public bool IsGazedAnchorSelection {get; set; }
    public bool IsGazedAnchorMonster {get; set; }
    public bool IsGazedAnchorNPC {get; set; }

    
    void Start () {
        anchorManager = AnchorManager.Instance;
        appStateManager = ApplicationStateManager.Instance;

        lineRenderer = anchorParent.GetComponent<LineRenderer>();

        CreateAnchorGizmo();

        InteractionManager.SourcePressed += InteractionManager_SourcePressed;
        InteractionManager.SourceReleased += InteractionManager_SourceReleased;

        KeywordCommandManager.Instance.AddKeywordCommandProvider(this);
    }

    void Update()
    {
        UpdateUi();
    }


    void UpdateUi()
    {
        if (mode == Mode.ANCHOR_PLACING)
        {
            UpdateGrabbedAnchor();
        }
        else
        {
            UpdateGizmo();
        }
    }

    // from Interaction Manager
   private void InteractionManager_SourcePressed(InteractionSourceState state)
    {
        if ((state.source.kind == InteractionSourceKind.Controller) && (GazedAnchor != null))
        {
            Notify.Beep();
            GrabGazedAnchor();
        }
    }

    private void InteractionManager_SourceReleased(InteractionSourceState state)
    {
        if (state.source.kind == InteractionSourceKind.Controller)
        {
            Notify.Beep();
            ReleaseGazedAnchor();
        }
        
    }
    

    /// <summary>
    /// Grabs the selected anchor, if any
    /// </summary>
    private void GrabGazedAnchor()
    {
        if (GazedAnchor != null)
        {
            grabbedAnchor = GazedAnchor;
            mode = Mode.ANCHOR_PLACING;
            grabbedGizmoPositionOffset = Quaternion.Inverse(Camera.main.transform.rotation) * (placingGizmo.Position - Camera.main.transform.position);
            grabbedGizmoRotationOffset = Quaternion.Inverse(Camera.main.transform.rotation) * placingGizmo.Rotation;
            placingGizmo.Highlighted = true;
        }
    }

    /// <summary>
    /// Releases the selected anchor, if any
    /// </summary>
    private void ReleaseGazedAnchor()
    {
        placingGizmo.Highlighted = false;
        mode = Mode.ANCHOR_CREATING;
        grabbedAnchor = null;
    }

    /// <summary>
    /// Updates the grabbed object.
    /// </summary>
    private void UpdateGrabbedAnchor()
    {
        float movingSpeed = 7;
        Vector3 targetPosition =  Camera.main.transform.position + Camera.main.transform.rotation * grabbedGizmoPositionOffset;
        placingGizmo.Position = Vector3.Lerp (placingGizmo.Position, targetPosition, movingSpeed * Time.deltaTime);
        Quaternion rot = Camera.main.transform.rotation * grabbedGizmoRotationOffset;
        placingGizmo.Rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);
        grabbedAnchor.AnchorPosition = Vector3.Lerp (grabbedAnchor.AnchorPosition, targetPosition, movingSpeed * Time.deltaTime);
        grabbedAnchor.AnchorRotation = placingGizmo.Rotation;
    }


    /// <summary>
    /// Sets all anchors to hidden.
    /// </summary>
    public void HideAnchors()
    {
        uiHidden = true;
        AnchorManager.Instance.AnchorList.ForEach(a => a.IsVisible = false);
        UpdateUi();
    }

    /// <summary>
    /// Sets all abchors to visible.
    /// </summary>
    public void ShowAnchors()
    {
        uiHidden = false;
        AnchorManager.Instance.AnchorList.ForEach(a => a.IsVisible = true);
        UpdateUi();
    }


    /// <summary>
    /// Stops the insert mode and switches to the append mode.
    /// </summary>
    public void SetAppendMode()
    {
        List<IAnchor> anchorList = AnchorManager.Instance.AnchorList;
        insertIndex = anchorList.Count;
     //   UpdateAnchorPath();
    }

    /// <summary>
    /// Initializes the anchor gizmo.
    /// </summary>
    private void CreateAnchorGizmo()
    {
        waypointCreatingGizmo = Instantiate(Resources.Load("B_AnchorGizmo") as GameObject, anchorParent).GetComponentInChildren<IAnchorGizmo>();
        waypointCreatingGizmo.IsVisible = false;

        foreach (PointOfInterestManager.PointOfInterest poi in PointOfInterestManager.Instance.GetPOIs())
        {
            IAnchorGizmo gizmo = Instantiate(poi.GizmoPrefab, anchorParent).GetComponentInChildren<IAnchorGizmo>();
            poiGizmos.Add(poi.Id, gizmo);
            gizmo.IsVisible = false;
        }
    }

    /// <summary>
    /// Displays the Anchor Gizmo
    /// </summary>
	private void UpdateGizmo()
    {
        Vector3 pos;

        waypointCreatingGizmo.IsVisible = false;
        foreach(IAnchorGizmo poiGizmo in poiGizmos.Values)
        {
            poiGizmo.IsVisible = false;
        }

        if (uiHidden == true)
        {
            return;
        }

        if (GazedAnchor == null)
        {
            if (GetPositionForNewAnchor(out pos))
            {
                placingGizmo = waypointCreatingGizmo;
                placingGizmo.IsVisible = true;
                placingGizmo.Position = pos;
                Vector3 rotation = (pos - Camera.main.transform.position).normalized;
                rotation.y = 0;
                placingGizmo.Rotation = Quaternion.LookRotation(rotation);

                if (GetPossibleInsertModeStartPoint() != null)
                {
                    placingGizmo.Hint = "Say \"Insert\"\nto start insert mode.";
                }
                else
                {
                    placingGizmo.Hint = "Say \"New Waypoint\"\nto create a new waypoint\nhere.";
                }
            }
            else
            {
                placingGizmo = null;
            }

        }
        else if (GazedAnchor is PoiAnchor)
        {
            SetAnchorGizmo(poiGizmos[PointOfInterestManager.Instance.GetIdOfAnchor(GazedAnchor)], GazedAnchor);
        } 
    }

    private void SetAnchorGizmo(IAnchorGizmo productXGizmo, IAnchor selectedAnchor)
    {
        if (placingGizmo != productXGizmo)
        {
            placingGizmo = productXGizmo;
            placingGizmo.Position = selectedAnchor.AnchorPosition;
            placingGizmo.Rotation = selectedAnchor.AnchorRotation;
        }
        placingGizmo.IsVisible = true;
    }


    /// <summary>
    /// Creates a new anchor at the world cursor position and adds it to the anchor list
    /// </summary>
    private IAnchor CreateNewAnchorAtWorldCursorPosition(string productId)
    {
        IAnchor result = null;
        Vector3 pos;
        if (GetPositionForNewAnchor(out pos))
        {
            result = AnchorManager.Instance.AddAnchor(productId, pos);
            SetAppendMode();
            result.AnchorRotation = VectorUtils.LookAt2D(result.AnchorPosition, Camera.main.transform.position);
        }
        return result;
    }

    private IAnchor CreateNewAnchorAtGivenPosition(string productId, Vector3 position)
    {
        IAnchor result = AnchorManager.Instance.AddAnchor(productId, position);
        SetAppendMode();
        result.AnchorRotation = VectorUtils.LookAt2D(result.AnchorPosition, Camera.main.transform.position);
        return result;
    }

    /// <summary>
    /// Toggles the visibility of the spatial mesh renderer.
    /// </summary>
    private void ToggleSpatialMeshDisplay()
    {
        renderSpatialMeshEnabled = !renderSpatialMeshEnabled;
        SetSpatialMeshVisibility(renderSpatialMeshEnabled);
    }


    /// <summary>
    /// Sets the visibility of the spatial mesh renderer.
    /// </summary>
    /// <param name="visible"></param>
    private void SetSpatialMeshVisibility(bool visible)
    {
        SpatialMapping.Instance.DrawVisualMeshes = visible;
    }

    /// <summary>
    /// Analyzes the current world cursor position and determines if this position is a valid anchor position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>If there is a valid anchor position.</returns>
    private bool GetPositionForNewAnchor(out Vector3 pos)
    {
        if (WorldCursor.CursorHitSth && !WorldCursor.CursorHitSpatialCollider)
        {
            // Dont add new waypoints when the user if focusing sth
            pos = Vector3.zero;
            return false;
        }
        if (WorldCursor.CursorHitSpatialCollider) {
            pos = WorldCursor.CursorPosition;
            pos.y = CameraHelper.Stats.groundPos.y;
            return true;
        }
        else
        {
            var stats = CameraHelper.Stats;
            float beta = Mathf.Deg2Rad * (90 - (Vector3.Angle(Vector3.down, stats.camLookDir)));
            float lookDist = stats.eyeHeight * (Mathf.Cos(beta) / Mathf.Sin(beta));
            pos = stats.groundPos + stats.camLookDirInPlane * lookDist;

            return lookDist < 100 && lookDist > 0;
        }
    }

    /// <summary>
    /// Returns the anchor that is a suitable insert mode start point in respect of the world cursor position.
    /// Example: If the users looks on the line beterrn anchor A and Anchor B, the Anchor A is returned. If the
    /// user does not look at any line between anchors, null is returned.
    /// </summary>
    /// <returns></returns>
    private IAnchor GetPossibleInsertModeStartPoint()
    {
        Vector3 pos;
        if (GetPositionForNewAnchor(out pos))
        {
            List<IAnchor> anchorList = AnchorManager.Instance.AnchorList;
            for (int i = 1; i < anchorList.Count; i++)
            {
                Vector3 a1 = anchorList[i - 1].AnchorPosition;
                Vector3 a2 = anchorList[i].AnchorPosition;
                a1.y = 0;
                a2.y = 0;
                pos.y = 0;
                if (VectorUtils.CalcDistanceOfRay(a1, a2, pos) < 0.1f && VectorUtils.IsPointBetweenStartAndEnd(a1, a2, pos))
                {
                    return anchorList[i - 1];
                }
            }
        }
        return null;
    }


    private void DeleteAnchor()
    {
        AnchorManager.Instance.DeleteAnchor(GazedAnchor);
        SetAppendMode();
    }


    private void ResetAll()
    {
        AnchorManager.Instance.DeleteAllAnchors();
        SetAppendMode();
    }

    // not used anymore
    private void NewSelectionAnchor()
    {
        CreateNewAnchorAtWorldCursorPosition("Selection");
    }
   
    private void NewMonsterSelectionAnchor()
    {
        CreateNewAnchorAtWorldCursorPosition("MonsterZone");
    }

    private void NewNPCSlectionAnchor()
    {
        CreateNewAnchorAtWorldCursorPosition("NPC");
    }

    private void NewAnchor(string anchorID)
    {
        if (GazedAnchor != null)
        {
            CreateNewAnchorAtGivenPosition(anchorID, GazedAnchor.AnchorPosition);
            AnchorManager.Instance.DeleteAnchor(GazedAnchor);
        }
    }

    public List<KeywordCommand> GetSpeechCommands()
    {
        Condition condEditMode = Condition.New(() => ApplicationStateManager.IsEditMode);
        Condition condAnchorPlacingMode = Condition.New(() => mode == Mode.ANCHOR_PLACING);
        Condition condInsertPointFound = Condition.New(() => GetPossibleInsertModeStartPoint() != null);
        Condition condAnchorGazed = Condition.New(() => GazedAnchor != null);
        Condition condNoAnchorGazed = Condition.New(() => GazedAnchor == null);


        Condition condGazedAnchorSelection    = Condition.New(() => IsGazedAnchorSelection == true);
        Condition condGazedAnchorMonster      = Condition.New(() => IsGazedAnchorMonster == true);
        Condition condGazedAnchorNPC          = Condition.New(() => IsGazedAnchorNPC == true);

        List<KeywordCommand> result = new List<KeywordCommand>();

        result.Add(new KeywordCommand(() => { ReleaseGazedAnchor(); },   condEditMode.And(condAnchorPlacingMode),                                                                                            "Place", KeyCode.R));
 
        result.Add(new KeywordCommand(() => { ResetAll(); },             condEditMode.And(condAnchorPlacingMode.Not()).And(condNoAnchorGazed),                                                               "Reset All", KeyCode.C));



      //  result.Add(new KeywordCommand(() => { NewSelectionAnchor(); },   condEditMode.And(condAnchorPlacingMode.Not()).And(condNoAnchorGazed).And(condGazedAnchorSelection.Not()),                                     "New Selection Anchor", KeyCode.T));
        result.Add(new KeywordCommand(() => { NewMonsterSelectionAnchor(); }, condEditMode.And(condAnchorPlacingMode.Not()),                                                                   "New Monster Anchor", KeyCode.Z));
        result.Add(new KeywordCommand(() => { NewNPCSlectionAnchor(); },         condEditMode.And(condAnchorPlacingMode.Not()),                                                             "New NPC Anchor", KeyCode.U));

        // Enemies
        result.Add(new KeywordCommand(() => { NewAnchor("Rhino1"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorMonster),                                                                   "New Rhino 1", KeyCode.I));
        result.Add(new KeywordCommand(() => { NewAnchor("Rhino2"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorMonster),                                                                  "New Rhino 2", KeyCode.Alpha8));
        result.Add(new KeywordCommand(() => { NewAnchor("Rhino3"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorMonster),                                                                  "New Rhino 3"));
        result.Add(new KeywordCommand(() => { NewAnchor("Rhino4"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorMonster),                                                                      "New Rhino 4"));
        result.Add(new KeywordCommand(() => { NewAnchor("Devil1"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorMonster),                                                                        "New Devil 1", KeyCode.Alpha6));
        result.Add(new KeywordCommand(() => { NewAnchor("Devil2"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorMonster),                                                                        "New Devil 2", KeyCode.Alpha7));
        result.Add(new KeywordCommand(() => { NewAnchor("Dragon1"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorMonster),                                                                   "New Dragon 1", KeyCode.Alpha4));

        // NPCs
        result.Add(new KeywordCommand(() => { NewAnchor("Soldier"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorNPC),                                                                      "New Soldier", KeyCode.Alpha5));
        result.Add(new KeywordCommand(() => { NewAnchor("FemaleWarrior1"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorNPC),                                                                 "New Female Warrior 1",  KeyCode.O));
        result.Add(new KeywordCommand(() => { NewAnchor("FemaleWarrior2"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorNPC),                                                                 "New Female Warrior 2",  KeyCode.P));
        result.Add(new KeywordCommand(() => { NewAnchor("OldMan"); }, condEditMode.And(condAnchorPlacingMode.Not()).And(condGazedAnchorNPC),                                                                 "New Old Man", KeyCode.Alpha3));

        result.Add(new KeywordCommand(() => { DeleteAnchor(); },         condEditMode.And(condAnchorPlacingMode.Not()).And(condAnchorGazed),                                                                            "Delete", KeyCode.X));
        result.Add(new KeywordCommand(() => { GrabGazedAnchor(); },      condEditMode.And(condAnchorPlacingMode.Not()).And(condAnchorGazed),                                                                            "Take", KeyCode.Y));


        result.Add(new KeywordCommand(() => { ToggleSpatialMeshDisplay(); },    Condition.TRUE, "Toggle Mesh", KeyCode.Escape, KeywordCommandManager.EXPERT_MODE));
        result.Add(new KeywordCommand(() => { HideAnchors(); },                 Condition.TRUE, "Hide Anchors", KeyCode.Alpha5, KeywordCommandManager.EXPERT_MODE));
        result.Add(new KeywordCommand(() => { ShowAnchors(); },                 Condition.TRUE, "Show Anchors", KeyCode.Alpha6, KeywordCommandManager.EXPERT_MODE));

        return result;
    }

}
