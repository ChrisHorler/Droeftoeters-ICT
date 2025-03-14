using UnityEngine;

[System.Serializable]
public class UISequencePanel
{
    public PanelType panelType;
    [Header("If SceneObject")]
    public string scenePanelID;
    
    [Header("If Prefab")]
    public GameObject prefab;
}