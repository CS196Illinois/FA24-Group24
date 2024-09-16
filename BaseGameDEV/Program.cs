// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
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
        public string Name;
        public int[] Pos = new int[2];

        public Player(string iname){
            Name = iname;
            Pos[0] = 0;
            Pos[1] = 0;
        }
    }

    class PlayerAction
    {
        //basic movement so far
        private Player P;
        public PlayerAction(Player iplayer){
            P = iplayer;
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
            if (methodinfo != null) {
                response = methodinfo.Invoke(session, null);                
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

