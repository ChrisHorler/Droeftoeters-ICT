using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public static RouteManager instance { get; private set; }
    public enum Route {RouteA, RouteB}
    public Route currentRoute = Route.RouteA;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
