using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SceneLoader;
using PlayerEnd;
using UnityEngine.Analytics;

public class StartMenuHandle : MonoBehaviour
{
    public Button start;
    public Button reset;
    public Button continues;
    public Button quit;
    public GameObject ESCScreen;
    public GameObject startScreen;
    public GameStarttoEnd gameStarttoEnd;
    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(startf);
        continues.onClick.AddListener(continuef);
        reset.onClick.AddListener(resetf);
        quit.onClick.AddListener(quitf);
        ESCScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    void resetf() {
        PlayerAction resetaccess = new PlayerAction();
        resetaccess.reset();
    }

    void continuef() {
        PlayerAction newchanceaccess = new PlayerAction();
        if (newchanceaccess.GETSTAT("HP") <= 0) {  //If you blacked out this is to continue on same map but reset stats
            newchanceaccess.reset();
        } 
        SceneLoader.LoadScene("main");
    }
    void startf() {
        resetf();
        PlayerAction newmapaccess = new PlayerAction();
        newmapaccess.GENERATE(); //New Map
        SceneLoader.LoadScene("main");
    }

    void quitf() {
        Application.Quit();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (ESCScreen.activeSelf) {
                ESCScreen.SetActive(false);
                startScreen.SetActive(true);
            } else {
                ESCScreen.SetActive(true);
                startScreen.SetActive(false);
            }
        }
    }
}
