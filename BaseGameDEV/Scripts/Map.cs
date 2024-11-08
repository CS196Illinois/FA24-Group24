using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using minigame;
using System.ComponentModel;



namespace MapEnd
{
    class Map
    {
        public class Room
        {   //Model class that carries the information containing in each room / need to change access level tho
            public string Description { get; set; }
            public bool Completed { get; set; }
            public int[] Adjacent { get; set; }
        }

        static string filepath = "Saves/map.json"; //Address 
        public Dictionary<int, string> mapInfo = new Dictionary<int, string> // Current possible room state
        {
            [1] = "DiceRoll",
            [2] = "MathQuestion",
            [3] = "MazeGame",
            [4] = "Shop",
            [5] = "Treasure",
            [6] = "CardGame",
            [7] = "Keyroom"
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

        //Used to determine if a room logically connects or not
        int RoomLogic(int origin, String movement)
        {
            int ans = origin;
            int test;
            //If a room doesn't logically connect, the player will stay in the same room.
            if (movement.Equals("U"))
            {
                test = origin + rowsize;
                if (test <= rowsize * rowsize && test >= 0)
                {
                    ans = test;
                }
            }
            else if (movement.Equals("D"))
            {
                test = origin - rowsize;
                if (test <= rowsize * rowsize && test >= 0)
                {
                    ans = test;
                }
                //Left and right stop the rooms from connecting when they're on opposite sides
                //Since it's in a grid pattern, you can't move left on a leftside edge, and can't move right on a rightside edge
            }
            else if (movement.Equals("L"))
            {
                test = origin - 1;
                if (origin % rowsize != 0)
                {
                    ans = test;
                }
            }
            else if (movement.Equals("R"))
            {
                test = origin + 1;
                if (origin % rowsize != rowsize - 1)
                {
                    ans = test;
                }
            }
            return ans;
        }
        public string GenerateNormalMap()
        {
            File.WriteAllText(filepath, ""); //Clear the file
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            Random random = new Random(); //Randomization
            List<int> keyspots = new List<int>(); // keyroom locations
            for (int n = 0; n < 3; n++)
            {
                Random rando = new Random();
                int num = rando.Next(rowsize, rowsize * rowsize);
                while (keyspots.Contains(num))
                {
                    num = rando.Next(rowsize, rowsize * rowsize);
                }
                keyspots.Add(num);
            }
            for (int i = 0; i < rowsize * rowsize; i++) //Creates rooms
            {
                //Add rowsize^2 random encounter rooms in a set order
                if (keyspots.Contains(i))
                {
                    temp.Add($"Room{i}", new Room() { Description = mapInfo[7], Completed = false, Adjacent = [RoomLogic(i, "U"), RoomLogic(i, "D"), RoomLogic(i, "L"), RoomLogic(i, "R")] });
                }
                else
                {
                    temp.Add($"Room{i}", new Room() { Description = mapInfo[random.Next(1, 6)], Completed = false, Adjacent = [RoomLogic(i, "U"), RoomLogic(i, "D"), RoomLogic(i, "L"), RoomLogic(i, "R")] });
                }
            }
            File.WriteAllText(filepath, JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true })); //Convert to Json and add
            LoadMap(); //Reload the map
            return "Completed";
        }

        public string GenerateMap()
        {
            File.WriteAllText(filepath, ""); //Clear the file
            //Prep the list of rooms
            Dictionary<string, Room> temp = new Dictionary<string, Room>();
            Random random = new Random(); //Randomization
            for (int i = 0; i < rowsize * rowsize; i++)
            {
                //Add rowsize^2, each with a random adjacent array leading to non-euclidean navigating
                temp.Add($"Room{i}", new Room() { Description = mapInfo[random.Next(1, 4)], Completed = false, Adjacent = [random.Next(0, 25), random.Next(0, 25), random.Next(0, 25), random.Next(0, 25)] });
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

        //Gets the description of the room that the player is in, the RMNumber pointer however is in class player
        public string getDescription(int RMNumber)
        {
            return current.Rooms[$"Room{RMNumber}"].Description;
        }

        //Function that returns the RmNumber according to the adjacent integer array in each room and user command. 
        public int getNext(int RMNumber, string direction)
        {
            if (direction == "UP")
            {
                return current.Rooms[$"Room{RMNumber}"].Adjacent[0];
            }
            else if (direction == "DOWN")
            {
                return current.Rooms[$"Room{RMNumber}"].Adjacent[1];
            }
            else if (direction == "LEFT")
            {
                return current.Rooms[$"Room{RMNumber}"].Adjacent[2];
            }
            else if (direction == "RIGHT")
                return current.Rooms[$"Room{RMNumber}"].Adjacent[3];
            return RMNumber;
        }

        //Function that check/set if map room is completed
        public Boolean getStatus(int RMNumber)
        {
            if (current.Rooms[$"Room{RMNumber}"].Completed)
            {
                return true;
            }
            return false;
        }

        public void setStatusDone(int RMNumber)
        {
            current.Rooms[$"Room{RMNumber}"].Completed = true;
            current.SaveMap(); //Autosaves this
        }



        //Call the map level function generatemap
        public void Generate()
        {
            current.GenerateMap();
        }
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
