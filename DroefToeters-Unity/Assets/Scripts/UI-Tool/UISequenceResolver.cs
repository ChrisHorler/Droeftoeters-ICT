using System.Collections.Generic;
using UnityEngine;

public static class UISequenceResolver
{
    public static List<GameObject> ResolvePanels(
        List<UISequencePanel> panelData,
        Transform parent)
    {
        var result = new List<GameObject>();

        foreach (var p in panelData) {
            switch (p.panelType) {
                case PanelType.SceneObject:
                    var sceneObj = ScenePanelRegistry.instance.GetPanelById(p.scenePanelID);
                    if(sceneObj != null)
                        result.Add(sceneObj);
                    else
                        Debug.LogWarning($"UISequenceResolver: Scene object not found for ID {p.scenePanelID}");

                    break;
                
                case PanelType.Prefab:
                    if (p.prefab != null) {
                        var instance = Object.Instantiate(p.prefab, parent);
                        instance.name = p.prefab.name;
                        result.Add(instance);
                    }
                    else
                        Debug.LogWarning($"UISequenceResolver: Prefab is null in panel data");

                    break;
            }
        }
        return result;
    }
}