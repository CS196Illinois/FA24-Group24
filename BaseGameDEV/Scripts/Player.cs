using System;
using System.IO;
using System.Text.Json;
using System.ComponentModel;
using MapEnd;
using minigame;
using System.Text.Json.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace PlayerEnd
{
    class Player
    {        
        public class PlayerModel       
        {
            public string? Name {get; set;}
            public Dictionary<string, int>? Stats {get; set;}
            public int RoomNumber {get; set;}
        }
        
        
        //This currently represents the player's current position as a room number, 
        //the entire room dictionary is stored in MapEnd, and accessed via calling mapaction methods and inputting said rmnumber to evaluate
        string filepath = "Saves/characters.json";
        public string? name;
        public Dictionary<string, int>? stats;
        public int RMNumber;
        public void LoadPlayer() {
            string e = File.ReadAllText(filepath);
            PlayerModel temp = JsonSerializer.Deserialize<PlayerModel>(e);
            name = temp.Name;
            stats = temp.Stats;
            RMNumber = temp.RoomNumber;
        }

        public void SavePlayer() {
            File.WriteAllText(filepath, JsonSerializer.Serialize(new PlayerModel(){Name=name, Stats=stats, RoomNumber=RMNumber}, new JsonSerializerOptions { WriteIndented = true }));
        }


        // Gets the player's name or prompts to set it if not set
        public string GetName() {
            return name ?? "Set name first.";
        }

        // Sets the player's name
        public void SetName(string inputName) {
            name = inputName;
        }

        public Player() {
            LoadPlayer();
        }
    }

    class PlayerAction 
    {
        private Player player; 
        private MapAction mymap;
        

        public PlayerAction() {
            player = new Player();
            mymap = new MapAction();
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

        public void __save__() {
            mymap.Save();
            player.SavePlayer();
        }
    }
}
