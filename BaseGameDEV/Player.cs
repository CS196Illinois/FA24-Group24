using System;
using System.ComponentModel;
using MapEnd;
using minigame;

namespace PlayerEnd
{
    class Player
    {
        public string name = ""; 
        public int RMNumber; 
        //This currently represents the player's current position as a room number, 
        //the entire room dictionary is stored in MapEnd, and accessed via calling mapaction methods and inputting said rmnumber to evaluate


        // Gets the player's name or prompts to set it if not set
        public string GetName() {
            if (name.Equals("")) {
                return "Set name first.";
            } else {
                return name;
            }
        }

        // Sets the player's name
        public void SetName(string inputName) {
            name = inputName;
        }
    }

    class PlayerAction 
    {
        private Player player; 
        private MapAction mymap;
        

        public PlayerAction(Player inputPlayer) {
            player = inputPlayer;
            mymap = new MapAction();
            player.RMNumber = 0;
        }

        public void SETNAME(string name) {
            player.SetName(name);
        }

        public string GETNAME() {
            return player.GetName();
        }

        public void UP() {
            player.RMNumber = mymap.getNext(player.RMNumber, "UP");
        }
        public void DOWN() {
            player.RMNumber = mymap.getNext(player.RMNumber, "DOWN");
        }
        public void LEFT() {
            player.RMNumber = mymap.getNext(player.RMNumber, "LEFT");
        }
        public void RIGHT() {
            player.RMNumber = mymap.getNext(player.RMNumber, "RIGHT");
        }

        public string GETRM() {
            return $"Room {player.RMNumber}";
        }  

        public string EXPLORE() {
            return mymap.getDescription(player.RMNumber);       
        }

        public string ECHO(string subject) {
            return subject;
        }

        public string SUM(int firstNum, int secondNum, int thirdNum) {
            return $"The sum is: {firstNum + secondNum + thirdNum}";
        }

        public string HELLO() {
            return "Hello";
        }

        public string EVALUATE() {
            minigameWrapper challenge = new minigameWrapper(mymap.getDescription(player.RMNumber), player.RMNumber);
            if (challenge.checkResult()) {
                return "Good job, you may continue.";
            } else {
                player.RMNumber = 0;
                return "Back to the beginning";
            }
        }

        //GENERATE --> MapAction Generate --> Map GenerateMap
        public string GENERATE() {
            mymap.Generate();
            return "New Map Generated";
        }
    }
}
