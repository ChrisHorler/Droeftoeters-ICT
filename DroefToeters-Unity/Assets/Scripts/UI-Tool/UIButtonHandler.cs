using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIButtonHandler : MonoBehaviour
{
    public UISequenceDatabase sequenceDatabase;
    public Transform uiParent;
    
    private Dictionary<string, List<UISequencePanel>> panelDataMap = new Dictionary<string, List<UISequencePanel>>();

    private void Awake() {
        if (sequenceDatabase == null) {
            Debug.LogError("UIButtonHandler: No sequenceDatabase assigned!");
            return;
        }

        foreach (var seq in sequenceDatabase.sequences) {
            panelDataMap[seq.buttonName] = seq.panelData;
        }
        
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons) {
            string bName = btn.gameObject.name;
            if (panelDataMap.ContainsKey(bName)) {
                btn.onClick.AddListener(() =>
                {
                    Debug.Log($"UIButtonHandler: Clicked {bName}, resolving panels...");
                    List<UISequencePanel> data = panelDataMap[bName];
                    List<GameObject> resolvedPanels = UISequenceResolver.ResolvePanels(data, uiParent);
                    UIManager.instance.OpenPanelSequence(resolvedPanels);
                });
            }
            else {
                Debug.Log($"{bName} has no sequence panels assigned in the database");
            }
        }
    }
}