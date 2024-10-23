using System;
using PlayerEnd;
using System.Reflection;
using System.Dynamic;

namespace UIEnd
{
    class UI
    {
        private PlayerAction playerAction {get; set;} 
        public UI() {
            playerAction = new PlayerAction();    
        }   

        // Returns a list of available commands and their effects
        public string GetHelp(){
            return "Player Commands:\n" +
                   "SETNAME - Set player's name\n" +
                   "GETNAME - Get player's name\n" +
                   "GETITEM - Get player's items\n" +
                   "GETSTAT - Get player's stats\n" +
                   "UP - Move up\n" +
                   "DOWN - Move down\n" +
                   "LEFT - Move left\n" +
                   "RIGHT - Move right\n" +
                   "Developer Commands: \n" +
                   "GETRM - Get current room\n" +
                   "EXPLORE - Explore the room\n" +
                   "GENERATEN - Generate a grid-based map";
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
                return "Method Not Found.";
            }
         
            return response;
        }

        public void Save() {
            playerAction.__save__();
        }
    } 
}
