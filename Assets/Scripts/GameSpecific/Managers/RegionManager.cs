using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IRegion
{
    string RegionName { get; }
}

public class RegionManager : Singleton<RegionManager> {

    public string CurrentRegionName { get { return isInWild ? "The Wild" : currentRegion.regionName; } }


    private IDictionary<string, Region> myRegions;

    private List<Region> visitedRegions;

    [SerializeField] private float SMALL_DISTANCE = 3;
    [SerializeField] private float MEDIUM_DISTANCE = 15;
    [SerializeField] private float LARGE_DISTANCE = 25;

    private class Region : IRegion
    {
        public string regionName = "NoRegion";
        public Vector3 centroid;
        public float radius;
        public List<Vector3> points;

        public string RegionName { get { return regionName; } }

        public Region(string regionName)
        {
            this.regionName = regionName;
            points = new List<Vector3>();
        }

        public Region()
        {
            regionName = "NoRegion";
            Vector3 centroid = new Vector3(999, 999, 999);
            points = new List<Vector3>();
        }
    }

    [System.Serializable]
    public class RegionDecoration
    {
        [SerializeField] public string regionName;
        [SerializeField] public GameObject particleEffect;
        [SerializeField] public GameObject mainMonument;
        [SerializeField] public GameObject[] smallDecorationItems;
        [SerializeField] public GameObject[] mediumDecorationItems;
        [SerializeField] public GameObject[] largeDecorationItems;
        [SerializeField] public AudioClip backgroundClip;
    }

    [SerializeField]
    private RegionDecoration[] regionDecorations;

    private bool initComplete = false;
    private Region currentRegion;
    private bool isInWild = true;
    // to counter first frame initialization of currentRegion
    private bool firstFrame = true;


    

	protected override void Awake()
    {
        base.Awake();

        visitedRegions = new List<Region>();

        currentRegion = new Region(); // wild

        StartCoroutine(InitRoutine());
    }

    void Update()
    {
        if (initComplete == true)
        {
            UpdateNearestRegion();
        }
    }

    private void UpdateNearestRegion()
    {
        // look for nearest region
        Region nearestRegion = currentRegion;
        float distanceUserCentroid_old = Utils.GetRelativeDistance(nearestRegion.centroid, Camera.main.transform.position);
        foreach(string key in myRegions.Keys) // for each region
        {
            // initialize current Region 
            if (currentRegion.regionName == "NoRegion")
            {
                currentRegion = myRegions[key];
                nearestRegion = currentRegion;
                distanceUserCentroid_old = Utils.GetRelativeDistance(nearestRegion.centroid, Camera.main.transform.position);
            }
            // make sure this is not the same region
            else if (key != currentRegion.regionName)
            {
                float distanceUserCentroid_new = Utils.GetRelativeDistance(myRegions[key].centroid, Camera.main.transform.position);

                // update nearest region if distance smaller
                if (distanceUserCentroid_new < distanceUserCentroid_old)
                {
                    nearestRegion = myRegions[key];
                    distanceUserCentroid_old = Utils.GetRelativeDistance(nearestRegion.centroid, Camera.main.transform.position);
                }
            }   
        }
        Debug.DrawLine(Camera.main.transform.position, nearestRegion.centroid, Color.green);

        // Check if player is inside the nearest region found
        if (distanceUserCentroid_old < nearestRegion.radius)
        {
            if (isInWild == true)
            {
                isInWild = false;
                GameManger.Instance.InvokeUpdateCanvasUI();
            }
            
            // Entered new region ?
            if ((nearestRegion != currentRegion) || (firstFrame == true))
            {
                currentRegion = nearestRegion;

                // See if this region has never been visited before
                if (visitedRegions.Contains(currentRegion) == false)
                {
                    visitedRegions.Add(currentRegion);
                    // skip XP reward for first region "?"
                    if (firstFrame == true)
                    {
                        GameManger.Instance.InvokeRegionEntered(currentRegion, false);
                    }
                    else // not first region
                    {
                        GameManger.Instance.InvokeRegionEntered(currentRegion, true);
                    }
                    
                }
                else // entered a region that has been visited before
                {
                    GameManger.Instance.InvokeRegionEntered(currentRegion, false);
                }
            }
            
            Debug.DrawLine(Camera.main.transform.position, nearestRegion.centroid, Color.green);
        }
        else // Player is in the wild
        {
            isInWild = true;
            GameManger.Instance.InvokeUpdateCanvasUI();
            Debug.DrawLine(Camera.main.transform.position, nearestRegion.centroid, Color.yellow);
        }

        firstFrame = false;
    }

