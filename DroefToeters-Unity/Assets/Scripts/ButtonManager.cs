using UnityEngine;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    public List<ButtonController> buttons;

    private void Start()
    {
        LoadButtonStates();
    }

    public void UnlockButton(int index)
    {
        if (index < buttons.Count)
        {
            buttons[index].SetState(ButtonState.Unlocked);
            SaveButtonStates();
        }
    }

    public void MarkButtonCompleted(int index)
    {
        if (index < buttons.Count)
        {
            buttons[index].SetState(ButtonState.Completed);
            SaveButtonStates();
        }
    }

    private void SaveButtonStates()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            PlayerPrefs.SetInt($"ButtonState_{i}", (int)buttons[i].currentState);
        }
        PlayerPrefs.Save();
    }

    private void LoadButtonStates()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (PlayerPrefs.HasKey($"ButtonState_{i}"))
            {
                buttons[i].SetState((ButtonState)PlayerPrefs.GetInt($"ButtonState_{i}"));
            }
        }
    }
}