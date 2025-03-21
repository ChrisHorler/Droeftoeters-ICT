using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum HttpMethod
{
    POST,
    PUT,
    GET,
    DELETE
}

public class LoginResponse
{
    public string tokenType { get; set; }
    public string accessToken { get; set; }
    public int expiresIn { get; set; }
    public string refreshToken { get; set; }
}
public class ApiConnecter : MonoBehaviour
{
    public string baseUrl = "";
    public string defaultLoginScene = "TestFunctionalLogin";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleResponse(string response, string error)
    {
        if (error == null)
        {
            Debug.Log("Response: " + response);
        }
        else
        {
            Debug.LogError("Error: " + error);
        }
    }

    /// <summary>
    /// The method that connects the frontend with the backend.
    /// </summary>
    /// <param name="path">The path that you put behind the baseurl of the api.</param>
    /// <param name="protocol">POST GET PUT or DELETE</param>
    /// <param name="authorized">wether you need to be logged in for this endpoint or not.</param>
    /// <param name="callback">the method that will get called with the result of the api request.</param>
    /// <param name="body">the body that you provide for an PUT and POST request. JSON string.</param>
    /// <param name="autoLogin">whether we send the user to the login page if we get an unauthorized error. If enabled it will try to auto login the user again.</param>
    /// <returns></returns>
    public IEnumerator SendRequest(string path, HttpMethod protocol, bool authorized, Action<string, string> callback, string body = "", bool autoLogin = true)
    {
        string url = $"{baseUrl}/{path}";
        if (MainManager.Instance.LoginResponse == null && authorized)
        {
            callback?.Invoke(null, "Not logged in");
        }
        else
        {
            switch (protocol)
            {
                case HttpMethod.POST:
                    using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
                    {
                        byte[] jsonToSend = Encoding.UTF8.GetBytes(body);
                        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                        request.downloadHandler = new DownloadHandlerBuffer();
                        request.SetRequestHeader("Content-Type", "application/json");
                        yield return ManageRequest(request, callback, authorized, autoLogin);
                    }
                    break;
                case HttpMethod.PUT:
                    using (UnityWebRequest request = UnityWebRequest.Put(url, body))
                    {
                        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                        request.downloadHandler = new DownloadHandlerBuffer();
                        request.SetRequestHeader("Content-Type", "application/json");
                    }
                    break;
                case HttpMethod.GET:
                    using (UnityWebRequest request = UnityWebRequest.Get(url))
                    {
                        yield return ManageRequest(request, callback, authorized, autoLogin);
                    }
                    break;
                case HttpMethod.DELETE:
                    break;
                default:
                    using (UnityWebRequest request = UnityWebRequest.Get(url))
                    {
                        yield return ManageRequest(request, callback, authorized, autoLogin);
                    }
                    break;
            }
        }
    }

    private IEnumerator ManageRequest(UnityWebRequest request, Action<string, string> callback, bool authorized, bool autoLogin)
    {
        if (authorized)
        {
            request.SetRequestHeader("Authorization", $"Bearer {MainManager.Instance.LoginResponse.accessToken}");
        }
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler == null)
            {
                callback?.Invoke("", null);
            }
            else
            {
                callback?.Invoke(request.downloadHandler.text, null);
            }
        }
        else
        {
            if (authorized)
            {
                if (!HandleLoginError(request.downloadHandler.text, GetErrorRequest(request, callback), true))
                {
                    Debug.LogError(GetErrorRequest(request, callback));
                    // nothing to do here :3
                } else
                {
                    Debug.LogError(GetErrorRequest(request, callback));
                    callback?.Invoke(null, GetErrorRequest(request, callback));
                }
            } else
            {
                Debug.LogError(GetErrorRequest(request, callback));
                callback?.Invoke(null, GetErrorRequest(request, callback));
            }
            
        }
    }

    public string GetErrorRequest(UnityWebRequest request, Action<string, string> callback)
    {
        if (request.downloadHandler != null && request.downloadHandler.text != null)
        {
            try
            {
                var jsonObject = JObject.Parse(request.downloadHandler.text);
                if (jsonObject["errors"] != null)
                {
                    var firstError = jsonObject["errors"]
                    .First
                    .First[0]
                    .ToString();

                    return firstError;
                }
                else
                {
                    return request.downloadHandler.text;
                }
            }
            catch
            {
                if (request.downloadHandler.text != "")
                {
                    return request.downloadHandler.text;
                } else
                {
                    return request.error;
                }
            }
        }
        else
        {
            return request.error;
        }
    }

    private bool HandleLoginError(string response, string error, bool autoLogin)
    {
        if (error == "HTTP/1.1 401 Unauthorized" || error == "Not logged in")
        {
            Debug.LogWarning("Login Session Illegal/Expired");
            if (autoLogin && error == "HTTP/1.1 401 Unauthorized" && MainManager.Instance.LoginResponse != null)
            {
                Debug.Log("Trying to refresh token");
                StartCoroutine(SendRequest("/account/refresh", HttpMethod.POST, true, (string response, string error) =>
                {
                    if (error == null)
                    {
                        Debug.Log($"Trying to use new token: {response}");
                        LoginResponse decodedResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                        MainManager.Instance.SetLoginCredentials(decodedResponse);
                        System.IO.File.WriteAllText(MainManager.Instance.LoginDataSaveLocation, response);
                        SceneManager.LoadScene(defaultLoginScene);
                    }
                    else
                    {
                        Debug.LogError($"Not new sessiontoken: {error}");
                        SceneManager.LoadScene(defaultLoginScene);
                    }
                },
                JsonConvert.SerializeObject(new { refreshToken = MainManager.Instance.LoginResponse.refreshToken }), false));
                return true;
            }
            else
            {
                SceneManager.LoadScene(defaultLoginScene);
                return true;
            }

        }
        return false;
    }
}
