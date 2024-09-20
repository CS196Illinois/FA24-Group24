using System;
using System.Collections.Generic;

namespace MapEnd
{
    class Node
    {
        string status; // Room status
        Node? previousNode; // Reference to previous room

        public Node(string inputStatus, Node? inputNode) {
            status = inputStatus;
            previousNode = inputNode;
        } 

        public string GetStatus() {
            return status;
        }

        public Node? GetPrev() {
            return previousNode;
        }
    }
    
    class Map
    {
        Dictionary<int, string> mapInfo = new Dictionary<int, string>
        {
            [1] = "Nothing to be found here",
            [2] = "Do a math problem",
            [3] = "Tell me your name", 
            [4] = "A giant statue",
            [5] = "Swift-Footed Achilles",
            [6] = "Agamemnon, Lion of Mycenae",
            [7] = "Hector of Troy",
            [8] = "Rhesus of Thrace"
        };

        Random random = new Random();
        Node root = new Node("Start", null); // Starting node

        // Generates a new room with a random status
        public void NEWROOM() {
            Node tempNode = new Node(mapInfo[random.Next(1, 9)], root);
            root = tempNode;
        }    

        // Returns the current room's status
        public string GetCurrentStatus() {
            return root.GetStatus();
        }
    }
}
