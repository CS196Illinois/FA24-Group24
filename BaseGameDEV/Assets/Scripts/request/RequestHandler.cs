using GameData;
using Unity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static SceneLoader;
using TMPro;
using UnityEditor.SearchService;

public class Request : MonoBehaviour
{
    
    public static int RequestBoxCount;
    public static string[] RequestNames;
    public static string[] RequestResponse;

    public TMP_InputField input1;
    public TMP_InputField input2;
    public TMP_InputField input3;
    public TMP_InputField[] inputFieldlist;
    public Button submit;
    public Button test;
    void Start() {
        inputFieldlist = new TMP_InputField[] {input1, input2, input3};
        RequestBoxCount = gamedata.RequestBoxCount;
        RequestNames = gamedata.RequestNames;
        ModifyPlaceHolder(inputFieldlist, RequestNames);
        submit.onClick.AddListener(submitf);
        test.onClick.AddListener(testf);
    }

    public void ModifyPlaceHolder(TMP_InputField[] input, string[] itext)
    {
        for (int i = 0; i < RequestBoxCount; i++) {
            TextMeshProUGUI title = input[i].placeholder.GetComponent<TextMeshProUGUI>();
            title.text = "Enter " + itext[i];
        }
        for (int i = RequestBoxCount; i < input.Length; i++) {
            TextMeshProUGUI title = input[i].placeholder.GetComponent<TextMeshProUGUI>();
            title.text = "Disabled";
        }
    } 

    void testf() {
        ModifyPlaceHolder(inputFieldlist, RequestNames);
    }


    void submitf() {
        string[] response = new string[RequestBoxCount];
        for (int i = 0; i < RequestBoxCount; i++) {
            response[i] = inputFieldlist[i].text;   
            inputFieldlist[i].text = "";
        }
        if (response != null) {
            gamedata.RequestResponse = response;
            LoadScene("Main");
        } 
    }
       
}
