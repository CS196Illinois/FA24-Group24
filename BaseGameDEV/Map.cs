using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using minigame;



namespace MapEnd
{   
    class Map
    {
        public class Room {   //Model class that carries the information containing in each room / need to change access level tho
            public string Description {get; set;}
            public int[] Adjacent {get; set;}
        }
 
        static string filepath = "map.json"; //Address 
        public Dictionary<int, string> mapInfo = new Dictionary<int, string> // Current possible room state
        {
            [1] = "Nothing to be found here",
            [2] = "MathQuestion",
            [3] = "Tell me your name", 
            [4] = "A giant statue",
            [5] = "Swift-Footed Achilles",
            [6] = "DiceRoll",
            [7] = "Hector of Troy",
            [8] = "Rhesus of Thrace",
            [9] = "Memnon, Prince of Ethiopia",
            [10] = "I took a pill in Ibiza",
            [11] = "Rocket Man"
        };
        
        public Dictionary<string, Room> Rooms = new Dictionary<string, Room>(); //The local map storage while program is running.
        public void LoadMap() {  //Load map in json.map into Rooms...
            string e = File.ReadAllText(filepath);
            Rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(e);
        }

        /*
        JSON Template: 
            {
            "Room0" :    
            {
                "Description" : "Nothing is here",
                "Adjacent" : [1, 0, 1, 2]
            },
            "Room1" : 
            {
                "Description" : "Nothing is here",
                "Adjacent" : [1, 2, 2, 0]
            }, 
            "Room2" :
            {
                "Description" : "Everything is here",
                "Adjacent" : [1, 0, 1, 2]
            }
        }

        */

        //2D Array Maybe; make euclidean and playable
        public string GenerateMap() {
            File.WriteAllText(filepath, ""); //Clear the file
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            Random random = new Random(); //Randomization
            for (int i = 0; i < 25; i++) {    
                //Add 25 random rooms, each with a random adjacent array leading to non-euclidean navigating
                temp.Add($"Room{i}", new Room(){Description = mapInfo[random.Next(1, 12)], Adjacent = [random.Next(0, 25), random.Next(0, 25), random.Next(0, 25), random.Next(0, 25)]});
            }  
            File.WriteAllText(filepath, JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true })); //Convert to Json and add
            LoadMap(); //Reload the map
            return "Completed";
        }  
        
        //Constructor Check for map.json file
        public Map() {
            if (File.Exists(filepath)) {
                Console.WriteLine("Map File Found");
                LoadMap();//Load the Map Here
            } else {
                throw new Exception("No Map Found");
            }         
        }     
    }

    class MapAction {
        
        Map current = new Map();
        
        //Gets the description of the room that the player is in, the RMNumber pointer however is in class player
        public string getDescription(int RMNumber) { 
            return current.Rooms[$"Room{RMNumber}"].Description;
        }  

        //Function that returns the RmNumber according to the adjacent integer array in each room and user command. 
        public int getNext(int RMNumber, string direction) {
            if (direction == "UP") {
                return current.Rooms[$"Room{RMNumber}"].Adjacent[0];
            } else if (direction == "DOWN") {
                return current.Rooms[$"Room{RMNumber}"].Adjacent[1];
            } else if (direction == "LEFT") {
                return current.Rooms[$"Room{RMNumber}"].Adjacent[2];
            } else if (direction == "RIGHT")
                return current.Rooms[$"Room{RMNumber}"].Adjacent[3];
            return RMNumber;
        }

        //Call the map level function generatemap
        public void Generate() {
            current.GenerateMap();
        }  
    }
}
