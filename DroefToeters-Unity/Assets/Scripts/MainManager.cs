using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now
    public static MainManager Instance;
    public LoginSaveFile LoginResponse; 
    public string NavigationScene; // the scene to go back to after login
    public string LoginDataSaveLocation = "UserSettings/playerLogin.json";
    public string LoginChoice = "";

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLoginCredentials(LoginSaveFile loginResponse)
    {
        MainManager.Instance.LoginResponse = loginResponse;
    }

    private void Start()
    {
        string filePath = LoginDataSaveLocation;
        if (System.IO.File.Exists(filePath))
        {
            string jsonString = System.IO.File.ReadAllText(filePath);
            Debug.Log(jsonString);
            LoginResponse = JsonConvert.DeserializeObject<LoginSaveFile>(jsonString);
        }
        else
        {
            Debug.Log("No data found.");
        }
    }
}