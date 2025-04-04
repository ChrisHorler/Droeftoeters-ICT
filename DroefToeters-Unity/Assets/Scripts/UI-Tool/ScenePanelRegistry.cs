using System.Collections.Generic;
using UnityEngine;

public class ScenePanelRegistry : MonoBehaviour {
    private Dictionary<string, GameObject> panelMap = new Dictionary<string, GameObject>();
    private void Awake() {
        var allPanels = FindObjectsOfType<ScenePanelID>();
        foreach (var p in allPanels)
        {
            string id = p.panelID;
            if (!panelMap.ContainsKey(id)) {
                panelMap[id] = p.gameObject;
            }
            else {
                Debug.LogWarning($"Duplicate panel ID detected: {id}");
            }
        }

       
        foreach (var p in panelMap.Values) {
            p.SetActive(false);
        }
    }
    public GameObject GetPanelById(string panelID) {
        if (panelMap.TryGetValue(panelID, out GameObject panel)) {
            return panel;
        }
        Debug.LogWarning($"ScenePanelRegistry: No panel found for ID '{panelID}'");
        return null;
    }
}