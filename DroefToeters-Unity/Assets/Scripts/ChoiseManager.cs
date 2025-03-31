using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiseManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLoginScreen(string choice)
    {
        MainManager.Instance.LoginChoice = choice;
        SceneManager.LoadScene("FunctionalLogin");
    }
}
