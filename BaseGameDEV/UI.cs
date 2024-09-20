using System;
using PlayerEnd;
using MapEnd;
using System.Reflection;

namespace UIEnd
{
    class UI
    {
        private Player player; 
        private PlayerAction playerAction; 
        private Map currentMap = new Map(); 

        public UI() {
            player = new Player();
            playerAction = new PlayerAction(player, currentMap);
        }   

        // Returns a list of available commands and their effects
        public string GetHelp(){
            return "Available Commands:\n" +
                   "SETNAME - Set player's name\n" +
                   "GETNAME - Get player's name\n" +
                   "UP - Move up\n" +
                   "DOWN - Move down\n" +
                   "LEFT - Move left\n" +
                   "RIGHT - Move right\n" +
                   "GETPOS - Get current position\n" +
                   "EXPLORE - Explore the room\n" +
                   "ECHO - Echo the message\n" +
                   "SUM - Return the sum of the three numbers\n" +
                   "HELLO - Hellooooooooooooooooo!";
        }

        // Processes user commands and invokes corresponding actions
        public string Process(string command) {
            dynamic response;
            command = command.ToUpper().Trim();
            MethodInfo methodInfo = typeof(PlayerAction).GetMethod(command);

            if (methodInfo != null) {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length == 0) {
                    response = methodInfo.Invoke(playerAction, null);
                } else {
                    object[] args = new object[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++) {
                        Console.WriteLine($"Enter {parameters[i].Name} of type {parameters[i].ParameterType}:");
                        dynamic input = Console.ReadLine();
                        if (input == "") {
                            return null;
                        }
                        args[i] = Convert.ChangeType(input, parameters[i].ParameterType);
                    }
                    response = methodInfo.Invoke(playerAction, args);
                }
            } else {
                return "Method not found.";
            }
         
            return response;
        }
    } 
}
