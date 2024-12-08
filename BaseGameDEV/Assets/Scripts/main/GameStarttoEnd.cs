using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LogicEnd;
using TMPro;
using static SceneLoader;
using PlayerEnd;
using System.Data;

public class GameStarttoEnd : MonoBehaviour
{
    public GameObject InputUI;
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI shopText;
    public TMP_InputField userInput;
    public Button submitButton;
    public Button evaluateButton;
    public Button buy1;
    public Button buy2;
    public Button buy3;
    public Button atkup;
    public Button defup;
    public Button intup;
    public Button spdup;
    public GameObject UpTrigger;
    public GameObject DownTrigger;
    public GameObject LeftTrigger;
    public GameObject RightTrigger;
    public GameObject Player;
    public GameObject ESCScreen;
    public Button exitButton;
    private Logic Session;

    void Start()
    {
        Session = new Logic();
        displayText.text = "WELCOME, Explore and Collect 5 Keys to Win";
        shopText.text = "";
        Session.UpdateStat();
        ESCScreen.SetActive(false);
        InputUI.SetActive(true);
        submitButton.onClick.AddListener(OnSubmit);
        evaluateButton.onClick.AddListener(Eval);
        exitButton.onClick.AddListener(exitf);
        buy1.onClick.AddListener(b1);
        buy2.onClick.AddListener(b2);
        buy3.onClick.AddListener(b3);
        atkup.onClick.AddListener(Atkup);
        defup.onClick.AddListener(Defup);
        intup.onClick.AddListener(Intup);
        spdup.onClick.AddListener(Spdup);
    }

    //Evaluate when Eval is clicked
    private void Eval() {
        ProcessInput("EVALUATE");
    }

    //Send textbox command to gamelogic
    private void OnSubmit() {
        string input = userInput.text.Trim();
        userInput.text = "";
        ProcessInput(input);
    }

    private void exitf() {
        SceneLoader.LoadScene("StartScreen");
    }

    void ProcessInput(string input) {
        string response = Session.Process(input);
        displayText.text = response;
    }
    
    //All Buy Options...
    void b1() {
        PlayerAction access = Session.playerAction;
        int hp = access.GETSTAT("HP");
        if (access.GETITEM("Coin") < 50) {
            shopText.text = "Not enough gold";
        } else if (hp == 100) {
            shopText.text = "You are at full health"; //Limit health to 100 max.
        } else if (hp + 40 > 100) {
            access.SETSTAT("HP", 100);
            access.ADDITEM("Coin", -50);
            shopText.text = "Purchased";
        } else {
            access.ADDSTAT("HP", 40);
            access.ADDITEM("Coin", -50);
            shopText.text = "Purchased";
        }
        Session.UpdateStat();
    }

    void b2() {
        PlayerAction access = Session.playerAction;
        if (access.GETITEM("Coin") < 200) {
            shopText.text = "Not enough gold";
        } else {
            access.ADDITEM("Key", 1);
            access.ADDITEM("Coin", -200);
            shopText.text = "Purchased";
        }
        Session.UpdateStat();
    }

    void b3(){
        PlayerAction access = Session.playerAction;
        int spd = access.GETSTAT("SPD");
        if (access.GETITEM("Coin") < 30) {
            shopText.text = "Not enough gold";
        } else if (spd >= 30) {
            shopText.text = "You can't go any faster"; 
        } else {
            access.ADDSTAT("SPD", 5);
            access.ADDITEM("Coin", -30);
            shopText.text = "Purchased";
        }
        Session.UpdateStat();
    }


