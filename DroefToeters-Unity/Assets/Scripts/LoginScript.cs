using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using ColorUtility = UnityEngine.ColorUtility;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Serialization;

public class ChildData
{
    public string userId { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public LoginResponse? loginResponse { get; set; }
}

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
    public TextMeshProUGUI parentRegisterErrorMessageLabel;
    public TMP_InputField parentRegisterUsernameField;
    public TMP_InputField parentRegisterPasswordField;
    public TMP_InputField parentRegisterSecondPasswordField;
    public TextMeshProUGUI parentLoginErrorMessageLabel;
    public TMP_InputField parentLoginUsernameField;
    public TMP_InputField parentLoginPasswordField;
    public TextMeshProUGUI childLoginErrorMessageLabel;
    public TMP_InputField childLoginUsernameField;
    public TMP_InputField childLoginPasswordField;
    public TextMeshProUGUI childRegisterErrorMessageLabel;
    public TMP_InputField childRegisterUsernameField;
    public TMP_InputField childRegisterPasswordField;
    public TMP_InputField childRegisterSecondPasswordField;
    private ApiConnecter apiConnecter;
    public string ParentlSceneAfterLogin = "";
    public string ChildSceneAfterLogin = "";
    public GameObject ChildPanel;
    public GameObject ParentPanel;
    public string currentSceneName = "FunctionalLogin";
    public bool loginPage = true;
    public GameObject ChildRegisterLoadingPanel;
    private string parentUserId;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!loginPage)
        {
            MainManager.Instance.NavigationScene = currentSceneName;
        }
        apiConnecter = FindFirstObjectByType<ApiConnecter>();
        StartCoroutine(DelayedRequest());
        if (MainManager.Instance.LoginChoice == "Parent")
        {
            ParentPanel.SetActive(true);
            ChildPanel.SetActive(false);
        }
        if (MainManager.Instance.LoginChoice == "Child")
        {
            ParentPanel.SetActive(false);
            ChildPanel.SetActive(true);
        }
    }

    IEnumerator DelayedRequest()
    {
        yield return new WaitForSeconds(1f);
        if (loginPage)
        {
            RefreshSessionToken();
        }
        else
        {
            CheckLoginStatus();
        }
    }

    private void CheckLoginStatus()
    {
        StartCoroutine(apiConnecter.SendRequest("account/checkAccessToken", HttpMethod.GET, true, (string response, string error) =>
        {
            // request the user id insted of the "account/checkAccessToken"
        }));
    }

    private void RefreshSessionToken()
    {
        StartCoroutine(apiConnecter.SendRequest("account/checkAccessToken", HttpMethod.GET, true, (string response, string error) =>
        {
            if (error == null)
            {
                if (MainManager.Instance.NavigationScene != null && MainManager.Instance.NavigationScene != "")
                {
                    SceneManager.LoadScene(MainManager.Instance.NavigationScene);
                }
                else
                {
                    if (MainManager.Instance.LoginResponse != null && MainManager.Instance.LoginResponse.isChild)
                    {
                        SceneManager.LoadScene(ChildSceneAfterLogin);
                    }
                    else
                    {
                        SceneManager.LoadScene(ParentlSceneAfterLogin);
                    }
                }
            }
            else
            {
                if (MainManager.Instance.LoginResponse != null)
                {
                    StartCoroutine(apiConnecter.SendRequest("account/refresh", HttpMethod.POST, true, (string response, string error) =>
                    {
                        if (error == null)
                        {
                            Debug.Log($"Trying to use new token: {response}");
                            LoginResponse decodedResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                            LoginSaveFile values = new LoginSaveFile(decodedResponse, MainManager.Instance.LoginResponse.isChild);
                            MainManager.Instance.SetLoginCredentials(values);
                            response = JsonConvert.SerializeObject(values);
                            System.IO.File.WriteAllText(MainManager.Instance.LoginDataSaveLocation, response);
                            if (MainManager.Instance.NavigationScene != null && MainManager.Instance.NavigationScene != "")
                            {
                                SceneManager.LoadScene(MainManager.Instance.NavigationScene);
                            }
                            else
                            {
                                if (MainManager.Instance.LoginResponse != null && MainManager.Instance.LoginResponse.isChild)
                                {
                                    SceneManager.LoadScene(ChildSceneAfterLogin);
                                }
                                else
                                {
                                    SceneManager.LoadScene(ParentlSceneAfterLogin);
                                }
                            }
                        }
                        else
                        {
                            string filePath = MainManager.Instance.LoginDataSaveLocation;
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    },
                    JsonConvert.SerializeObject(new { refreshToken = MainManager.Instance.LoginResponse.refreshToken }),
                    false));
                }
                else
                {
                    string filePath = MainManager.Instance.LoginDataSaveLocation;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

            }
        }, "", false));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetErrorMessages(string text)
    {
        if (parentRegisterErrorMessageLabel != null)
        {
            parentRegisterErrorMessageLabel.text = text;
        }
        if (parentLoginErrorMessageLabel != null)
        {
            parentLoginErrorMessageLabel.text = text;
        }
        if (childLoginErrorMessageLabel != null)
        {
            childLoginErrorMessageLabel.text = text;
        }
        if (childRegisterErrorMessageLabel != null)
        {
            childRegisterErrorMessageLabel.text = text;
        }
    }

    private void RegisterUser(bool child)
    {
        if (!Validator.IsValidEmail(usernameValue))
        {
            SetErrorMessages("Invalid MailAddress");
            return;
        }
        else if (!Validator.IsValidPassword(passwordValue))
        {
            if (passwordValue.Length < 10)
            {
                SetErrorMessages("Password must be 10+ characters");
                return;
            }
            else
            {
                SetErrorMessages("Password must have 1 lowercase, 1 uppercase, 1 number and 1 special character.");
                return;
            }
        }
        else if ((parentRegisterSecondPasswordField != null || secondPasswordValue != "") && passwordValue != secondPasswordValue)
        {
            SetErrorMessages("The 2 Passwords are not the same.");
            return;
        }
        string json = JsonConvert.SerializeObject(new { email = usernameValue, password = passwordValue }, Formatting.Indented);
        Debug.Log(json);
        StartCoroutine(apiConnecter.SendRequest("account/register", HttpMethod.POST, false, (string response, string error) =>
        {
            SetTextColor("#FFFFFF", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel, childRegisterErrorMessageLabel);
            SetErrorMessages("Connecting...");
            if (error == null)
            {
                if (child)
                {
                    if (ChildRegisterLoadingPanel != null)
                    {
                        ChildRegisterLoadingPanel.SetActive(true);

                        // login api request
                        // save the bearer token etc
                        // get child user id
                        // forget the child login data
                        // use parent data to link the 2 accounts.
                    }
                }
                else
                {
                    Debug.Log("Response: " + response);
                    SetTextColor("#FFFFFF", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel, childRegisterErrorMessageLabel);
                    SetErrorMessages("Account Created! You are now able to login!");
                    parentRegisterPasswordField.Select();
                    parentRegisterPasswordField.text = "";
                    passwordValue = "";
                    if (parentRegisterSecondPasswordField != null)
                    {
                        parentRegisterSecondPasswordField.Select();
                        parentRegisterSecondPasswordField.text = "";
                        secondPasswordValue = "";
                    }
                }
            }
            else
            {
                SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel, childRegisterErrorMessageLabel);
                SetErrorMessages("Username already taken.");
                Debug.LogError(error);
            }
        },
        json, false));
    }

    private void LoginUser(bool isKind)
    {
        SetTextColor("#FFFFFF", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel, childRegisterErrorMessageLabel);
        SetErrorMessages("Connecting...");
        string json = JsonConvert.SerializeObject(new { email = usernameValue, password = passwordValue }, Formatting.Indented);
        Debug.Log(json);
        StartCoroutine(apiConnecter.SendRequest("account/login", HttpMethod.POST, false, (string response, string error) =>
        {
            if (error == null)
            {
                SetErrorMessages("");
                Debug.Log("Response: " + response);
                if (!isKind)
                {
                    SceneManager.LoadScene(ParentlSceneAfterLogin);
                }
                else
                {
                    SceneManager.LoadScene(ChildSceneAfterLogin);
                }
                LoginResponse decodedResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                LoginSaveFile values = new LoginSaveFile(decodedResponse, isKind);
                MainManager.Instance.SetLoginCredentials(values);
                response = JsonConvert.SerializeObject(values);
                System.IO.File.WriteAllText(MainManager.Instance.LoginDataSaveLocation, response);
            }
            else
            {
                SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel, childRegisterErrorMessageLabel);
                SetErrorMessages("Invalid username & password combination.");
            }
        },
        json, false));
    }

    public void EmptyLoginFormFields()
    {
        parentRegisterPasswordField.Select();
        parentRegisterPasswordField.text = "";
        passwordValue = "";
        parentRegisterUsernameField.Select();
        parentRegisterUsernameField.text = "";
        usernameValue = "";
        if (parentRegisterSecondPasswordField != null)
        {
            parentRegisterSecondPasswordField.Select();
            parentRegisterSecondPasswordField.text = "";
            secondPasswordValue = "";
        }
        childLoginPasswordField.Select();
        childLoginPasswordField.text = "";
        parentLoginUsernameField.Select();
        parentLoginUsernameField.text = "";
        parentLoginPasswordField.Select();
        parentLoginPasswordField.text = "";
        childLoginUsernameField.Select();
        childLoginUsernameField.text = "";
        SetErrorMessages("");
        SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel, childRegisterErrorMessageLabel);
    }

    public void ClickButton(string registerOrLogin)
    {
        SetErrorMessages("");
        SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel, childRegisterErrorMessageLabel);
        if (registerOrLogin == "ParentRegister")
        {
            RegisterUser(false);
        }
        else if (registerOrLogin == "ParentLogin")
        {
            LoginUser(false);
        }
        else if (registerOrLogin == "ChildLogin")
        {
            LoginUser(true);
        }
        else if (registerOrLogin == "ChildRegister")
        {
            RegisterUser(true);
        }
        else
        {
            Debug.LogError($"'{registerOrLogin}' is not a valid formButtonId, use 'ParentRegister', 'ParentLogin', 'ChildRegister' or 'ChildLogin' instead.");
        }
    }

    public void SetPasswordValue(string value)
    {
        passwordValue = value;
    }

    public void SetSecondPasswordValue(string value)
    {
        secondPasswordValue = value;
    }

    public void SetTextColor(string colorText, TextMeshProUGUI element, TextMeshProUGUI element2, TextMeshProUGUI element3, TextMeshProUGUI element4)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorText, out color))
        {
            if (element != null)
            {
                element.color = color;
            }
            if (element2 != null)
            {
                element2.color = color;
            }
            if (element3 != null)
            {
                element3.color = color;
            }
            if (element4 != null)
            {
                element4.color = color;
            }
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
