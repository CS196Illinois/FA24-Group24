using System;
using PlayerEND;
using MapEnd;
using System.Reflection;
namespace UIEnd
{
    class UI
    {
        //Add UI to handle command input and avoids prolong else if statement
        //Also + prompt user on what command is available
        Player P;
        PlayerAction session;
        Map current = new Map();
        //currently need to pass map information to playeraction class, need to decide exactly what scope UI should have
        //Also what part of the program should handle interaction between Player and Map, Perhaps all as part of PLayer Action???
        public UI() {
            P = new Player();
            session = new PlayerAction(P, current);
        }   
    
        //Doesn't work yet
        public string Prompt(){
            String message = "";
            MethodInfo[] methods = typeof(PlayerAction).GetMethods(BindingFlags.Public);
            for (int i = 0; i < methods.Length; i++) {
                message += $"{methods[i].Name}\n";
            }
            return message;
        } // Doesn't work yet
           
        public string Process(string command) {
            dynamic response;

            //String processing
            command = command.ToUpper();
            command = command.Trim();
            //

            //Method Retrieval
            MethodInfo methodinfo = typeof(PlayerAction).GetMethod(command);
            if (methodinfo != null) 
            {
                ParameterInfo[] parameters = methodinfo.GetParameters();
                if (parameters.Length == 0){
                    response = methodinfo.Invoke(session, null);
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
                    response = methodinfo.Invoke(session, args);
                }
            } else {
                return "Method not found.";
            }   
            //
         
            return response;
        }
    } 
}