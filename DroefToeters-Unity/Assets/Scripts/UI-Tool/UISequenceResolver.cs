using System.Collections.Generic;
using UnityEngine;

public static class UISequenceResolver {
    public static List<GameObject> ResolvePanels(
        List<UISequencePanel> panelData,
        Transform parent)
    {
        var result = new List<GameObject>();
        ScenePanelRegistry registry = Object.FindObjectOfType<ScenePanelRegistry>();
        if (registry == null) {
            Debug.LogWarning("UISequenceResolver: No ScenePanelRegistry found in this scene!");
        }

        foreach (var p in panelData) {
            switch (p.panelType) {
                case PanelType.SceneObject: {
                    if (registry != null) {
                        // Retrieve the scene object by ID
                        var sceneObj = registry.GetPanelById(p.scenePanelID);
                        if (sceneObj != null)
                            result.Add(sceneObj);
                        else
                            Debug.LogWarning($"UISequenceResolver: No scene object found for ID {p.scenePanelID}");
                    }
                    break;
                }

                case PanelType.Prefab: {
                    if (p.prefab != null) {
                        var instance = Object.Instantiate(p.prefab, parent);
                        instance.name = p.prefab.name;
                        result.Add(instance);
                    }
                    else {
                        Debug.LogWarning("UISequenceResolver: Prefab is null in panel data");
                    }
                    break;
                }
            }
        }
        return result;
    }
}