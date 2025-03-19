using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using ColorUtility = UnityEngine.ColorUtility;
using System.Collections;
using System.Text.RegularExpressions;

public class Validator
{
    // Password validation: Min 10 chars, at least one lowercase, uppercase, digit, and special character
    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{10,}$";
        return Regex.IsMatch(password, pattern);
    }

    // Email validation: Checks for a standard email format
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
}

public class LoginScript : MonoBehaviour
{
    private string passwordValue = "";
    private string secondPasswordValue = "";
    private string usernameValue = "";
    public TextMeshProUGUI errorMessageLabel;
    public TMP_InputField passwordField;
    public TMP_InputField secondPasswordField;
    private ApiConnecter apiConnecter;
    public string defaultSceneAfterLogin = "SampleScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        apiConnecter = FindFirstObjectByType<ApiConnecter>();
        StartCoroutine(DelayedRequest());
    }

    IEnumerator DelayedRequest()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(apiConnecter.SendAuthGetRequest("account/checkAccessToken", (string response, string error) =>
        {
            if (error == null)
            {
                if (MainManager.Instance.NavigationScene != null && MainManager.Instance.NavigationScene != "")
                {
                    SceneManager.LoadScene(MainManager.Instance.NavigationScene);
                }else
                {
                    SceneManager.LoadScene(defaultSceneAfterLogin);
                }
            }
            else
            {
                StartCoroutine(apiConnecter.SendAuthPostRequest(JsonConvert.SerializeObject(new { refreshToken = MainManager.Instance.LoginResponse.refreshToken }), "/account/refresh",
                (string response, string error) =>
                {
                    if (error == null)
                    {
                        Debug.Log($"Trying to use new token: {response}");
                        LoginResponse decodedResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                        MainManager.Instance.SetLoginCredentials(decodedResponse);
                        System.IO.File.WriteAllText("UserSettings/playerLogin.json", response);
                        if (MainManager.Instance.NavigationScene != null && MainManager.Instance.NavigationScene != "")
                        {
                            SceneManager.LoadScene(MainManager.Instance.NavigationScene);
                        }
                        else
                        {
                            SceneManager.LoadScene(defaultSceneAfterLogin);
                        }
                    }
                    else
                    {
                        string filePath = "UserSettings/playerLogin.json";
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }));
                
            }
        }));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RegisterUser()
    {
        if (!Validator.IsValidEmail(usernameValue))
        {
            errorMessageLabel.text = "Invalid MailAddress";
            return;
        }
        else if (!Validator.IsValidPassword(passwordValue))
        {
            if (passwordValue.Length < 10)
            {
                errorMessageLabel.text = "Password must be 10+ characters";
                return;
            }
            else
            {
                errorMessageLabel.text = "Password must have 1 lowercase, 1 uppercase, 1 number and 1 special character.";
                return;
            }
        } else if (secondPasswordField != null && passwordValue != secondPasswordValue)
        {
            errorMessageLabel.text = "The 2 Passwords are not the same.";
            return;
        }
        string json = JsonConvert.SerializeObject(new { email = usernameValue, password = passwordValue }, Formatting.Indented);
        Debug.Log(json);
        StartCoroutine(apiConnecter.SendPostRequest(json, "account/register", (string response, string error) =>
        {
            SetTextColor("#FFFFFF", errorMessageLabel);
            errorMessageLabel.text = "Connecting...";
            if (error == null)
            {
                Debug.Log("Response: " + response);
                SetTextColor("#FFFFFF", errorMessageLabel);
                errorMessageLabel.text = "Account Created! Re-enter password to Login.";
                passwordField.Select();
                passwordField.text = "";
                passwordValue = "";
                if (secondPasswordField != null)
                {
                    secondPasswordField.Select();
                    secondPasswordField.text = "";
                    secondPasswordValue = "";
                }
            }
            else
            {
                SetTextColor("#FF0000", errorMessageLabel);
                errorMessageLabel.text = "Username already taken.";
                Debug.LogError(error);
            }
        }));
    }

    private void LoginUser()
    {
        SetTextColor("#FFFFFF", errorMessageLabel);
        errorMessageLabel.text = "Connecting...";
        string json = JsonConvert.SerializeObject(new { email = usernameValue, password = passwordValue }, Formatting.Indented);
        Debug.Log(json);
        StartCoroutine(apiConnecter.SendPostRequest(json, "account/login", (string response, string error) =>
        {
            if (error == null)
            {
                errorMessageLabel.text = "";
                Debug.Log("Response: " + response);
                SceneManager.LoadScene(defaultSceneAfterLogin);
                LoginResponse decodedResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                MainManager.Instance.SetLoginCredentials(decodedResponse);
                System.IO.File.WriteAllText("UserSettings/playerLogin.json", response);
            }
            else
            {
                SetTextColor("#FF0000", errorMessageLabel);
                errorMessageLabel.text = "Invalid username & password combination.";
            }
        }));
    }

    public void ClickButton(string registerOrLogin)
    {
        errorMessageLabel.text = "";
        SetTextColor("#FF0000", errorMessageLabel);
        if (registerOrLogin == "Register")
        {
            RegisterUser();
        }
        else
        {
            LoginUser();
        }
    }

    public void SetPasswordValue(string value, bool secondPasswordField = false)
    {
        if (secondPasswordField)
        {
            secondPasswordValue = value;
        } else
        {
            passwordValue = value;
        }
    }

    public void SetTextColor(string colorText, TextMeshProUGUI element)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorText, out color))
        {
            element.color = color;
        }
        else
        {
            Debug.LogError("Invalid color string");
        }
    }

    public void SetUsernameValue(string value)
    {
        usernameValue = value;
    }
}
