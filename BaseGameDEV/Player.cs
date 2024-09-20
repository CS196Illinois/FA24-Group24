using System;
using MapEnd;

namespace PlayerEnd
{
    class Player
    {
        private string? name; 
        public int[] position = new int[2] { 0, 0 }; 

        // Gets the player's name or prompts to set it if not set
        public string GetName() {
            return name ?? "Set name first.";
        }

        // Sets the player's name
        public void SetName(string inputName) {
            name = inputName;
        }
    }

    class PlayerAction
    {
        private Player player; 
        private Map currentMap; 

        public PlayerAction(Player inputPlayer, Map inputMap) {
            player = inputPlayer;
            currentMap = inputMap;
        }

        public void SETNAME(string name) {
            player.SetName(name);
        }

        public string GETNAME() {
            return player.GetName();
        }

        public void UP() {
            player.position[1]++;
        }

        public void DOWN() {
            player.position[1]--;
        }

        public void LEFT() {
            player.position[0]--;
        }

        public void RIGHT() {
            player.position[0]++;
        }

        public string GETPOS() {
            return $"({player.position[0]}, {player.position[1]})";
        }  

        public string EXPLORE() {
            currentMap.NEWROOM();
            return currentMap.GetCurrentStatus();
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
    }
}
