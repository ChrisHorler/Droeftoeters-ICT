using UnityEngine;
using UnityEngine.SceneManagement;

public class DashManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SwitchScene(string scene)
    {
        Debug.Log("Loading scene: "+ scene);
        SceneManager.LoadScene(scene);
    }
}