    //All level stat options
    void Atkup() {
        PlayerAction access = Session.playerAction;
        int exp = access.GETSTAT("EXP");
        if (exp < 80) {
            shopText.text = "Not enough exp";
        } else if (access.GETSTAT("ATK") >= 30) {
            shopText.text = "Your atk can't go any higher"; 
        } else {
            access.ADDSTAT("EXP", -80);
            access.ADDSTAT("ATK", 1);
            shopText.text = "Upgraded";
        }
        Session.UpdateStat();
    }
    void Defup() {
        PlayerAction access = Session.playerAction;
        int exp = access.GETSTAT("EXP");
        if (exp < 80) {
            shopText.text = "Not enough exp";
        } else if (access.GETSTAT("DEF") >= 30) {
            shopText.text = "Your def can't go any higher"; 
        } else {
            access.ADDSTAT("EXP", -80);
            access.ADDSTAT("DEF", 1);
            shopText.text = "Upgraded";
        }
        Session.UpdateStat();
    }
    void Intup() {
        PlayerAction access = Session.playerAction;
        int exp = access.GETSTAT("EXP");
        if (exp < 80) {
            shopText.text = "Not enough exp";
        } else if (access.GETSTAT("INT") >= 30) {
            shopText.text = "Your int can't go any higher"; 
        } else {
            access.ADDSTAT("EXP", -80);
            access.ADDSTAT("INT", 1);
            shopText.text = "Upgraded";
        }
        Session.UpdateStat();
    }
    void Spdup() {
        PlayerAction access = Session.playerAction;
        int exp = access.GETSTAT("EXP");
        if (exp < 80) {
            shopText.text = "Not enough exp";
        } else if (access.GETSTAT("SPD") >= 30) {
            shopText.text = "Your spd can't go any higher"; 
        } else {
            access.ADDSTAT("EXP", -80);
            access.ADDSTAT("SPD", 1);
            shopText.text = "Upgraded";
        }
        Session.UpdateStat();
    }


   //Sets player position when WallTrigger hit.
    public void Up() {
        string response = Session.Process("UP");
        displayText.text = response;
        //ResetPlayerNode Position if they gone to a new room sent them to other side otherwise bounce back
        if (response != "No Room This Way!" && response != "Complete this room first!") {
            Player.transform.position = new Vector2 (Player.transform.position.x, 250);
        } else {
            Player.transform.position = new Vector2 (Player.transform.position.x, 960);
        }
    }
    public void Down() {
        string response = Session.Process("DOWN");
        displayText.text = response;
        //ResetPlayerNode Position if they gone to a new room sent them to other side otherwise bounce back
        if (response != "No Room This Way!" && response != "Complete this room first!") {
            Player.transform.position = new Vector2 (Player.transform.position.x, 960);
        } else {
            Player.transform.position = new Vector2 (Player.transform.position.x, 250);
        }
        
    }
    public void Left() {
        string response = Session.Process("LEFT");
        displayText.text = response;
        //ResetPlayerNode Position if they gone to a new room sent them to other side otherwise bounce back
        if (response != "No Room This Way!" && response != "Complete this room first!") {
            Player.transform.position = new Vector2 (1800, Player.transform.position.y);
        } else {
            Player.transform.position = new Vector2 (150, Player.transform.position.y);
        }
        
    }

    public void Right() {
        string response = Session.Process("RIGHT");
        displayText.text = response;
        //ResetPlayerNode Position if they gone to a new room sent them to other side otherwise bounce back
        if (response != "No Room This Way!" && response != "Complete this room first!") {
            Player.transform.position = new Vector2 (150, Player.transform.position.y);
        } else {
            Player.transform.position = new Vector2 (1800, Player.transform.position.y);
        }
    }

    //Frame per Update currently movement
    void Update() {          
        float MovementSpeed =  500 + 10 * Session.playerAction.GETSTAT("SPD");
        if (InputUI.activeSelf) {
            if (Input.GetKey(KeyCode.UpArrow))
                Player.transform.Translate(Vector2.up * MovementSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.DownArrow))
                Player.transform.Translate(Vector2.down * MovementSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftArrow))
                Player.transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.RightArrow))
                Player.transform.Translate(Vector2.right * MovementSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (ESCScreen.activeSelf) {
                ESCScreen.SetActive(false);
                InputUI.SetActive(true);
                shopText.text = "";
                displayText.text = "Collect 5 Keys to Win";
            } else {
                ESCScreen.SetActive(true);
                InputUI.SetActive(false);
                shopText.text = "Welcome to the shop"; //Display when entering the shop/esc screen.
            }
        }
        if (Session.checkWin()) {
            SceneLoader.LoadScene("endscreen");
        }
        if (Session.playerAction.GETSTAT("HP") <= 0) {
            LoadScene("StartScreen");
        }
    }
   
}
