using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    
    [Header("UI Navigation Elements")]
    public Button leftButton;
    public Button rightButton;
    public Button closeButton;
    
    private List<GameObject> activePanels = new List<GameObject>();
    private int currentStep = 0;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        if (leftButton) leftButton.onClick.AddListener(PreviousStep);
        if (rightButton) rightButton.onClick.AddListener(NextStep);
        if (closeButton) closeButton.onClick.AddListener(ClosePanel);
    }

    public void OpenPanelSequence(List<GameObject> panels) {
        if (panels == null || panels.Count == 0) {
            Debug.LogError("UIManager: Invalid panel sequence");
            return;
        }
        ClosePanel();
        
        activePanels = panels;
        currentStep = 0;
        
        UpdateStepDisplay();
    }
    
    private void UpdateStepDisplay() {
        for (int i = 0; i < activePanels.Count; i++) {
            activePanels[i].SetActive(false);
        }
        
        activePanels[currentStep].SetActive(true);
        Debug.Log($"UIManager: Now showing {activePanels[currentStep].name} (Step {currentStep + 1}/{activePanels.Count})");

        if (leftButton) leftButton.interactable = (currentStep > 0);
        if (rightButton) rightButton.interactable = (currentStep < activePanels.Count - 1);
    }
    
    public void NextStep() {
        if (currentStep < activePanels.Count - 1) {
            currentStep++;
            UpdateStepDisplay();
        }
    }

    public void PreviousStep() {
        if (currentStep > 0) {
            currentStep--;
            UpdateStepDisplay();
        }
    }

    public void ClosePanel() {
        foreach (var panel in activePanels) {
            panel.SetActive(false);
        }
        
        activePanels.Clear();
        currentStep = 0;
    }
    
    
}