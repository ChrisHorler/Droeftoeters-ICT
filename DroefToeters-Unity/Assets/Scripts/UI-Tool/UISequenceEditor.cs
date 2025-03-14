using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Experimental;

public class UISequenceEditor : EditorWindow
{
    private UISequenceDatabase database;
    private string currentButtonName;
    private List<UISequencePanel> currentSequence = new List<UISequencePanel>();

    [MenuItem("Tools/UI Sequence Editor")]
    public static void ShowWindow() {
        GetWindow<UISequenceEditor>("UI Sequence Editor");
    }

    private void OnGUI() {
        GUILayout.Label("UI Sequence Editor", EditorStyles.boldLabel);
        
        database = (UISequenceDatabase)EditorGUILayout.ObjectField(
            "Database", database, typeof(UISequenceDatabase), true);
        
        currentButtonName = EditorGUILayout.TextField("Button Name", currentButtonName);
        
        GUILayout.Label("Sequence Steps", EditorStyles.boldLabel);

        for (int i = 0; i < currentSequence.Count; i++) {
            EditorGUILayout.BeginVertical("box");
            currentSequence[i].panelType = (PanelType)EditorGUILayout.EnumPopup("Panel Type", currentSequence[i].panelType);

            if (currentSequence[i].panelType == PanelType.SceneObject) {
                currentSequence[i].scenePanelID = EditorGUILayout.TextField("Scene Panel ID", currentSequence[i].scenePanelID);
            }
            else if (currentSequence[i].panelType == PanelType.Prefab) {
                currentSequence[i].prefab = (GameObject)EditorGUILayout.ObjectField(
                    "Prefab", currentSequence[i].prefab, typeof(GameObject), true);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Add Step")) {
            currentSequence.Add(new UISequencePanel());
        }

        if (GUILayout.Button("Remove Last Step")) {
            if (currentSequence.Count > 0 )
                currentSequence.RemoveAt(currentSequence.Count - 1);
        }
        EditorGUILayout.EndHorizontal();

        
        if (GUILayout.Button("Load Sequence") && database != null && !string.IsNullOrEmpty(currentButtonName)) {
            LoadSequence();
        }

        if (GUILayout.Button("Save Sequence") && database != null && !string.IsNullOrEmpty(currentButtonName)) {
            SaveSequence();
        }
    }


    private void LoadSequence()
    {
        var existing = database.sequences.Find(s => s.buttonName == currentButtonName);
        if (existing != null) {
            currentSequence = new List<UISequencePanel>(existing.panelData);
            Debug.Log($"Loaded sequence for '{currentButtonName}' with {currentSequence.Count} steps");
        }
        else {
            currentSequence.Clear();
            Debug.Log($"No sequence for '{currentButtonName}' with {currentSequence.Count} steps");
        }
    }

    private void SaveSequence()
    {
        if (database == null) {   
            Debug.LogError($"No UISequenceDatabase assigned");
        }
        
        var existing = database.sequences.Find(s => s.buttonName == currentButtonName);
        if (existing != null) {
            existing.panelData = new List<UISequencePanel>(currentSequence);
        }
        else
        {
            database.sequences.Add(new UISequenceDatabase.UISequence
            {
                buttonName = currentButtonName,
                panelData = new List<UISequencePanel>(currentSequence)
            });
        }

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Saved sequence for '{currentButtonName}' with {currentSequence.Count} steps");
    }
}
