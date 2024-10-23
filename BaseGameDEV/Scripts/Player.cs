using System;
using System.IO;
using System.Text.Json;
using MapEnd;
using minigame;


namespace PlayerEnd
{
    public class Player
    {
        public class PlayerModel //Data Model that contains all character-related information and json format 
        {
            public string? Name { get; set; }
            public Dictionary<string, int>? Stats { get; set; }

            public Dictionary<string, int>? Items { get; set; }
            public int RoomNumber { get; set; }
        }

        //Loading in Player
        string filepath = "Saves/characters.json";
        private string? name;
        private Dictionary<string, int>? stats;
        public Dictionary<string, int>? items;
        public int RMNumber;
        public void LoadPlayer()
        {
            string e = File.ReadAllText(filepath);
            PlayerModel? temp = JsonSerializer.Deserialize<PlayerModel>(e);
            if (temp == null)
            {
                throw new Exception("Error loading in player info.");
            }
            name = temp.Name;
            stats = temp.Stats;
            items = temp.Items;
            RMNumber = temp.RoomNumber;
        }

        //Save PlayerInfo into characters.json
        public void SavePlayer()
        {
            File.WriteAllText(filepath, JsonSerializer.Serialize(new PlayerModel() { Name = name, Stats = stats, Items = items, RoomNumber = RMNumber }, new JsonSerializerOptions { WriteIndented = true }));
        }


        // Get & Set the player's name or prompts to set it if not set
        public string GetName()
        {
            return name ?? "Set name first.";
        }

        public void SetName(string inputName)
        {
            name = inputName;
        }

        // Get & Set the player's stats
        public Dictionary<string, int> GetStat()
        {
            return stats;
        }

        public int GetStat(string cat)
        {
            if (stats.ContainsKey(cat))
            {
                return stats[cat];
            }
            else
            {
                return -1;
            }
        }

        public void SetStat(string cat, int val)
        {
            if (stats.ContainsKey(cat))
            {
                stats[cat] = val;
            }
            else
            {
                stats.Add(cat, val);
            }
        }

        public void AddStat(string cat, int val)
        {
            if (stats.ContainsKey(cat))
            {
                stats[cat] += val;
            }
        }

        //Get and add items into the inventory
        public Dictionary<string, int> GetItems()
        {
            return items;
        }

        public void AddItem(string cat)
        {
            if (items.ContainsKey(cat))
            {
                items[cat] = items[cat] + 1;
            }
            else
            {
                items.Add(cat, 1);
            }
        }


        //Constructor that automatically load player when new session initiates
        public Player()
        {
            LoadPlayer();
        }
    }

    class PlayerAction
    {
        private Player player;
        private MapAction mymap;


        public PlayerAction()
        {
            player = new Player();
            mymap = new MapAction();
        }


        //End-User Actions
        public void SETNAME(string name)
        {
            player.SetName(name);
        }

        public string GETNAME()
        {
            return player.GetName();
        }

        public string GETSTAT()
        {
            return JsonSerializer.Serialize(player.GetStat());
        }

        public string GETITEM()
        {
            return JsonSerializer.Serialize(player.GetItems());
        }

        public void UP()
        {
            if (!mymap.getStatus(player.RMNumber))
            {  //Just check to see if the room is completed
                Console.WriteLine("Complete this room first!");
            }
            else
            {
                player.RMNumber = mymap.getNext(player.RMNumber, "UP");
                Console.WriteLine(GETRM());
                Console.WriteLine(EXPLORE());
            }
        }
        public void DOWN()
        {
            if (!mymap.getStatus(player.RMNumber))
            {
                Console.WriteLine("Complete this room first!");
            }
            else
            {
                player.RMNumber = mymap.getNext(player.RMNumber, "DOWN");
                Console.WriteLine(GETRM());
                Console.WriteLine(EXPLORE());
            }
        }
        public void LEFT()
        {
            if (!mymap.getStatus(player.RMNumber))
            {
                Console.WriteLine("Complete this room first!");
            }
            else
            {
                player.RMNumber = mymap.getNext(player.RMNumber, "LEFT");
                Console.WriteLine(GETRM());
                Console.WriteLine(EXPLORE());
            }
        }
        public void RIGHT()
        {
            if (!mymap.getStatus(player.RMNumber))
            {
                Console.WriteLine("Complete this room first!");
            }
            else
            {
                player.RMNumber = mymap.getNext(player.RMNumber, "RIGHT");
                Console.WriteLine(GETRM());
                Console.WriteLine(EXPLORE());
            }
        }


        //Backend Actions eventually change all reference to lowercase (or private) so user can't access

        public void ADDSTAT(string stat, int increment)
        {
            player.AddStat(stat.ToUpper(), increment);
        }

        public string GETRM()
        {
            return $"Room {player.RMNumber}";
        }

        public string EXPLORE()
        {
            return mymap.getDescription(player.RMNumber);
        }

        public string ECHO(string subject)
        {
            return subject;
        }

        public string SUM(int firstNum, int secondNum, int thirdNum)
        {
            return $"The sum is: {firstNum + secondNum + thirdNum}";
        }

        public string HELLO()
        {
            return "Hello";
        }

        public string EVALUATE()
        {
            if (!mymap.getStatus(player.RMNumber))
            {
                minigameWrapper challenge = new minigameWrapper(mymap.getDescription(player.RMNumber), player.RMNumber);
                if (challenge.checkResult())
                {
                    mymap.setStatusDone(player.RMNumber); //Update room to be completed
                    return "You may continue.";
                }
                else
                {
                    player.RMNumber = 0;
                    return "Back to the beginning";
                }
            }
            else
            {
                return "You have completed this room before";
            }

        }

        //generate --> MapAction Generate --> Map GenerateMap Deprecated lowercase
        public string generate()
        {
            mymap.Generate();
            return "New Random Map Generated";
        }

        //GENERATEN --> MapAction GenerateNormal --> Map GenerateNormalMap
        public string GENERATEN()
        {
            mymap.GenerateNormal();
            return "New Map Generated";
        }

        //UI save command --> __save__ which saves both map via MapAction Save() and player via Player SavePlayer()
        //Not callable from UI process() since not capitalized 
        public void __save__()
        {
            mymap.Save();
            player.SavePlayer();
        }
    }
}
