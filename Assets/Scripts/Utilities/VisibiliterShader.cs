using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibiliterShader : MonoBehaviour {

    private List<Material> fadingMaterials;
    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private bool renderersAreActive = true;

    [SerializeField] private float distanceThreshold = 10;
    [SerializeField] private float maxDistance = 15;

    private float distanceToPlayer;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        fadingMaterials = new List<Material>();

        foreach(MeshRenderer mr in meshRenderers)
        {
            fadingMaterials.Add(mr.material);
        }

        foreach(SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            fadingMaterials.Add(smr.material);
        }

        foreach (Material fadingMaterial in fadingMaterials)
        {
            fadingMaterial.SetFloat("_DistanceThreshold", distanceThreshold);
            fadingMaterial.SetFloat("_MaxDistance", maxDistance);
        }
    }

    private void Update()
    {
        distanceToPlayer = Utils.GetRelativeDistance(Camera.main.transform.position, transform.position);

        if (distanceToPlayer > maxDistance + 1)
        {
            if (renderersAreActive == true)
            {
                DisableMeshRenderers();
            }
        }
        else
        {
            if (renderersAreActive == false)
            {
                EnableMeshRenderers();
            }
            foreach (Material fadingMaterial in fadingMaterials)
            {
                fadingMaterial.SetFloat("_Distance", distanceToPlayer);
            }
        }
        
    }


    private void DisableMeshRenderers()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = false;
        }

        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            smr.enabled = false;
        }
        renderersAreActive = false;
    }

    private void EnableMeshRenderers()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = true;
        }

        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            smr.enabled = true;
        }
        renderersAreActive = true;
    }


}
