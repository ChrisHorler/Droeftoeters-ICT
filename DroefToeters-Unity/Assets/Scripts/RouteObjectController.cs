using System;
using UnityEngine;

public class RouteObjectController : MonoBehaviour
{
    public GameObject[] aRouteObjects;
    public GameObject[] bRouteObjects;

    private void Start()
    {
        if (!RouteManager.instance) return;
        
        bool isA = (RouteManager.instance.currentRoute == RouteManager.Route.RouteA);
        bool isB = (RouteManager.instance.currentRoute == RouteManager.Route.RouteB);

        foreach (var obj in aRouteObjects) {
            if (obj != null) obj.SetActive(isA);
        }

        foreach (var obj in bRouteObjects) {
            if (obj != null) obj.SetActive(isB);
        }
    }
}

