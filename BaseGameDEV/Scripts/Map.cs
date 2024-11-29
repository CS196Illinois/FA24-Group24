using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using minigame;
using System.Drawing;
using System.Reflection.Metadata;
using System.Linq.Expressions;



namespace MapEnd
{
    class Map
    {
        public class Room
        {   //Model class that carries the information containing in each room / need to change access level tho
            public string Description { get; set; }
            public bool Completed { get; set; }
            public int Num { get; set; }
        }

        static string filepath = "/Users/natedee/Documents/UIUC/F24/CS124H/FA24-Group24-6/BaseGameDEV/Saves/map.json"; //Address 
        public Dictionary<int, string> mapInfo = new Dictionary<int, string> // Current possible room state
        {
            [1] = "Key",
            [2] = "Minigame",
            [3] = "Combat",
            [4] = "Item",
            [5] = "Shop",
            [6] = "Empty"
        };

        public Dictionary<string, Room> Rooms = new Dictionary<string, Room>(); //The local map storage while program is running.
        public void LoadMap()
        {  //Load map in json.map into Rooms...
            string e = File.ReadAllText(filepath);
            Rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(e);
        }

        /*
        JSON Template: 
        {
            "Room0,0": {
            "Description": "Shop",
            "Completed": false,
            "Num": 0
        },
        "Room0,1": {
            "Description": "Minigame",
            "Completed": false,
            "Num": 1
        },
        "Room0,-1": {
            "Description": "Empty",
            "Completed": false,
            "Num": 2
        }
        }
        */


        //Number of tiles in a map
        const int numTiles = numKeys + numMinigames + numCombats + numItems + numEmpty;
        //Breakdown of room types
        const int numKeys = 3;
        const int numMinigames = 5;
        const int numCombats = 2;
        const int numItems = 5;
        //Must have at least 1 empty room, starting room is always empty
        const int numEmpty = 10;
        const int numShop = 3;

        public List<String> generateDescriptions()
        {
            List<String> descriptions = new List<string>();
            for (int x = 0; x < numKeys; x++)
            {
                descriptions.Add("Key");
            }
            for (int x = 0; x < numMinigames; x++)
            {
                descriptions.Add("Minigame");
            }
            for (int x = 0; x < numCombats; x++)
            {
                descriptions.Add("Combat");
            }
            for (int x = 0; x < numItems; x++)
            {
                descriptions.Add("Item");
            }
            for (int x = 0; x < numShop; x++) {
                descriptions.Add("Shop");
            }
            for (int x = 0; x < numEmpty - 1; x++)
            {
                descriptions.Add("Empty");
            }
            return descriptions;
        }
        //Generates a map with any specified number of tiles, with tiles placed randomly from 0, 0. Keys come in the form "Roomx,y" with x,y being the point of the room.
        public string GenerateNormalMap()
        {
            File.WriteAllText(filepath, ""); //Clear the file
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            List<String> descriptions = generateDescriptions();
            Random random = new Random(); //Randomization
            temp.Add("Room0,0", new Room() { Description = "Empty", Completed = false, Num = 0 }); //Add base room at 0,0
            for (int i = 1; i < numTiles; i++)
            { //Add the remaining rooms, traveling in a random direction through already generated rooms until an available location is found.
                Boolean isAdded = false;
                string description = descriptions[random.Next(0, descriptions.Count)];
                descriptions.Remove(description);
                Point point = new Point(0, 0);
                while (!isAdded)
                {
                    int r = random.Next(0, 4);
                    if (r == 0)
                    {
                        point.Y++;
                    }
                    else if (r == 1)
                    {
                        point.Y--;
                    }
                    else if (r == 2)
                    {
                        point.X--;
                    }
                    else
                    {
                        point.X++;
                    }
                    Room newOutput;


                    Room adjacentOutput;
                    if (!temp.TryGetValue($"Room{point.X},{point.Y}", out newOutput))
                    { //Creates new room when a blank space is found, updating adjacent information on both the new room and the rooms it connects to.
                        temp.Add($"Room{point.X},{point.Y}", new Room() { Description = description, Completed = false, Num = i });
                        isAdded = true;
                    }

                }
            }
            File.WriteAllText(filepath, JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true })); //Convert to Json and add
            LoadMap(); //Reload the map
            return "Completed";
        }

        public void SaveMap()
        {
            File.WriteAllText(filepath, JsonSerializer.Serialize(Rooms, new JsonSerializerOptions { WriteIndented = true }));
            LoadMap(); //Reload the Map
        }

        //Constructor Check for map.json file
        public Map()
        {
            if (File.Exists(filepath))
            {
                Console.WriteLine("Map File Found");
                LoadMap();//Load the Map Here
            }
            else
            {
                throw new Exception("No Map Found");
            }
        }
    }

    class MapAction
    {

        Map current = new Map();

        //Gets the description of the room that the player is in, the RMPos pointer however is in class player
        public string getDescription(Point RMPos)
        {
            return current.Rooms[$"Room{RMPos.X},{RMPos.Y}"].Description;
        }
        //Function that returns the RMPos in the specified direction. Must test if room exists first with canMove or code breaks.
        public Point getNext(Point RMPos, string direction)
        {
            if (direction == "UP")
            {
                return new Point(RMPos.X, RMPos.Y + 1);
            }
            else if (direction == "DOWN")
            {
                return new Point(RMPos.X, RMPos.Y - 1);
            }
            else if (direction == "LEFT")
            {
                return new Point(RMPos.X - 1, RMPos.Y);
            }
            else if (direction == "RIGHT")
            {
                return new Point(RMPos.X + 1, RMPos.Y);
            }
            return RMPos;
        }
        //True if a room exists in the given direction, false otherwise
        public Boolean canMove(Point RMPos, string direction)
        {
            if (direction == "UP")
            {
                if (current.Rooms.TryGetValue($"Room{RMPos.X},{RMPos.Y + 1}", out var room))
                {
                    return true;
                }
                return false;
            }
            else if (direction == "DOWN")
            {
                if (current.Rooms.TryGetValue($"Room{RMPos.X},{RMPos.Y - 1}", out var room))
                {
                    return true;
                }
                return false;
            }
            else if (direction == "LEFT")
            {
                if (current.Rooms.TryGetValue($"Room{RMPos.X - 1},{RMPos.Y}", out var room))
                {
                    return true;
                }
                return false;
            }
            else if (direction == "RIGHT")
            {
                if (current.Rooms.TryGetValue($"Room{RMPos.X + 1},{RMPos.Y}", out var room))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        //Function that check/set if map room is completed
        public Boolean getStatus(Point RMPos)
        {
            if (current.Rooms[$"Room{RMPos.X},{RMPos.Y}"].Completed)
            {
                return true;
            }
            return false;
        }

        public void setStatusDone(Point RMPos)
        {
            current.Rooms[$"Room{RMPos.X},{RMPos.Y}"].Completed = true;
            current.SaveMap(); //Autosaves this
        }


        //Call the map level function generatemap

        public void GenerateNormal()
        {
            current.GenerateNormalMap();
        }
        public void Save()
        {
            current.SaveMap();
        }
    }
}