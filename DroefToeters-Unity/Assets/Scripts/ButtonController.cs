using UnityEngine;
using UnityEngine.UI;

public enum ButtonState {
    Locked,
    Unlocked,
    Completed
}

public class ButtonController : MonoBehaviour {
    public ButtonState currentState = ButtonState.Locked;
    public Button button;
    public Image buttonImage;

    public Color completedColor = Color.green;
    public Color lockedColor = Color.red;
    public Color unlockedColor = Color.yellow;

    private void Start() { 
        UpdateButtonState();   
    }

    public void SetState(ButtonState newState) {
        currentState = newState;
        UpdateButtonState();
    }
    
    private void UpdateButtonState() {
        switch (currentState) {
            case ButtonState.Locked:
                button.interactable = false;
                buttonImage.color = lockedColor;
                break;
            case ButtonState.Unlocked:
                button.interactable = true;
                buttonImage.color = unlockedColor;
                break;
            case ButtonState.Completed:
                button.interactable = true;
                buttonImage.color = completedColor;
                break;
        }
    }
}