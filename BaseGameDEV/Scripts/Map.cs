using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using minigame;
using System.Drawing;



namespace MapEnd
{   
    class Map
    {
        public class Room {   //Model class that carries the information containing in each room / need to change access level tho
            public string Description {get; set;}
            public bool Completed {get;set;}
            public int[] Adjacent {get; set;}
            public int Num {get; set;}
        }

        static string filepath = "C:\\Users\\zacha\\FA24-Group24-4\\BaseGameDEV\\Saves\\map.json"; //Address 
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

        //Size of rows in a map. Total rooms is just rows^2
        const int rowsize = 5;
        const int numTiles = 25;

        //Used to determine if a room logically connects or not
        int RoomLogic(int origin, String movement) {
            int ans = origin;
            int test;
            //If a room doesn't logically connect, the player will stay in the same room.
            if (movement.Equals("U")) {
                test = origin + rowsize;
                if (test <= rowsize * rowsize && test >= 0) {
                    ans = test;
                }
            } else if (movement.Equals("D")) {
                test = origin - rowsize;
                if (test <= rowsize * rowsize && test >= 0) {
                    ans = test;
                }
                //Left and right stop the rooms from connecting when they're on opposite sides
                //Since it's in a grid pattern, you can't move left on a leftside edge, and can't move right on a rightside edge
            } else if (movement.Equals("L")) {
                test = origin - 1;
                if (origin % rowsize != 0) {
                    ans = test;
                }
            } else if (movement.Equals("R")) {
                test = origin + 1;
                if (origin % rowsize != rowsize - 1) {
                    ans = test;
                }
            }
            return ans;
        }
        //Generates a map with any specified number of tiles, with tiles placed randomly from 0, 0. Keys come in the form "Roomx,y" with x,y being the point of the room.
        public string GenerateNormalMap() {
            File.WriteAllText(filepath, ""); //Clear the file
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            Random random = new Random(); //Randomization
            temp.Add("Room0,0", new Room(){Description = mapInfo[random.Next(1, 3)], Completed = false, Adjacent = [-1, -1, -1, -1], Num = 0}); //Add base room at  0,0
            for (int i = 1; i < numTiles; i++) { //Add the remaining rooms, traveling in a random direction through already generated rooms until an available location is found.
                Boolean isAdded = false;
                Point point = new Point(0, 0);
                while (!isAdded) {
                    int r = (int) (4 * random.NextDouble());
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
                    if (!temp.TryGetValue($"Room{point.X}{point.Y}", out newOutput)) { //Updates adjacent information on both the new room and the rooms it connects to.
                        temp.Add($"Room{point.X}{point.Y}", new Room(){Description = mapInfo[random.Next(1, 3)], Completed = false, Adjacent = [-1, -1, -1, -1], Num = i});
                        if (temp.TryGetValue($"Room{point.X}{point.Y + 1}", out adjacentOutput)) {
                            temp[$"Room{point.X}{point.Y}"].Adjacent[0] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[1] = i;
                        }
                        if (temp.TryGetValue($"Room{point.X}{point.Y - 1}", out adjacentOutput)) {
                            temp[$"Room{point.X}{point.Y}"].Adjacent[1] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[0] = i;
                        }
                        if (temp.TryGetValue($"Room{point.X - 1}{point.Y}", out adjacentOutput)) {
                            temp[$"Room{point.X}{point.Y}"].Adjacent[2] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[3] = i;
                        }
                        if (temp.TryGetValue($"Room{point.X + 1}{point.Y}", out adjacentOutput)) {
                            temp[$"Room{point.X}{point.Y}"].Adjacent[3] = adjacentOutput.Num;
                            adjacentOutput.Adjacent[2] = i;
                        }
                        isAdded = true;
                    }
                }
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
            for (int i = 0; i < rowsize * rowsize; i++) {    
                //Add rowsize^2, each with a random adjacent array leading to non-euclidean navigating
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
        
        //Gets the description of the room that the player is in, the RMPos pointer however is in class player
        public string getDescription(Point RMPos) { 
            return current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Description;
        }  
        //Function that returns the RMPos in the specified direction. Must test if room exists first with canMove or code breaks.
        public Point getNext(Point RMPos, string direction) {
            if (direction == "UP") {
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[0] == -1) {
                    return RMPos;
                } else {
                    return new Point(RMPos.X, RMPos.Y + 1);
                }
            } else if (direction == "DOWN") {
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[1] == -1) {
                    return RMPos;
                } else {
                    return new Point(RMPos.X, RMPos.Y - 1);
                }
            } else if (direction == "LEFT") {
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[2] == -1) {
                    return RMPos;
                } else {
                    return new Point(RMPos.X - 1, RMPos.Y);
                }
            } else if (direction == "RIGHT")
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[3] == -1) {
                    return RMPos;
                } else {
                    return new Point(RMPos.X + 1, RMPos.Y);
                }
            return RMPos;
        }
        public Boolean canMove(Point RMPos, string direction) {
            if (direction == "UP") {
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[0] == -1) {
                    return false;
                } else {
                    return true;
                }
            } else if (direction == "DOWN") {
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[1] == -1) {
                    return false;
                } else {
                    return true;
                }
            } else if (direction == "LEFT") {
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[2] == -1) {
                    return false;
                } else {
                    return true;
                }
            } else if (direction == "RIGHT")
                if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Adjacent[3] == -1) {
                    return false;
                } else {
                    return true;
                }
            return false;
        }
        //Function that check/set if map room is completed
        public Boolean getStatus(Point RMPos) {
            if (current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Completed) {
                return true;
            }
            return false;
        }

        public void setStatusDone(Point RMPos) {
            current.Rooms[$"Room{RMPos.X}{RMPos.Y}"].Completed = true;
            current.SaveMap(); //Autosaves this
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
