// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using PlayerEND;
namespace ProgramCompiler
{    
    class UI
    {
        //Add UI to handle command input and avoids prolong else if statement
        //Also + prompt user on what command is available
        PlayerAction session;
        public UI(PlayerAction isession) {
            session = isession;
        }   
    
       
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

    class Program    
    {
        //have basic login and exit command
        static void Main(string[] args)
        {       
            Console.WriteLine("Hello to our game, type 'quit' to exit");
            Player Usr = new Player();
            PlayerAction session = new PlayerAction(Usr);
            while (true) 
            {      
                string response;   
                string command;     
                UI Window = new UI(session);
                Console.Write("Command: ");
                command = Console.ReadLine();
                if (command.Equals("quit")) {
                    break;
                } else {
                    response = Window.Process(command);
                    if (response != null) {
                        Console.WriteLine(response);
                    } 
                }
                            
            }
        }
    }
}

