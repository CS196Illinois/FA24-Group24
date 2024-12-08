using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using PlayerEnd;
namespace minigame
{
    class Minigame
    {
        System.Random random = new System.Random();
        private int diff;
        public Minigame(int idiff){ 
            diff = idiff;
        }

        public KeyValuePair<bool, string> DiceRoll() {
            int result = random.Next(0, 100);
            int threshold;
            if (diff <= 5) {
                threshold = 10;
            } else if (diff <= 10) {
                threshold = 40;
            } else if (diff <= 25) {
                threshold = 70;
            } else {
                threshold = 90;
            }
            if (result < threshold) {
                return new KeyValuePair<bool, string>(true, "Rolled: " + result + " Needed: <" + threshold);
            }
            return new KeyValuePair<bool, string>(false, "Rolled: " + result + " Needed: <" + threshold + " GET INT ");    
        }

        private Dictionary<int, string> statList = new Dictionary<int, string>() {
            [0] = "ATK",
            [1] = "DEF",
            [2] = "INT",
            [3] = "SPD"
        };

        public KeyValuePair<bool, string> StatCheck() {
            int r = random.Next(0, 3);
            PlayerAction stataccess = new PlayerAction();
            int statvalue = stataccess.GETSTAT(statList[r]);
            if (statvalue <= 10) {
                return new KeyValuePair<bool, string>(false, "Your " + statList[r] + " is too low");
            }
            return new KeyValuePair<bool, string>(true, "Your " + statList[r] + " is enough");
        }

    }
    class minigameWrapper
    {
        Minigame current;
        MethodInfo methodInfo;


        public minigameWrapper(string iminigame, int diff) {
            string minigame = iminigame;
            current = new Minigame(diff);
            methodInfo = typeof(Minigame).GetMethod(minigame);
        }

        public  KeyValuePair<bool, string> checkResult() {
            if (methodInfo == null) {
                return new KeyValuePair<bool, string>(true, "Vacuumly True");
            } 
            KeyValuePair<bool, string> response = (KeyValuePair<bool, string>) methodInfo.Invoke(current, null);
            return response;
        }

    }
}