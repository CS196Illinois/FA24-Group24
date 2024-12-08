using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Drawing;
using Unity.VisualScripting;

namespace PlayerEnd
{
    class Player
    {        
        static public string filepath = "Assets/Saves/characters.json";

        public class PlayerModel //Data Model that contains all character-related information and json format 
        {
            public string name {get; set;}
            public Dictionary<string, int> stats {get; set;}
            public Dictionary<string, int> Items {get; set;}
            public Point RoomPos {get; set;}
        }
    
        //Loading in Player
        private PlayerModel current = new PlayerModel();

        public void LoadPlayer() {
            string e = File.ReadAllText(filepath);
            current = JsonConvert.DeserializeObject<PlayerModel>(e);
            Debug.Log("Completed");
        }

        public void SavePlayer() {
            string e = JsonConvert.SerializeObject(current);
            File.WriteAllText(filepath, e);
        }
    
        // Get & Set the player's name or prompts to set it if not set
        public string GetName() {
            return current.name;
        }

        public void SetName(string inputName) {
            current.name = inputName;
        }

        // Get & Set the player's stats
        public Dictionary<string, int> GetStat() {
            return current.stats;
        }

        public int GetStat(string cat) {
            if (current.stats.Keys.Contains(cat)) {
                return current.stats[cat];
            } else {
                return -1;
            }
        }

        public void SetStat(string cat, int val) {
            if (current.stats.Keys.Contains(cat)) {
                current.stats[cat] = val;
            } else {
                current.stats.Add(cat, val);
            }
        }
        
        public void AddStat(string cat, int val) {
            if (current.stats.Keys.Contains(cat)) {
                current.stats[cat] += val;
            } 
            SavePlayer();
        }

        public void AddItem(string name, int number) {
            if (current.Items.Keys.Contains(name)) {
                current.Items[name] += number;
            } else {
                current.Items.Add(name, number);
            }
            SavePlayer();
        }

        public Dictionary<string, int> GetItem() {
            return current.Items;
        }

        public int GetItem(string key) {
            return current.Items[key];
        }

        public Point GetPOS() {
            return current.RoomPos;
        }

        public void SetPOS(Point input) {
            current.RoomPos = input;
        }


        public void ResetStats() {
            current.stats = new Dictionary<string, int> {
                ["HP"] =  100,
                ["EXP"] = 0,
                ["ATK"] = 5,
                ["DEF"] = 5,
                ["INT"] = 5,
                ["SPD"] = 5,
            };
            SavePlayer();
        }

        public void ResetItems() {
            current.Items = new Dictionary<string, int> {
                ["Key"] =  0,
                ["Coin"] = 0
            };
            SavePlayer();
        }

        //Constructor that automatically load player when new session initiates
        public Player() {
            LoadPlayer();
            Debug.Log("Player Loaded");
        }
    }
}