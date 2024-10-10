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
            public bool Completed {get;set;}
            public int[] Adjacent {get; set;}
        }
 
        static string filepath = "Saves/map.json"; //Address 
        public Dictionary<int, string> mapInfo = new Dictionary<int, string> // Current possible room state
        {
            [1] = "DiceRoll",
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
                "Completed" : true,
                "Adjacent" : [1, 0, 1, 2]
            },
            "Room1" : 
            {
                "Description" : "Nothing is here",
                "Completed" : false,
                "Adjacent" : [1, 2, 2, 0]
            }, 
            "Room2" :
            {
                "Description" : "Everything is here",
                "Completed" : false,
                "Adjacent" : [1, 0, 1, 2]
            }
        }

        */

        //2D Array Maybe; make euclidean and playable


        //Used to determine if a room logically connects or not
        int RoomLogic(int origin, String movement) {
            int ans = origin;
            int test;
            //If a room doesn't logically connect, the player will stay in the same room.
            if (movement.Equals("U")) {
                test = origin + 5;
                if (test <= 24 && test >= 0) {
                    ans = test;
                }
            } else if (movement.Equals("D")) {
                test = origin - 5;
                if (test <= 24 && test >= 0) {
                    ans = test;
                }
                //Left and right stop the rooms from connecting when they're on opposite sides
                //Since it's in a grid pattern, you can't move left from room 5 and can't move right from room 4
            } else if (movement.Equals("L")) {
                test = origin - 1;
                if (origin % 5 != 0) {
                    ans = test;
                }
            } else if (movement.Equals("R")) {
                test = origin + 1;
                if (origin % 5 != 4) {
                    ans = test;
                }
            }
            return ans;
        }
        public string GenerateNormalMap() {
            File.WriteAllText(filepath, ""); //Clear the file
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            Random random = new Random(); //Randomization
            for (int i = 0; i < 25; i++) {    
                //Add 25 random encounter rooms in a set order
                temp.Add($"Room{i}", new Room(){Description = mapInfo[random.Next(1, 3)], Completed = false, Adjacent = [RoomLogic(i, "U"), RoomLogic(i, "D"), RoomLogic(i, "L"), RoomLogic(i, "R")]});
            }  
            File.WriteAllText(filepath, JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true })); //Convert to Json and add
            LoadMap(); //Reload the map
            return "Completed";
        }  

        public string GenerateMap() {
            File.WriteAllText(filepath, ""); //Clear the file
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            Random random = new Random(); //Randomization
            for (int i = 0; i < 25; i++) {    
                //Add 25 random rooms, each with a random adjacent array leading to non-euclidean navigating
                temp.Add($"Room{i}", new Room(){Description = mapInfo[random.Next(1, 3)], Completed = false, Adjacent = [random.Next(0, 25), random.Next(0, 25), random.Next(0, 25), random.Next(0, 25)]});
            }  
            File.WriteAllText(filepath, JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true })); //Convert to Json and add
            LoadMap(); //Reload the map
            return "Completed";
        }  

        public void SaveMap() {
            File.WriteAllText(filepath, JsonSerializer.Serialize(Rooms, new JsonSerializerOptions { WriteIndented = true }));
            LoadMap(); //Reload the Map
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
        public void GenerateNormal() {
            current.GenerateNormalMap();
        }  
        public void Save() {
            current.SaveMap();
        }
    }
}
