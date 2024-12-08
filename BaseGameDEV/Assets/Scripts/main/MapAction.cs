using System;
using System.IO;
using System.Collections.Generic;
using minigame;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Drawing;
using Unity.VisualScripting;

namespace MapEnd
{   

    class MapAction {      
        Map current = new Map();

        
        //Gets the description of the room that the player is in, the RMNumber pointer however is in class player
        public string getDescription(Point RMPos) { 
            return current.rooms[$"Room{RMPos.X},{RMPos.Y}"].Description;
        }  

        //Function that returns the RmNumber according to the adjacent integer array in each room and user command. 
        public Point getNext(Point RMPos, string direction) {
            if (direction == "UP") {
                return new Point(RMPos.X, RMPos.Y + 1);
            } else if (direction == "DOWN") {
                return new Point(RMPos.X, RMPos.Y - 1);
            } else if (direction == "LEFT") {
                return new Point(RMPos.X - 1, RMPos.Y);
            } else if (direction == "RIGHT") {
                return new Point(RMPos.X + 1, RMPos.Y);
            }
            return RMPos;
        }

        public Boolean canMove(Point RMPos, string direction) {
            if (direction == "UP") {
                if (current.rooms[$"Room{RMPos.X},{RMPos.Y}"].Adjacent[0] == -1) {
                    return false;
                } else {
                    return true;
                }
            } else if (direction == "DOWN") {
                if (current.rooms[$"Room{RMPos.X},{RMPos.Y}"].Adjacent[1] == -1) {
                    return false;
                } else {
                    return true;
                }
            } else if (direction == "LEFT") {
                if (current.rooms[$"Room{RMPos.X},{RMPos.Y}"].Adjacent[2] == -1) {
                    return false;
                } else {
                    return true;
                }
            } else if (direction == "RIGHT")
                if (current.rooms[$"Room{RMPos.X},{RMPos.Y}"].Adjacent[3] == -1) {
                    return false;
                } else {
                    return true;
                }
            return false;
        }

        //Function that check/set if map room is completed
        public bool getStatus(Point RMPos) {
            if (current.rooms[$"Room{RMPos.X},{RMPos.Y}"].Completed) {
                return true;
            }
            return false;
        }

        public void setStatusDone(Point RMPos) {
            current.rooms[$"Room{RMPos.X},{RMPos.Y}"].Completed = true;
            current.SaveMap(); //Autosaves this
        }

        public void resetStatusTotal() {
            foreach (var room in current.rooms) {
                room.Value.Completed = false;
            }
            current.SaveMap();
        }


        //Call the map level function generatemap

        public void GenerateNormal() {
            current.GenerateNormalMapZach();
        }  
        public void Save() {
            current.SaveMap();
        }
    }
}
