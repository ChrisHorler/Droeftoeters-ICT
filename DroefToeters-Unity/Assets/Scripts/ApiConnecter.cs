using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    public IEnumerator SendGetRequest(string path, Action<string, string> callback)
    {
        Debug.Log("Sent GET");
        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                callback?.Invoke(request.downloadHandler.text, null);
            else
                callback?.Invoke(null, request.error);
        }
    }

    public IEnumerator SendAuthGetRequest(string path, Action<string, string> callback)
    {
        Debug.Log("Sent Auth GET");
        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            if (MainManager.Instance.LoginResponse == null)
            {
                callback?.Invoke(null, "Not logged in");
            } else
            {
                request.SetRequestHeader("Authorization", $"Bearer {MainManager.Instance.LoginResponse.accessToken}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback?.Invoke(request.downloadHandler.text, null);
                }
                else
                {
                    Debug.LogError(JsonConvert.SerializeObject(request));
                    callback?.Invoke(null, request.error);
                }
            }
        }
    }

    public IEnumerator SendAuthDeleteRequest(string path, Action<string, string> callback)
    {
        Debug.Log("Sent Auth DELETE");
        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            if (MainManager.Instance.LoginResponse == null)
            {
                callback?.Invoke(null, "Not logged in");
            }
            else
            {
                request.SetRequestHeader("Authorization", $"Bearer {MainManager.Instance.LoginResponse.accessToken}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    if (request.downloadHandler == null)
                    {
                        callback?.Invoke("", null);
                    } else
                    {
                        callback?.Invoke(request.downloadHandler.text, null);
                    }
                    
                }
                else
                {
                    Debug.LogError(JsonConvert.SerializeObject(request.error));
                    callback?.Invoke(null, request.error);
                }
            }
        }
    }


    public IEnumerator SendPostRequest(string jsonData, string path, Action<string, string> callback)
    {
        Debug.Log("Sent POST");
        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                callback?.Invoke(request.downloadHandler.text, null);
            else
                callback?.Invoke(null, request.error);
        }
    }

    public IEnumerator SendAuthPutRequest(string jsonData, string path, Action<string, string> callback)
    {
        Debug.Log("Sent Auth PUT");
        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            if (MainManager.Instance.LoginResponse == null)
            {
                callback?.Invoke(null, "Not logged in");
            }
            else
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Bearer {MainManager.Instance.LoginResponse.accessToken}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback?.Invoke(request.downloadHandler.text, null);
                }
                else
                {
                    Debug.LogError(JsonConvert.SerializeObject(request.error));
                    callback?.Invoke(null, request.error);
                }
            }
        }
    }


    public bool HandleLoginError(string response, string error, bool autoLogin, CanvasGroup loadingScreen = null, CanvasGroup contentPanel = null)
    {
        if (error == "HTTP/1.1 401 Unauthorized" || error == "Not logged in")
        {
            Debug.LogWarning("Login Session Illegal/Expired");
            if (autoLogin && error == "HTTP/1.1 401 Unauthorized" && MainManager.Instance.LoginResponse != null)
            {
                if (loadingScreen != null && contentPanel != null)
                {
                    loadingScreen.alpha = 1f;
                    contentPanel.alpha = 0f;
                    contentPanel.interactable = false;
                    loadingScreen.interactable = true;
                    if (loadingScreen != null && loadingScreen.transform.childCount >= 2)
                    {
                        // Access the second child and get the TextMeshProUGUI component
                        TextMeshProUGUI LoadingScreenTooltipLabel = loadingScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                        LoadingScreenTooltipLabel.text = "Trying to log back in automatically...";
                    }
                }
                Debug.Log("Trying to refresh token");
                StartCoroutine(SendAuthPostRequest(JsonConvert.SerializeObject(new { refreshToken = MainManager.Instance.LoginResponse.refreshToken }),"/account/refresh", 
                    (string response, string error) =>
                {
                    if (error == null)
                    {
                        Debug.Log($"Trying to use new token: {response}");
                        LoginResponse decodedResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                        MainManager.Instance.SetLoginCredentials(decodedResponse);
                        System.IO.File.WriteAllText("UserSettings/playerLogin.json", response);
                        SceneManager.LoadScene("LoginScene");
                    } else
                    {
                        Debug.LogError($"Not new sessiontoken: {error}");
                        SceneManager.LoadScene("LoginScene");
                    }
                }));
                return true;
            } 
            else
            {
                SceneManager.LoadScene("LoginScene");
                return true;
            }
            
        }
        return false;
    }

    public IEnumerator SendAuthPostRequest(string jsonData, string path, Action<string, string> callback)
    {
        Debug.Log("Sent Auth POST");
        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            if (MainManager.Instance.LoginResponse == null)
            {
                callback?.Invoke(null, "Not logged in");
            }
            else
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                // Add Authorization header
                request.SetRequestHeader("Authorization", $"Bearer {MainManager.Instance.LoginResponse.accessToken}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback?.Invoke(request.downloadHandler.text, null);
                }
                else
                {
                    Debug.LogError(JsonConvert.SerializeObject(request));
                    
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

                                callback?.Invoke(null, firstError);
                            } else
                            {
                                callback?.Invoke(null, request.downloadHandler.text);
                            }

                        } catch
                        {
                            callback?.Invoke(null, request.downloadHandler.text);
                        }
                    } else
                    {
                        callback?.Invoke(null, request.error);
                    }
                }
            }
        }
    }
}
