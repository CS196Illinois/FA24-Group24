using UnityEngine.SceneManagement;
using UnityEngine;
using GameData;
using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    //Load the request and pause it... until u are done...
    public void StartRequest(string[] requestNames, ParameterInfo[] parameters) {
        gamedata.RequestBoxCount = parameters.Length;
        gamedata.RequestNames = requestNames;

        //Start the scene loading process
        StartCoroutine(LoadRequestSceneAndRetrieveData("Request", parameters));
    } 

    private IEnumerator LoadRequestSceneAndRetrieveData(string sceneName, ParameterInfo[] parameters) 
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone) {
            yield return null;
        }
        //Wait for player to Exit the scene
        yield return new WaitUntil(() => IsSceneUnloaded("Request"));
        Debug.Log("Successful exit the scene");
        //Retrieve 
        string[] requestResponse = gamedata.RequestResponse;
        Debug.Log(requestResponse);
    }

    private bool IsSceneUnloaded(string sceneName) {
        return !SceneManager.GetSceneByName("Request").isLoaded;
    }
}