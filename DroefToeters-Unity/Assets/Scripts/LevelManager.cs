using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static string[] _sceneNames;
    private void Awake()
    {
        _sceneNames = Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
            .Select(i => System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)))
            .ToArray();
    }

    public void LoadLevelOnIndex(int index) {
       if (IsValidSceneIndex(index))
           SceneManager.LoadScene(index);
       else
           Debug.LogError($"Invaid Scene index {index}");
       
    }

    public void LoadNextLevel() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(IsValidSceneIndex(nextSceneIndex))
            SceneManager.LoadScene(nextSceneIndex);
        else
            Debug.Log("No more scenes available");
    }

    public void QuitApp() => Application.Quit();
    public void RestartApp() => SceneManager.LoadScene(0);
    
    private bool IsValidSceneIndex(int index)  => index >= 0 && index < _sceneNames.Length;
}
