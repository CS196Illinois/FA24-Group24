// See https://aka.ms/new-console-template for more information

using System;
using UIEnd;
namespace Program
{    
    class Program    
    {
        //have basic login and exit command
        static void Main(string[] args)
        {       
            Console.WriteLine("Hello to our game, type 'quit' to exit");
            UI Window = new UI();
            while (true) 
            {      
                string response;   
                string command;     
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

