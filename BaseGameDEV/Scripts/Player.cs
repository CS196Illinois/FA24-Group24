using System;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using MapEnd;
using minigame;


namespace PlayerEnd
{
    class Player
    {        
        public class PlayerModel //Data Model that contains all character-related information and json format 
        {
            public string? Name {get; set;}
            public Dictionary<string, int>? Stats {get; set;}
            public Point RoomPos {get; set;}
        }
    
        //Loading in Player
<<<<<<< HEAD
        string filepath = "C:\\Users\\zacha\\FA24-Group24-6\\BaseGameDEV\\Saves\\characters.json";
=======
        string filepath = "C:\\Users\\zacha\\FA24-Group24-4\\BaseGameDEV\\Saves\\characters.json";
>>>>>>> origin/ZachWork
        private string? name;
        private Dictionary<string, int>? stats;
        public Point RMPos;
        public void LoadPlayer() {
            string e = File.ReadAllText(filepath);
            PlayerModel? temp = JsonSerializer.Deserialize<PlayerModel>(e);
            if (temp == null) {
                throw new Exception("Error loading in player info.");
            }
            name = temp.Name;
            stats = temp.Stats;
            RMPos = temp.RoomPos;
        }

        //Save PlayerInfo into characters.json
        public void SavePlayer() {
            File.WriteAllText(filepath, JsonSerializer.Serialize(new PlayerModel(){Name=name, Stats=stats, RoomPos = RMPos}, new JsonSerializerOptions { WriteIndented = true }));
        }


        // Get & Set the player's name or prompts to set it if not set
        public string GetName() {
            return name ?? "Set name first.";
        }

        public void SetName(string inputName) {
            name = inputName;
        }

        // Get & Set the player's stats
        public Dictionary<string, int> GetStat() {
            return stats;
        }

        public int GetStat(string cat) {
            if (stats.Keys.Contains(cat)) {
                return stats[cat];
            } else {
                return -1;
            }
        }
        public Dictionary<string, int> GetStats() {
            return stats;
        }

        public void SetStat(string cat, int val) {
            if (stats.Keys.Contains(cat)) {
                stats[cat] = val;
            } else {
                stats.Add(cat, val);
            }
        }
        
