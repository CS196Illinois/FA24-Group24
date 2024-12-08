using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SceneLoader;

public class BacktoStartHandle : MonoBehaviour
{
    public Button BackToStart;
    // Start is called before the first frame update
    void Start()
    {
        BackToStart.onClick.AddListener(backtostart);
        Debug.Log("Hi");
    }

    void backtostart() {
        Debug.Log("Hi");
        LoadScene("startScreen");
    }

}
