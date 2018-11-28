using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour {


    [SerializeField] private float radarRange = 15;

    [SerializeField] private SpriteRenderer radarBackground;

    private GameObject EnemyIcon;
    private GameObject NPCIcon;

    private bool isShown = false;

    private Stack<GameObject> stack = new Stack<GameObject>();

    private void Awake()
    {
        EnemyIcon = Resources.Load<GameObject>("Radar_EnemyIcon");
        NPCIcon = Resources.Load<GameObject>("Radar_NPCIcon");
    }

    private void Update()
    {
        if (isShown == true)
        {
            // Empty stack
            foreach (GameObject gob in stack)
            {
                Destroy(gob);
            }
            stack.Clear();


            Vector3 campPos = Camera.main.transform.position;
            Vector3 camView = Camera.main.transform.forward;
            Vector2 viewDir = new Vector2(camView.x, camView.z);

            // Todo sort according to region
            string actualRegion = RegionManager.Instance.CurrentRegionName;

            DebugLog.Log(radarBackground.bounds.extents.ToString());

            Debug.Log(Mathf.Cos(Mathf.Deg2Rad * 350) + ", " + Mathf.Sin(Mathf.Deg2Rad * 350));
            Debug.Log(Mathf.Cos(Mathf.Deg2Rad * -10) + ", " + Mathf.Sin(Mathf.Deg2Rad * -10));

            foreach (IAnchor anchor in AnchorManager.Instance.AnchorList)
            {

                float relativeDistance = Utils.GetRelativeDistance(campPos, anchor.AnchorPosition) / radarRange;
                if (relativeDistance < 1.0f)
                {
                    Vector3 tmp = (anchor.AnchorPosition - campPos).normalized;
                    Vector2 anchorDir = new Vector2(tmp.x, tmp.z);


                    float angle = Utils.GetAbsoluteAngle(anchorDir) - Utils.GetAbsoluteAngle(viewDir);

                    Vector2 radarDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)).normalized * relativeDistance * 2.27f; //2.27 for scale of the radar

                    GameObject newIcon = new GameObject("icon");
                    if (anchor is INpc)
                    {
                        newIcon = Instantiate(NPCIcon, radarBackground.transform.position, radarBackground.transform.rotation);
                    }
                    else if (anchor is IEnemy)
                    {
                        newIcon = Instantiate(EnemyIcon, radarBackground.transform.position, radarBackground.transform.rotation);
                    }



                    newIcon.transform.parent = radarBackground.transform;
                    newIcon.transform.localPosition = new Vector3(radarDir.x, radarDir.y, 0);



                    stack.Push(newIcon);
                }
            }

        }
    }

    
    public void ShowRadar()
    {
        isShown = true;
        radarBackground.enabled = true;
    }

    public void HideRadar()
    {
        isShown = false;
        radarBackground.enabled = false;

        // Empty stack
        foreach (GameObject gob in stack)
        {
            Destroy(gob);
        }
        stack.Clear();
    }



}
