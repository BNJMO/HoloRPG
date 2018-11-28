using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route
{
    private List<IAnchor> anchors = new List<IAnchor>();
    private int index = 0;

    public Vector3 CurrentPosition
    {
        get
        {
            return anchors[index].AvatarTargetPosition;
        }
    }

    public virtual string CurrentPoiId
    {
        get
        {
            return PointOfInterestManager.Instance.GetIdOfAnchor(anchors[index]);
        }
    }

    public virtual bool CurrentIsPoi
    {
        get
        {
            return anchors[index] is PoiAnchor;
        }
    }

    public bool CurrentIsLast
    {
        get
        {
            return index == anchors.Count - 1;
        }
    }

    public virtual int RemainingProductsCount
    {
        get
        {
            int result = 0;
            for (int i = index; i < anchors.Count; i++)
            {
                result += (anchors[i] is PoiAnchor && !(anchors[i] is EntertainmentAnchor)) ? 1 : 0;
            }
            return result;
        }
    }

    public IAnchor Current
    {
        get { return anchors[index]; }
    }

    public Route(List<IAnchor> anchorlist)
    {
        anchors.AddRange(anchorlist);
    }

    public int Next()
    {
        if (!CurrentIsLast)
        {
            index++;
        }
        return index;
    }

    public virtual Route GetWayHome(bool shortestWay)
    {
        List<IAnchor> wayHome = new List<IAnchor>();

        // simple strategy: Add the shortest way
        if (!shortestWay || index >= (anchors.Count + 1) / 2)
        {
            // clock wise way home
            for (int i = index; i < anchors.Count; i++)
            {
                wayHome.Add(anchors[i]);
            }
            wayHome.Add(anchors[0]);
            if (shortestWay)
            {
                return new RouteWayHome(wayHome);
            }
            else
            {
                return new Route(wayHome);
            }

        }

        // couter clock wise way home
        for (int i = index; i >= 0; i--)
        {
            wayHome.Add(anchors[i]);
        }
        return new RouteWayHome(wayHome);

    }
}

public class RouteWayHome : Route
{
    // On the way home, we are not interested in products
    public override string CurrentPoiId { get { return null; } }
    public override bool CurrentIsPoi { get { return false; } }
    public override int RemainingProductsCount { get { return 0; } }

    public RouteWayHome(List<IAnchor> anchorlist) : base(anchorlist)
    {
    }

    public override Route GetWayHome(bool shortestWay)
    {
        return this;
    }
}