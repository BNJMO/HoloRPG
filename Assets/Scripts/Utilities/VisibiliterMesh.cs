using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component with which the visibility of a GameObject can be controller
/// </summary>
public class VisibiliterMesh : MonoBehaviour {

    public bool IsVisible { get { return isVisible; } set { SetVisibility(value); } }

    private MeshRenderer[] myMeshRenderers;
    private SkinnedMeshRenderer[] mySkinnedMeshRenderers;
    
    public Condition VisibilityCondition;

    private bool isVisible = true;

	void Awake()
    {
        myMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        mySkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    void Start()
    {
        if (VisibilityCondition == null) Debug.LogError("Warning! Visibiliter needs a VisibilityCondition");
    }

    void Update()
    {
        // Update visibility only on the frame when it is changed
        if (isVisible != VisibilityCondition.IsTrue)
        {
            IsVisible = VisibilityCondition.IsTrue;
        } 
    }

    public void SetVisibility(bool visible)
    {
        if (myMeshRenderers.Length != 0)
        {
            foreach(MeshRenderer meshRenderer in myMeshRenderers)
            {
                meshRenderer.enabled = visible;
            }
        }
        if (mySkinnedMeshRenderers.Length != 0)
        {
            foreach(SkinnedMeshRenderer skinnedMeshRenderer in mySkinnedMeshRenderers)
            {
                skinnedMeshRenderer.enabled = visible;
            }
        }
        if ((mySkinnedMeshRenderers.Length == 0) && (myMeshRenderers.Length == 0))
        {
            Debug.LogError("No mesh renderer on this object : " + name);
        }

        isVisible = visible;
    }
}
