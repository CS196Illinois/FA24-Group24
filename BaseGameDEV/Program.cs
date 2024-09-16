// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Principal;
namespace BaseGameDEV
{  
    class CurrentMap
    {
        //MapInformation I'd assume
    }
    
    class Player
    {
        private string Name;
        public int[] Pos = new int[2];

        public Player(string iname){
            Name = iname;
            Pos[0] = 0;
            Pos[1] = 0;
        }
        public String get() {
            return Name;
        }
        public void set(String iname) {
            Name = iname;
        }
    }

    class PlayerAction
    {
        //basic movement so far
        private Player P;
        public PlayerAction(Player iplayer){
            P = iplayer;
        }
        public void setName(String iname){
            P.set(iname);
        }
        public String getName(){
            return P.get();
        }

        public void UP(){
            P.Pos[1]++;
        }
        public void DOWN(){
            P.Pos[1]--;
        }
        public void LEFT(){
            P.Pos[0]--;
        }
        public void RIGHT(){
            P.Pos[0]++;
        }
        public void getPOS(){
            Console.WriteLine("(" + (P.Pos[0]).ToString() + "," + (P.Pos[1]).ToString() + ")");
        }

        //for testing purpose only
        public int test(int a, int b, int c) {
            return a + b + c;
        }
    }
    class UI
    {
        //Add UI to handle command input and avoids prolong else if statement
        //Also to prompt user on what command is available
        PlayerAction session;

        public UI(PlayerAction isession) {
            session = isession;
        }   
    
           
        public String Process(string command) {
            dynamic response = null;
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
                Console.WriteLine("Method not found.");
            }    

            if (response != null) {
                return response;
            } else {
                return null;
            }
        }

        public String Process(string command, dynamic input) {
            dynamic response = null;
            MethodInfo methodinfo = typeof(PlayerAction).GetMethod(command);
            if (methodinfo != null) {
                response = methodinfo.Invoke(session, input);                
            } else {
                Console.WriteLine("Method not found.");
            }    
            if (response != null) {
                return response;
            } else {
                return null;
            }
        }

        
    }

    class Program    
    {
        //have basic login and exit command
        static void Main(string[] args)
        {
            string command;
            Console.WriteLine("Hello to our game, type 'quit' to exit");
            Console.WriteLine("Enter your name:");
            Player Usr = new Player("Ethan");
            PlayerAction session = new PlayerAction(Usr);
            while (true) 
            {              
                UI Window = new UI(session);
                Console.Write("Command: ");
                command = Console.ReadLine();
                if (command.Equals("quit")) {
                    break;
                } else {
                    Console.WriteLine(Window.Process(command));
                }
                             
            }
        }
    }
}