        public void AddStat(string cat, int val) {
            if (stats.Keys.Contains(cat)) {
                stats[cat] += val;
            } 
        }
        //Gives a player a +10 boost in a random stat, 1 in 4 chance of player being able to pick the stat, 1 in 4 chance of boost being +15 instead.
        public string CollectItem() {
            Random random = new Random();
            int r = random.Next(0, 16);
            int toAdd = 10;
            if (r % 4 == 0) {
                toAdd += 5;
            }
            if (r <= 3) {
                Console.WriteLine("Boost item: select which stat to improve:\nATK for Attack\nDEF for Defense\nSPD for Speed\nDEX for Dexterity\nINT for Intelligence");
                Boolean isDone = false;
                //User could input HP or EXP, causing unintended boosts. May have to make health and experience seperate from stats.
                String stat = Console.ReadLine().ToUpper().Trim();
                while (!isDone) {
                    if (stats.TryGetValue(stat, out int num)) {
                        stats[stat] += toAdd;
                        isDone = true;
                        return toAdd + " was added to " + stat + ".";
                    } else {
                        Console.WriteLine("Enter a valid three-letter stat code");
                    }
                }
            } else {
                string stat = stats.Keys.ToArray()[random.Next(2, 7)];
                stats[stat] += toAdd;
                return toAdd + " was added to " + stat + ".";
            }
            return "Error";
        }
        public void ResetStats() {
            stats = new Dictionary<string, int> {
                ["HP"] =  100,
                ["EXP"] = 0,
                ["ATK"] = 0,
                ["DEF"] = 0,
                ["SPD"] = 0,
                ["DEX"] = 0,
                ["INT"] = 0,
            };
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

        public string GETSTAT() {
            return JsonSerializer.Serialize(player.GetStat());
        }

        public void UP() {
            if (!mymap.getStatus(player.RMPos)) {  //Just check to see if the room is completed
                Console.WriteLine("Complete this room first!");
            } else if (mymap.canMove(player.RMPos, "UP")) {
            player.RMPos = mymap.getNext(player.RMPos, "UP");
            Console.WriteLine(GETPOS());
            Console.WriteLine(EXPLORE());
            } else {
                Console.WriteLine("No Room This Way!");
            }
        }
        public void DOWN() {
            if (!mymap.getStatus(player.RMPos)) {
                Console.WriteLine("Complete this room first!");
            } else  if (mymap.canMove(player.RMPos, "DOWN")) {
            player.RMPos = mymap.getNext(player.RMPos, "DOWN");
            Console.WriteLine(GETPOS());
            Console.WriteLine(EXPLORE());
            } else {
                Console.WriteLine("No Room This Way!");
            }
        }
        public void LEFT() {
            if (!mymap.getStatus(player.RMPos)) {
                Console.WriteLine("Complete this room first!");
            } else  if (mymap.canMove(player.RMPos, "LEFT")) {
            player.RMPos = mymap.getNext(player.RMPos, "LEFT");
            Console.WriteLine(GETPOS());
            Console.WriteLine(EXPLORE());
            } else {
                Console.WriteLine("No Room This Way!");
            }
        }
        public void RIGHT() {
            if (!mymap.getStatus(player.RMPos)) {
                Console.WriteLine("Complete this room first!");
            } else  if (mymap.canMove(player.RMPos, "RIGHT")) {
            player.RMPos = mymap.getNext(player.RMPos, "RIGHT");
            Console.WriteLine(GETPOS());
            Console.WriteLine(EXPLORE());
            } else {
                Console.WriteLine("No Room This Way!");
            }
        }


        //Backend Actions eventually change all reference to lowercase (or private) so user can't access

        public void ADDSTAT(string stat, int increment) {
            player.AddStat(stat.ToUpper(), increment);
        }
       
        public string GETPOS() {
            return $"Position: {player.RMPos.X}, {player.RMPos.Y}";
        }  

        public string EXPLORE() {
            return mymap.getDescription(player.RMPos);       
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
            if (!mymap.getStatus(player.RMPos)) {
                if (mymap.getDescription(player.RMPos).Equals("Minigame")) { //tests if room is a minigame
                    minigameWrapper challenge = new minigameWrapper(Math.Abs(player.RMPos.X) + Math.Abs(player.RMPos.Y));
                    if (challenge.checkResult()) {
                        mymap.setStatusDone(player.RMPos); //Update room to be completed
                        player.AddStat("EXP", 100);
                        return "Good job, you may continue.";
                    } else {
                        player.RMPos = new Point(0, 0);
                        return "Back to the beginning";
                    }
                } else if (mymap.getDescription(player.RMPos).Equals("Item")) { //tests if room is an item
                    mymap.setStatusDone(player.RMPos);
                    player.AddStat("EXP", 50);
                    return player.CollectItem();
                } else {
                    mymap.setStatusDone(player.RMPos);
                    return "Empty, Key, and Combat rooms still in development, you may continue.";
                }
            } else {
                return "You have completed this room before";
            }
            
        }

        //generate --> MapAction Generate --> Map GenerateMap Deprecated lowercase
        public string generate() {
            mymap.Generate();
            return "New Random Map Generated";
        }

        //GENERATEN --> MapAction GenerateNormal --> Map GenerateNormalMap
        public string GENERATEN() {
            mymap.GenerateNormal();
            return "New Map Generated";
        }
        //Generate new map, return player to first square, reset stats and room progress
        public string RESET() {
            mymap.GenerateNormal();
            player.RMPos = new Point(0, 0);
            player.ResetStats();
            return "Game Reset";
        }

        //UI save command --> __save__ which saves both map via MapAction Save() and player via Player SavePlayer()
        //Not callable from UI process() since not capitalized 
        public void __save__() {
            mymap.Save();
            player.SavePlayer();
        }
    }
}