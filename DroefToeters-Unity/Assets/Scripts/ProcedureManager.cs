using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ProcedureManager : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    [FormerlySerializedAs("Items")] public TextMeshProUGUI ProcedureItems;
    private ApiConnecter _apiConnecter;

    void Start()
    {
        _apiConnecter = FindFirstObjectByType<ApiConnecter>();
        GetProcedures();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetProcedures()
    {
        Debug.Log("requesting procedure");
        
        //I SAID IM SORRY
        var id = Environment.GetEnvironmentVariable("PROCEDURE_ID");
        Environment.SetEnvironmentVariable("PROCEDURE_ID", null);
        
        StartCoroutine(_apiConnecter.SendRequest($"api/procedure/{id}", HttpMethod.GET, true, (response, error) =>
        {
            Debug.Log("received procedure");
            if (error == null)
            {
                var procedure = JsonConvert.DeserializeObject<Procedure>(response);
                Title.text = procedure.Title;
                Description.text = procedure.Description;

                
                //sort procedure list
                var sortedProcedureItems = new List<ProcedureItem>();
                string previousId = null;
                for (int i = 0; i < procedure.ProcedureItems.Count; i++)
                {
                    sortedProcedureItems.Add(
                        procedure.ProcedureItems.First(x => x.PreviousItemId == previousId));
                    previousId = sortedProcedureItems[i].Id;
                }

                int iter = 1;
                ProcedureItems.text = "";
                foreach (var item in sortedProcedureItems)
                {
                    ProcedureItems.text += @$"{iter}: {item.Title}
    - {item.Description}\n\n";
                    iter++;
                }


            }
            else
            {
                Debug.LogError(error);
            }
        }));
    }
    
    public void BackToSelection()
    {
        SceneManager.LoadScene("ProcedureSelection");
    }
}
