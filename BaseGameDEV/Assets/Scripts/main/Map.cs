using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Drawing;



namespace MapEnd
{   
    public class Map
    {
        public class Room {   //Model class that carries the information containing in each room / need to change access level tho
            public string Description {get; set;}
            public bool Completed {get;set;}
            public int[] Adjacent {get; set;}
            public int Num {get; set;}
        }

        static string filepath = "Assets/Saves/map.json"; //Address 
        public Dictionary<int, string> mapInfo = new Dictionary<int, string> // Current possible room state
        {
            [1] = "Key",
            [2] = "Minigame",
            [3] = "Combat",
            [4] = "Item",
            [5] = "Empty",
            [6] = "Treasure"
        };
        
        public Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        public void LoadMap() {  //Load map in json.map into Rooms...
            string e = File.ReadAllText(filepath);
            rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(e); 
            Debug.Log("Loaded");  
            //Debug.Log(rooms["Room0,0"].ToString());
        }

        /*
        JSON Template: 
        {
        "Room0,0":
            {
            "Description":"Empty",
            "Completed":true,
            "Adjacent":[2,1,6,5],
            "Num":0
            },
        "Room0,-1":
            {
            "Description":"Empty",
            "Completed":true,
            "Adjacent":[0,-1,11,8],
            "Num":1
            },

        */


        const int rowsize = 8;
        //Number of tiles in a map
        const int numTiles = numKeys + numCombats + numItems + numEmpty  + numTreasures + numMinigame;
        //Breakdown of room types
        //Setting insert here...
        const int numKeys = 4;
        const int numMinigame = 13;
        const int numTreasures = 4;
        const int numCombats = 0;
        const int numItems = 10;

        //Must have at least 1 empty room, starting room is always empty
        const int numEmpty = 4;


        public string GenerateNormalMapZach() {
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            List<string> descriptions = generateDescriptions(); //A List of Descriptions for rooms
            System.Random random = new System.Random(); //Randomization
            temp.Add("Room0,0", new Room() {Description = "Empty", Completed = false, Adjacent = new int[] {-1, -1, -1, -1}, Num = 0}); //Add base room at  0,0
            for (int i = 1; i < numTiles; i++) { //Add the remaining rooms, traveling in a random direction through already generated rooms until an available location is found.
                bool isAdded = false;
                string description = descriptions[random.Next(0, descriptions.Count)];
                descriptions.Remove(description);
                Point point = new Point(0, 0);
                while (!isAdded) {
                    int r = random.Next(0, 4);
                    if (r == 0) {
                        point.Y++;
                    } else if (r == 1) {
                        point.Y--;
                    } else if (r == 2) {
                        point.X--;
                    } else {
                        point.X++;
                    }
                    Room newOutput;
                    Room adjacentOutput;
                    if (!temp.TryGetValue($"Room{point.X},{point.Y}", out newOutput)) { //Creates new room when a blank space is found, updating adjacent information on both the new room and the rooms it connects to.
                        temp.Add($"Room{point.X},{point.Y}", new Room(){Description = description, Completed = false, Adjacent = new int[] {-1, -1, -1, -1}, Num = i});
                        if (temp.TryGetValue($"Room{point.X},{point.Y + 1}", out adjacentOutput)) {
                            temp[$"Room{point.X},{point.Y}"].Adjacent[0] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[1] = i;
                        }
                        if (temp.TryGetValue($"Room{point.X},{point.Y - 1}", out adjacentOutput)) {
                            temp[$"Room{point.X},{point.Y}"].Adjacent[1] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[0] = i;
                        }
                        if (temp.TryGetValue($"Room{point.X - 1},{point.Y}", out adjacentOutput)) {
                            temp[$"Room{point.X},{point.Y}"].Adjacent[2] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[3] = i;
                        }
                        if (temp.TryGetValue($"Room{point.X + 1},{point.Y}", out adjacentOutput)) {
                            temp[$"Room{point.X},{point.Y}"].Adjacent[3] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[2] = i;
                        }
                        isAdded = true;
                    }
                }
            }  
            File.WriteAllText(filepath, ""); //Clear the file before
            File.WriteAllText(filepath, JsonConvert.SerializeObject(temp)); //Convert to Json and add
            LoadMap(); //Reload the map
            return "Completed";
        }

        public List<string> generateDescriptions() {
            List<string> descriptions = new List<string>();
            for (int x = 0; x < numKeys; x++) {
                descriptions.Add("Key");
            }
            for (int x = 0; x < numCombats; x++) {
                descriptions.Add("Combat");
            }
            for (int x = 0; x < numItems; x++) {
                descriptions.Add("Item");
            }
            for (int x = 0; x < numMinigame; x++) {
                descriptions.Add("Minigame");
            }
            for (int x = 0; x < numEmpty - 1; x++) {
                descriptions.Add("Empty");
            }
            for (int x = 0; x < numTreasures; x++) {
                descriptions.Add("Treasure");
            }
            return descriptions;
        }

        public void SaveMap() {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(rooms));
            LoadMap(); //Reload the Map
        }
        
        //Constructor Check for map.json file
        public Map() {      
            LoadMap();//Load the Map Here 
            Debug.Log("Map loaded");    
        }     
    }
}