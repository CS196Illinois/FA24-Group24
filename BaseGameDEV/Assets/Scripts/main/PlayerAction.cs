using System;
using System.IO;
using UnityEngine;
using MapEnd;
using minigame;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;



namespace PlayerEnd
{
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

        //Important!!!!
        public string UP() {
            Point temp = player.GetPOS();
            if (!mymap.getStatus(temp)) {  //Just check to see if the room is completed
                return "Complete this room first!";
            } else if (mymap.canMove(temp, "UP")) {
                player.SetPOS(mymap.getNext(temp, "UP"));
                return EXPLORE();
            } else {
                return "No Room This Way!";
            }
        }
        public string DOWN() {
            Point temp = player.GetPOS();
            if (!mymap.getStatus(temp)) {  //Just check to see if the room is completed
                return "Complete this room first!";
            } else if (mymap.canMove(temp, "DOWN")) {
                player.SetPOS(mymap.getNext(temp, "DOWN"));
                return EXPLORE();
            } else {
                return "No Room This Way!";
            }
        }
        public string LEFT() {
            Point temp = player.GetPOS();
            if (!mymap.getStatus(temp)) {  //Just check to see if the room is completed
                return "Complete this room first!";
            } else if (mymap.canMove(temp, "LEFT")) {
                player.SetPOS(mymap.getNext(temp, "LEFT"));
                return EXPLORE();
            } else {
                return "No Room This Way!";
            }
        }
        public string RIGHT() {
            Point temp = player.GetPOS();
            if (!mymap.getStatus(temp)) {  //Just check to see if the room is completed
                return "Complete this room first!";
            } else if (mymap.canMove(temp, "RIGHT")) {
                player.SetPOS(mymap.getNext(temp, "RIGHT"));
                return EXPLORE();
            } else {
                return "No Room This Way!";
            }
        }


        //Backend Actions eventually change all reference to lowercase (or private) so user can't access

        //TestFunctions
        public string GETALLSTAT() {
            return 
            " HP: " + player.GetStat("HP").ToString() + "\n" +
            " EXP: " + player.GetStat("EXP").ToString() + "\n" +
            " ATK: " + player.GetStat("ATK").ToString() + "\n" +
            " DEF: " + player.GetStat("DEF").ToString() + "\n" +
            " INT: " + player.GetStat("INT").ToString() + "\n" +
            " SPD: " + player.GetStat("SPD").ToString() + "\n";
        }
        //

        public string GETALLITEM() {
            Dictionary<string, int> items = player.GetItem();
            string output = "";
            foreach (string key in items.Keys) {
                output += key + ":" + items[key] + "\n";
            }
            return output;
        }

        public int GETITEM(string key) {
            return player.GetItem(key);
        }

        public void ADDITEM(string key, int value) {
            player.AddItem(key, value);
        }

        public int GETSTAT(string Stat) {
            return player.GetStat(Stat);
        }

        public void ADDSTAT(string stat, int increment) {
            player.AddStat(stat.ToUpper(), increment);
        }

        public void SETSTAT(string stat, int value) {
            player.SetStat(stat.ToUpper(), value);
        }
       
        public Point GETPOS() {
            return player.GetPOS();
        }
        
        public string EXPLORE() {
            Point temp = player.GetPOS();
            if (mymap.getDescription(temp) == "Minigame") {
                return "Minigame"; //Hard coded in...
            } else {
                return mymap.getDescription(temp);  
            }          
        }

        public string HELLO() {
            return "Hello";
        }

        private string CollectStat() {
            System.Random random = new System.Random();
            int r = random.Next(0, 16);
            int toAdd = 3;
            if (r == 0) {
                toAdd += 5;
            }
            string stat = player.GetStat().Keys.ToArray()[random.Next(2, 5)];
            player.AddStat(stat, toAdd);
            return toAdd + " was added to " + stat + ".";
        }
        
        private KeyValuePair<string, string> MiniGameRandomizer() {  //Random selects a minigame when player enters a minigame room
            System.Random rand = new System.Random();
            int r = rand.Next(1, 4);
            if (r <= 2) {
                return new KeyValuePair<string, string>("DiceRoll", "INT"); 
            } else {
                return new KeyValuePair<string, string>("StatCheck", "NAH"); 
            }
        }

        public string EVALUATE() {  //Central Room evaluation logic
            Point temp = player.GetPOS();
            if (!mymap.getStatus(temp)) {
                if (mymap.getDescription(temp).Equals("Minigame")) { //tests if room is a minigame
                    KeyValuePair<string, string> minigame = MiniGameRandomizer();
                    minigameWrapper challenge = new minigameWrapper(minigame.Key, player.GetStat(minigame.Value)); //difficulty
                    KeyValuePair<bool, string> result = challenge.checkResult();
                    if (result.Key) {
                        mymap.setStatusDone(temp); 
                        player.AddStat("EXP", 100);
                        player.AddItem("Coin", 50);
                        return result.Value + "\n" + "Good to Go";
                    } else {
                        player.AddStat("HP", -20); //Lose health
                        player.AddStat("EXP", 10); //Some EXP
                        player.SetPOS(new Point(0, 0)); //Reset player back to beginning...
                        return result.Value + "\n" + "Back to the beginning";
                    }
                } else if (mymap.getDescription(temp).Equals("Item")) { 
                    mymap.setStatusDone(temp);
                    player.AddItem("Coin", 20);
                    return "You acquired some new coins and items";
                } else if  (mymap.getDescription(temp).Equals("Key")) {
                    mymap.setStatusDone(temp);
                    player.AddItem("Key", 1);
                    player.AddStat("EXP", 50);
                    return "You acquired a new key!";
                } else if (mymap.getDescription(temp).Equals("Treasure")) {
                    mymap.setStatusDone(temp);
                    player.AddStat("EXP", 50);
                    return CollectStat();
                } else if (mymap.getDescription(temp).Equals("Empty")) {
                    mymap.setStatusDone(temp);
                    player.AddStat("EXP", 10);
                    System.Random random = new System.Random();
                    if (random.Next(0,3) == 1) {
                        player.AddItem("Coin", 1);
                        return "You found a coin!";
                    }
                    return "Nothing here.";
                } else {
                    mymap.setStatusDone(temp);
                    return "Combat rooms still in development, you may continue.";
                }
            } else {
                return "You have completed this room before";
            }
            
        }

        //GENERATEN --> MapAction GenerateNormal --> Map GenerateNormalMap
        public string GENERATE() {
            mymap.GenerateNormal();
            player.SetPOS(new Point(0, 0));
            return "New Map Generated";
        }

        //UI save command --> __save__ which saves both map via MapAction Save() and player via Player SavePlayer()
        //Not callable from UI process() since not capitalized 

        //total reset
        public void reset() {
            player.ResetItems();
            player.ResetStats();
            player.SetPOS(new Point(0, 0));
            player.SavePlayer();
            mymap.resetStatusTotal();
        }

    }
}
