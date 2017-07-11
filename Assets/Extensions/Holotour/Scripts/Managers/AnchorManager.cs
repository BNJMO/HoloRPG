using System.Collections.Generic;
using UnityEngine;

public class AnchorManager : HoloToolkit.Unity.Singleton<AnchorManager>
{
    [SerializeField]
    private Transform anchorParent;

   // private GameObject waypointPrefab;

    private List<IAnchor> anchorList = new List<IAnchor> ();

    /// <summary>
    /// Returns a copy of the list that contains all anchors in their current state.
    /// </summary>
    public List<IAnchor> AnchorList { get { return new List<IAnchor>(anchorList); } }

    void Start()
    {
     //   waypointPrefab = (Resources.Load("Waypoint") as GameObject);
    }

   /* public IAnchor InsertWaypoint(Vector3 position, int insertIndex)
    {
        return InsertNewAnchor(position, insertIndex, waypointPrefab);
    }

    public IAnchor AddWaypoint(Vector3 position)
    {
        return AddNewAnchor(position, waypointPrefab);
    }*/

 /*   public IAnchor InsertAnchor(string anchorID, Vector3 position, int insertIndex)
    {
        return InsertNewAnchor(position, insertIndex, PointOfInterestManager.Instance.GetPOI(anchorID).AnchorPrefab);
    }*/

    public IAnchor AddAnchor(string anchorID, Vector3 position)
    {
        return AddNewAnchor(position, PointOfInterestManager.Instance.GetPOI(anchorID).AnchorPrefab);
    }

    /// <summary>
    /// Creates a new anchor and adds it to the anchor list
    /// </summary>
    /// <param name="position">Where to spawn the anchor (in local space of the AnchorManager).</param>
    private IAnchor InsertNewAnchor(Vector3 position, int insertIndex, GameObject prefab)
    {
        IAnchor newAnchor = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<IAnchor>();
        newAnchor.AnchorPosition = position;
        anchorList.Insert(insertIndex, newAnchor);
        newAnchor.GameObject.transform.parent = anchorParent;
        return newAnchor;
    }
    

    /// <summary>
    /// Creates a new anchor and adds it to the anchor list
    /// </summary>
    /// <param name="position">Where to spawn the anchor (in local space of the AnchorManager).</param>
	private IAnchor AddNewAnchor(Vector3 position, GameObject prefab)
    {
        return InsertNewAnchor(position, anchorList.Count, prefab);
    }

    /// <summary>
    /// Deletes the selected Anchor if any selected otherwise delete the last created anchor.
    /// </summary>
    public void DeleteAnchor(IAnchor anchorToDelete)
    {
        anchorList.Remove(anchorToDelete);
        Destroy(anchorToDelete.GameObject);
    }

    /// <summary>
    /// Clears all current anchors.
    /// </summary>
    public void DeleteAllAnchors()
    {
        if (anchorList != null)
        {
            foreach (IAnchor a in anchorList)
            {
                Destroy(a.GameObject);
            }
            anchorList.Clear();
        }
    }

    /// <summary>
    /// Returns the index position of the given anchor.
    /// </summary>
    /// <param name="anchor"></param>
    /// <returns></returns>
	public int GetIndexOf(IAnchor anchor)
    {
        return anchorList.IndexOf(anchor);
    }

    /// <summary>
    /// Returns the current count of anchors.
    /// </summary>
    /// <returns></returns>
    public int GetAnchorsCount()
    {
        return anchorList.Count;
    }

    /// <summary>
    /// Returns an anchor at index "index".
    /// </summary>
    /// <param name="index">Index of anchor to be returned.</param>
    public IAnchor GetAnchor(int index)
    {
        if ((index >= 0) && (index < anchorList.Count))
        {
            return anchorList[index];
        }
        else
        {
            Debug.LogError("Fatal error! Given index is out of bound.");
            return null;
        }
    }
}
