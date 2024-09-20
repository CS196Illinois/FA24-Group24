//Experimental using LinkedList, and randomly generated room "status"
//Pre-determined frame work of map
//Generate 5x5 grid. each cell is a new level....
using System;
namespace MapEnd
{    
    class Node
    {
        String status;
        Node? PreviousNode;  
        public Node(String istatus, Node iNNode ){
            status = istatus;
            PreviousNode = iNNode;
        } 

        public String getStatus() {
            return status;
        }
        public Node getPrev(){
            return PreviousNode;
        }
    }
    
    class Map
    {
        //MapInformation I'd assume
        Dictionary<int, String> mapinfo = new Dictionary<int, string>
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

        Node root = new Node("Start", null);

        public void NEWROOM(){
            Node tempNode = new Node(mapinfo[random.Next(1, 8)], root);
            root = tempNode;
        }    
        public String getCurrentStatus(){
            return root.getStatus();
        }
    }
}
