using UnityEngine;
using UnityEngine.UI;

public class RouteToggleUI : MonoBehaviour
{
    [SerializeField] private Toggle routeToggleA;
    [SerializeField] private Toggle routeToggleB;
    
    private bool isInitializing = false;
    private bool isRecursing = false;

    private void OnEnable() {
        if (RouteManager.instance == null) return;
        isInitializing = true;
        
        switch (RouteManager.instance.currentRoute) {
            case RouteManager.Route.RouteA:
                routeToggleA.isOn = true;
                routeToggleB.isOn = false;
                break;

            case RouteManager.Route.RouteB:
                routeToggleA.isOn = false;
                routeToggleB.isOn = true;
                break;
            
            default:
                routeToggleA.isOn = true;
                routeToggleB.isOn = false;
                RouteManager.instance.currentRoute = RouteManager.Route.RouteA;
                break;
        }
        
        isInitializing = false;
    }

    private void Start() {
        //routeToggleB.isOn = false;
        
        routeToggleA.onValueChanged.AddListener(OnToggleAChanged);
        routeToggleB.onValueChanged.AddListener(OnToggleBChanged);
    }

    private void OnToggleAChanged(bool isOn) {
        if (isInitializing || isRecursing) return;
        
        if (!isOn) {
            isRecursing = true;
            routeToggleB.isOn = true;   
            isRecursing = false;

            RouteManager.instance.currentRoute = RouteManager.Route.RouteB;
            Debug.Log("Forced B on because A was turned off");
        }
        else {
            isRecursing = true;
            routeToggleB.isOn = false;
            isRecursing = false;

            RouteManager.instance.currentRoute = RouteManager.Route.RouteA;
            Debug.Log("Route A selected, forced B off");
        }
    }

    private void OnToggleBChanged(bool isOn) {
        if (isInitializing || isRecursing) return;

        if (!isOn) {
            isRecursing = true;
            routeToggleA.isOn = true;
            isRecursing = false;

            RouteManager.instance.currentRoute = RouteManager.Route.RouteA;
            Debug.Log("Forced A on because B was turned off");
        }
        else {
            isRecursing = true;
            routeToggleA.isOn = false;
            isRecursing = false;

            RouteManager.instance.currentRoute = RouteManager.Route.RouteB;
            Debug.Log("Route B selected, forced A off");
        }
    }
}