    IEnumerator InitRoutine()
    {
        myRegions = new Dictionary<string, Region>();

        yield return new WaitForSeconds(0.5f);

        // get all points from all the anchor in the scene
        foreach(IAnchor anchor in AnchorManager.Instance.AnchorList)
        {
            if (anchor.Region != "NoRegion")
            {
                if (myRegions.ContainsKey(anchor.Region) == false)
                {
                    myRegions[anchor.Region] = new Region(anchor.Region);
                }
                 myRegions[anchor.Region].points.Add(anchor.AnchorPosition);
            }
        }

        yield return new WaitForEndOfFrame();

        // calculate centroid
        foreach(string key in myRegions.Keys) // for each region
        {
            Vector3 sumVector = Vector3.zero;
            int sumIndices = 0;
            foreach(Vector3 point in myRegions[key].points) // for each point in region
            {
                sumVector += point;
                sumIndices++;
            }
            myRegions[key].centroid = sumVector / sumIndices;
            
#if UNITY_EDITOR
            // Debug centroid position 
            GameObject debug = Instantiate(Resources.Load<GameObject>("DebugCube"), myRegions[key].centroid, Quaternion.identity);
            debug.name = key;
#endif
        }

        yield return new WaitForEndOfFrame();

        // calculate radius
        foreach(string key in myRegions.Keys) // for each region
        {
            float maxRadius = 0;
            foreach(Vector3 point in myRegions[key].points) // for each point in region
            {
                float distanceFromCentroidToPoint = Vector3.Distance(myRegions[key].centroid, point);
                if(distanceFromCentroidToPoint > maxRadius)
                {
                    maxRadius = distanceFromCentroidToPoint;
                }
            }
            myRegions[key].radius = maxRadius + 3; // add 3 meters to the borders
            Debug.DrawLine(myRegions[key].centroid, myRegions[key].centroid + transform.forward * myRegions[key].radius, Color.blue, 500);
            Debug.DrawLine(myRegions[key].centroid, myRegions[key].centroid - transform.forward * myRegions[key].radius, Color.blue, 500);
            Debug.DrawLine(myRegions[key].centroid, myRegions[key].centroid + transform.right * myRegions[key].radius, Color.blue, 500);
            Debug.DrawLine(myRegions[key].centroid, myRegions[key].centroid - transform.right * myRegions[key].radius, Color.blue, 500);
        }

        yield return new WaitForEndOfFrame();

        // Decorate scene
        DecorateMethod_1();
        //DecorateMethod_2();
        
        initComplete = true;
    }

    


