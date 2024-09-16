// See https://aka.ms/new-console-template for more information

using System;
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
    }
    class UI
    {
        //Add UI to handle command input and avoids prolong else if statement
        //Also to prompt user on what command is available
    }

    class Program    
    {
        //have basic login and exit command
        static void Main(string[] args)
        {
            string command;
            Player Usr;
            PlayerAction session;
            Console.WriteLine("Hello to our game, type 'quit' to exit");
            Console.WriteLine("Enter your name:");
            Usr = new Player(Console.ReadLine());
            session = new PlayerAction(Usr);
            while (true) 
            {              
                Console.WriteLine("Command:");
                command = Console.ReadLine();
                if (command == "quit") {
                    break;
                } else if (command == "up") {
                    session.UP();
                } else if (command == "down") {
                    session.DOWN();
                } else if (command == "left") {
                    session.LEFT();
                } else if (command == "right") {
                    session.RIGHT();
                } else if (command == "checkpos") {
                    Console.WriteLine(Usr.Name + " (" + Usr.Pos[0] + ", " + Usr.Pos[1] + ")");
                }  
            }
        }
    }
}

