using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class ProcedureManager : MonoBehaviour
{
    public TextMeshProUGUI OutputText;
    private ApiConnecter _apiConnecter;
    void Start()
    {
        _apiConnecter = FindFirstObjectByType<ApiConnecter>();
    }


    void Update()
    {
        
    }

    public void GetProcedures()
    {
        Debug.Log("requesting procedures");
        StartCoroutine(_apiConnecter.SendRequest("api/procedure/all", HttpMethod.GET, false, (response, error) =>
        {
            Debug.Log("received procedures");
            if (error == null)
            {
                var procedures = JsonConvert.DeserializeObject<List<Procedure>>(response);

                OutputText.text = $"{procedures[0].Title}";
            }
            else
            {
                Debug.LogError(error);
                OutputText.text = "error";
            }
        }));
    }
}
