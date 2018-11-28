using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestManager : Singleton<PointOfInterestManager>
{
    [System.Serializable]
    public class PointOfInterest
    {
        [SerializeField]
        public string Id;
        [SerializeField]
        public GameObject AnchorPrefab;
        [SerializeField]
        public GameObject GizmoPrefab;
    }

    [SerializeField]
    private PointOfInterest[] pointsOfInterest;

    public List<PointOfInterest> GetPOIs()
    {
        return new List<PointOfInterest>(pointsOfInterest);
    }

    public PointOfInterest GetPOI(string id)
    {
        foreach (PointOfInterest poi in pointsOfInterest)
        {
            if (poi.Id.Equals(id))
            {
                return poi;
            }
        }
        return null;
    }

    public string GetIdOfAnchor(IAnchor poi)
    {
        if (poi is PoiAnchor)
        {
            return (poi as PoiAnchor).PoiId;
        } else
        {
            return null;
        }
    }
}
