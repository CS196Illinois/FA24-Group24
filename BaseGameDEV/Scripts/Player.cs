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
        public class PlayerModel //Data Model that contains all character-related information and json format 
        {
            public string? Name {get; set;}
            public Dictionary<string, int>? Stats {get; set;}
            public int RoomNumber {get; set;}
        }
    
        //Loading in Player
        string filepath = "Saves/characters.json";
        public string? name;
        public Dictionary<string, int>? stats;
        public int RMNumber;
        public void LoadPlayer() {
            string e = File.ReadAllText(filepath);
            PlayerModel? temp = JsonSerializer.Deserialize<PlayerModel>(e);
            if (temp == null) {
                throw new Exception("Error loading in player info.");
            }
            name = temp.Name;
            stats = temp.Stats;
            RMNumber = temp.RoomNumber;
        }

        //Save PlayerInfor into characters.json
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

        //Constructor that automatically load player when new session initiates
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


        //End-User Actions
        public void SETNAME(string name) {
            player.SetName(name);
        }

        public string GETNAME() {
            return player.GetName();
        }

        public void UP() {
            player.RMNumber = mymap.getNext(player.RMNumber, "UP");
            Console.WriteLine(GETRM());
            Console.WriteLine(EXPLORE());
            Console.WriteLine(EVALUATE());
        }
        public void DOWN() {
            player.RMNumber = mymap.getNext(player.RMNumber, "DOWN");
            Console.WriteLine(GETRM());
            Console.WriteLine(EXPLORE());
            Console.WriteLine(EVALUATE());
        }
        public void LEFT() {
            player.RMNumber = mymap.getNext(player.RMNumber, "LEFT");
            Console.WriteLine(GETRM());
            Console.WriteLine(EXPLORE());
            Console.WriteLine(EVALUATE());
        }
        public void RIGHT() {
            player.RMNumber = mymap.getNext(player.RMNumber, "RIGHT");
            Console.WriteLine(GETRM());
            Console.WriteLine(EXPLORE());
            Console.WriteLine(EVALUATE());
        }


        //Backend Actions

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

        //GENERATEN --> MapAction GenerateN --> Map GenerateNormalMap
        public string GENERATEN() {
            mymap.GenerateN();
            return "New Map Generated";
        }

        //UI save command --> __save__ which saves both map via MapAction Save() and player via Player SavePlayer()
        //Not callable from UI process() since not capitalized 
        public void __save__() {
            mymap.Save();
            player.SavePlayer();
        }
    }
}
