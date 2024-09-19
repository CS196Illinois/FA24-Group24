// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using PlayerEND;
using UIEnd;
namespace Program
{    
    class Program    
    {
        //have basic login and exit command
        static void Main(string[] args)
        {       
            Console.WriteLine("Hello to our game, type 'quit' to exit");
            while (true) 
            {      
                string response;   
                string command;     
                UI Window = new UI();
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

