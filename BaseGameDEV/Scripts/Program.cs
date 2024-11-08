﻿using System;
using UIEnd;
using minigame;
using System.Reflection;

namespace RPGGame
{    
    class Program    
    {
        static void Main(string[] args)         
        {           
            Console.WriteLine("Welcome to our game! Type 'quit' to exit or 'help' for available commands.");
            UI gameUI = new UI();
            while (true) 
            {      
                Console.Write("Command: ");
                string command = Console.ReadLine();
                
                if (command.Equals("quit")) {
                    //Some sort of save here probably
                    gameUI.Save();
                    break;
                } 
                else if (command.Equals("help")) {
                    Console.WriteLine(gameUI.GetHelp());
                }
                else if (command.Equals("save")) {
                    gameUI.Save();
                    Console.WriteLine("Manual Save");
                }
                else {
                    string response = gameUI.Process(command);
                    if (response != null) {
                        Console.WriteLine(response);
                    } 
                }                
            }
        }
    }
}