using System;
using PlayerEnd;
using GameData;
using System.Reflection;
using VisEnd;
using static SceneLoader;
using System.Linq;
using UnityEngine;
using System.Threading;

namespace LogicEnd
{
    class Logic : MonoBehaviour
    {
        public PlayerAction playerAction {get; set;} 

        //New attempt maybe don't use coroutine cause it doesn't work...
        public SceneLoader requestAttempt;

        public Logic() {
            playerAction = new PlayerAction();    
        }   

        // Returns a list of available commands and their effects
        public string GetHelp(){
            return "Player Commands:\n" +
                   "SETNAME - Set player's name\n" +
                   "GETNAME - Get player's name\n" +
                   "UP - Move up\n" +
                   "DOWN - Move down\n" +
                   "LEFT - Move left\n" +
                   "RIGHT - Move right\n" +
                   "Developer Commands: \n" +
                   "GETRM - Get current room\n" +
                   "EXPLORE - Explore the room\n" +
                   "ECHO - Echo the message\n" +
                   "SUM - Return the sum of the three numbers\n" +
                   "HELLO - Hellooooooooooooooooo!\n" + 
                   "GENERATE - Generate new Map";
        }

        public void UpdateStat() {
            EventTrigger.TriggerUpdateText(playerAction.GETNAME() + "\n" + playerAction.GETALLSTAT() + playerAction.GETPOS(), playerAction.GETALLITEM());
        }

        //Put necessary info to gamedata.cs
        public void Request(string[] requestList, int num) {
            gamedata.RequestBoxCount = num;
            gamedata.RequestNames = requestList;
            gamedata.RequestResponse = null;
        }

        //Retrieve info from gamedata.cs
        public string[] Response() {
            return gamedata.RequestResponse;
        }

    //FUH:KNJLKFnkdsnsl
        public string NoParamProcess(MethodInfo methodInfo) {
            return (string) methodInfo.Invoke(playerAction, null);
        }

        public void ParamProcessRequest(MethodInfo methodInfo, ParameterInfo[] parameters) {
            object[] args = new object[parameters.Length];
            string[] RequestNames = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++) {
                Debug.Log(parameters[i].Name);
                Debug.Log(parameters.Length);
                RequestNames[i] = parameters[i].Name;
            }
            //Pass the necessary data to gamedata.cs so it can be read by the Request script...
            gamedata.RequestBoxCount = parameters.Length;
            gamedata.RequestNames = RequestNames;
            //Async load and wait until completion
            Debug.Log("try");
            requestAttempt.StartRequest(RequestNames, parameters);
            Debug.Log("StartRequest works");
            //Retrieve Data...
            string[] RequestResponse = gamedata.RequestResponse;//How to make it wait???
            Debug.Log(RequestResponse);
        }

        // Processes user commands and invokes corresponding actions
        public string Process(string command) {
            string response;
            command = command.ToUpper().Trim();
            MethodInfo methodInfo = typeof(PlayerAction).GetMethod(command);
            if (methodInfo != null) {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length == 0) {
                    response = NoParamProcess(methodInfo);
                } else {
                    object[] args = new object[parameters.Length];
                    ParamProcessRequest(methodInfo, parameters);
                    string[] RequestResponse = gamedata.RequestResponse;//How to make it wait???
                    Debug.Log(RequestResponse);
                    for (int i = 0; i < parameters.Length; i++) {
                        args[i] = Convert.ChangeType(RequestResponse[i], parameters[i].ParameterType);
                    }
                    response = (string) methodInfo.Invoke(playerAction, args);
                }
            } else {
                return "Method Not Found.";
            }
            //Calls the event trigger to update game text
            UpdateStat();
            return response;
        }

        public Boolean checkWin() {
            return playerAction.GETITEM("Key") >= 5;
        }


    } 
}
