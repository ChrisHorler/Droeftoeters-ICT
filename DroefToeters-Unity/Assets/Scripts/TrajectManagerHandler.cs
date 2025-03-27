using Newtonsoft.Json;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectManagerHandler : MonoBehaviour
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
    private ApiConnecter apiConnecter;
    public string defaulSceneAfterLogin = "SampleScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        apiConnecter = FindFirstObjectByType<ApiConnecter>();
        StartCoroutine(DelayedRequest());
    }

    IEnumerator DelayedRequest()
    {
        yield return new WaitForSeconds(1f);
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
                    SceneManager.LoadScene(defaulSceneAfterLogin);
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
                            MainManager.Instance.SetLoginCredentials(decodedResponse);
                            System.IO.File.WriteAllText(MainManager.Instance.LoginDataSaveLocation, response);
                            if (MainManager.Instance.NavigationScene != null && MainManager.Instance.NavigationScene != "")
                            {
                                SceneManager.LoadScene(MainManager.Instance.NavigationScene);
                            }
                            else
                            {
                                SceneManager.LoadScene(defaulSceneAfterLogin);
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
        parentRegisterErrorMessageLabel.text = text;
        parentLoginErrorMessageLabel.text = text;
        childLoginErrorMessageLabel.text = text;
    }

    private void RegisterUser()
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
            SetTextColor("#FFFFFF", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel);
            SetErrorMessages("Connecting...");
            if (error == null)
            {
                Debug.Log("Response: " + response);
                SetTextColor("#FFFFFF", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel);
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
            else
            {
                SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel);
                SetErrorMessages("Username already taken.");
                Debug.LogError(error);
            }
        },
        json, false));
    }

    private void LoginUser()
    {
        SetTextColor("#FFFFFF", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel);
        SetErrorMessages("Connecting...");
        string json = JsonConvert.SerializeObject(new { email = usernameValue, password = passwordValue }, Formatting.Indented);
        Debug.Log(json);
        StartCoroutine(apiConnecter.SendRequest("account/login", HttpMethod.POST, false, (string response, string error) =>
        {
            if (error == null)
            {
                SetErrorMessages("");
                Debug.Log("Response: " + response);
                SceneManager.LoadScene(defaulSceneAfterLogin);
                LoginResponse decodedResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                MainManager.Instance.SetLoginCredentials(decodedResponse);
                System.IO.File.WriteAllText(MainManager.Instance.LoginDataSaveLocation, response);
            }
            else
            {
                SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel);
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
        SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel);
    }

    public void ClickButton(string registerOrLogin)
    {
        SetErrorMessages("");
        SetTextColor("#FF0000", parentRegisterErrorMessageLabel, parentLoginErrorMessageLabel, childLoginErrorMessageLabel);
        if (registerOrLogin == "ParentRegister")
        {
            RegisterUser();
        }
        else if (registerOrLogin == "ParentLogin")
        {
            LoginUser();
        }
        else if (registerOrLogin == "ChildLogin")
        {
            LoginUser();
        }
        else
        {
            Debug.LogError($"'{registerOrLogin}' is not a valid formButtonId, use 'ParentRegister', 'ParentLogin' or 'ChildLogin' instead.");
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

    public void SetTextColor(string colorText, TextMeshProUGUI element, TextMeshProUGUI element2, TextMeshProUGUI element3)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorText, out color))
        {
            element.color = color;
            element2.color = color;
            element3.color = color;
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