    private void DecorateMethod_1()
    {
     /*   SMALL_DISTANCE = 3;
        MEDIUM_DISTANCE = 15;
        LARGE_DISTANCE = 25;*/
        foreach(string key in myRegions.Keys) // for each region
        {
            RegionDecoration rDeco = GetRegionDecoration(key);
            if (rDeco == null) break;

            // add a particle effect if available
            if (rDeco.particleEffect != null)
            {
                GameObject obj = Instantiate(rDeco.particleEffect, myRegions[key].centroid, Quaternion.identity);
                obj.AddComponent<VisibiliterParticle>().VisibilityCondition = Condition.New(() => Utils.GetRelativeDistance(obj.transform.position, Camera.main.transform.position) < 15);
            }

            // Go through all points and create a deco element with the remaining other elements
            for (int i = 0; i < myRegions[key].points.Count; i++)
            {
                for (int j = i + 1; j < myRegions[key].points.Count; j++)
                {
                    // check if the two anchors have almost the same height
                    if (Mathf.Abs(myRegions[key].points[i].y - myRegions[key].points[j].y) < 0.3f)
                    {
                        float distance = Vector3.Distance(myRegions[key].points[i], myRegions[key].points[j]);
                        Vector3 center = (myRegions[key].points[i] + myRegions[key].points[j]) / 2.0f;
                        GameObject deco = null;
                        if (distance > LARGE_DISTANCE)
                        {
                            if (rDeco.largeDecorationItems.Length == 0) break;
                            deco = rDeco.largeDecorationItems[Utils.GetRndIndex(rDeco.largeDecorationItems.Length)];
                            deco.name = "Large_Deco";
                        }
                        else if (distance > MEDIUM_DISTANCE)
                        {
                            if (rDeco.mediumDecorationItems.Length == 0) break;
                            deco = rDeco.mediumDecorationItems[Utils.GetRndIndex(rDeco.mediumDecorationItems.Length)];
                            deco.name = "Medium_Deco";
                        }
                        else if (distance > SMALL_DISTANCE)
                        {
                            if (rDeco.smallDecorationItems.Length == 0) break;
                            deco = rDeco.smallDecorationItems[Utils.GetRndIndex(rDeco.mediumDecorationItems.Length)];
                            deco.name = "Small_Deco";
                        }
                        if (deco != null) {
                            GameObject obj = Instantiate(deco, center, Utils.GetRndStandRotation());
                            //cast a ray to detect and place the object on the floor
                            Vector3 shiftedPosition = new Vector3(center.x, center.y + 2, center.z);
                            RaycastHit hit;
                            if (Physics.Raycast(center, Vector3.down, out hit, Mathf.Infinity, SpatialMapping.PhysicsRaycastMask))
                            {
                                center = hit.point;
                            }
                            // add components
                            obj.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
                            obj.AddComponent<VisibiliterMesh>().VisibilityCondition = Condition.New(() => Utils.GetRelativeDistance(Camera.main.transform.position, obj.transform.position) < 15);
                        }
                    }
                }
            }
        }

    
    }


    private void DecorateMethod_2()
    {
        SMALL_DISTANCE = 3;
        MEDIUM_DISTANCE = 8;
        LARGE_DISTANCE = 15;
        foreach(string key in myRegions.Keys) // for each region
        {
            RegionDecoration rDeco = GetRegionDecoration(key);
            if (rDeco == null) break;

            // add a particle effect if available
            if (rDeco.particleEffect != null)
            {
                GameObject obj = Instantiate(rDeco.particleEffect, myRegions[key].centroid, Quaternion.identity);
                obj.AddComponent<VisibiliterParticle>().VisibilityCondition = Condition.New(() => Utils.GetRelativeDistance(obj.transform.position, Camera.main.transform.position) < 15);
            }

            // Go through all points and create a deco element with the centroid
            for (int i = 0; i < myRegions[key].points.Count; i++)
            {      
                float distanceToCentroid = Vector3.Distance(myRegions[key].centroid, myRegions[key].points[i]);
                Vector3 center = (myRegions[key].points[i] + myRegions[key].centroid) / 2.0f;
                GameObject deco = null;
                if (distanceToCentroid > LARGE_DISTANCE)
                {
                    if (rDeco.largeDecorationItems.Length == 0) break;
                    deco = rDeco.largeDecorationItems[Utils.GetRndIndex(rDeco.largeDecorationItems.Length)];
                    deco.name = "Large_Deco";
                }
                else if (distanceToCentroid > MEDIUM_DISTANCE)
                {
                    if (rDeco.mediumDecorationItems.Length == 0) break;
                    deco = rDeco.mediumDecorationItems[Utils.GetRndIndex(rDeco.mediumDecorationItems.Length)];
                    deco.name = "Medium_Deco";
                }
                else if (distanceToCentroid > SMALL_DISTANCE)
                {
                    if (rDeco.smallDecorationItems.Length == 0) break;
                    deco = rDeco.smallDecorationItems[Utils.GetRndIndex(rDeco.mediumDecorationItems.Length)];
                    deco.name = "Small_Deco";
                }
                if (deco != null) {
                    GameObject obj = Instantiate(deco, center, Utils.GetRndStandRotation());
                    obj.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
                    obj.AddComponent<VisibiliterMesh>().VisibilityCondition = Condition.New(() => Utils.GetRelativeDistance(Camera.main.transform.position, obj.transform.position) < 15);
                }
            }
        }
    }
    

    


    private RegionDecoration GetRegionDecoration(string region)
    {
        foreach(RegionDecoration rd in regionDecorations)
        {
            if (rd.regionName == region)
            {
                return rd;
            }
        }
        return null;
    }
}
