using System.Collections.Generic;
using UnityEngine;

public class ScenePanelRegistry : MonoBehaviour
{
    public static ScenePanelRegistry instance { get; private set; }
    private Dictionary<string, GameObject> panelMap = new Dictionary<string, GameObject>();

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        var allPanels = FindObjectsOfType<ScenePanelID>();
        foreach (var p in allPanels) {
            string id = p.panelID;
            if (!panelMap.ContainsKey(id)) {
                panelMap[id] = p.gameObject;
            }
            else {
                Debug.LogWarning($"Duplicate panel ID Detected: {id}");
            }
        }

        //Disable panels on scene start
        foreach (var p in panelMap.Values) {
            p.SetActive(false);
        }

    }

    public GameObject GetPanelById(string panelID) {
        if (panelMap.ContainsKey(panelID)) {
            return panelMap[panelID];
        }
        Debug.LogWarning($"ScenePanelRegistry: No panel found for ID '{panelID}'");
        return null;
    }
}