using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UIElements.Image;
using Random = System.Random;

public class ProceduresManager : MonoBehaviour
{
    public TextMeshProUGUI ErrorText;
    public Canvas ProcedureButtons;
    private ApiConnecter _apiConnecter;
    void Start()
    {
        _apiConnecter = FindFirstObjectByType<ApiConnecter>();

        GetProcedures();
    }


    void Update()
    {
        
    }

    public void GetProcedures()
    {
        Debug.Log("requesting procedures");
        StartCoroutine(_apiConnecter.SendRequest("api/procedure/all", HttpMethod.GET, true, (response, error) =>
        {
            Debug.Log("received procedures");
            if (error == null)
            {
                var procedures = JsonConvert.DeserializeObject<List<Procedure>>(response);

                for (int i = 1; i < 6; i++)
                {
                    var btn = GameObject.Find("Procedure " + i);
                    try
                    {
                        Debug.Log("Procedure " + i);
                        var procedure = procedures[i - 1];
                        btn.SetActive(true);
                        btn.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = procedure.Title;
                        btn.GetComponent<Button>().onClick.AddListener(
                            delegate { LoadProcedure(procedure.Id); });
                    }
                    catch(Exception e)
                    {
                        btn.SetActive(false);
                        Debug.Log(e.Message);
                    }
                }
            }
            else
            {
                Debug.LogError(error);
            }
        }));
    }

    public void LoadProcedure(string id)
    {
        //Im sorry
        Environment.SetEnvironmentVariable("PROCEDURE_ID", id);
        SceneManager.LoadScene("ProcedureDetails");
    }
}
