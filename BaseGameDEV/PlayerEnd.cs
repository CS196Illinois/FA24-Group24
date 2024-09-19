using System;
using MapEnd; //Gain Access to Map Operation
namespace PlayerEND
{
class Player
    {
        //Basic Player Attributes
        private string? Name;
        public int[] Pos = [0, 0];
        public String get() {
            if (Name == null) {
                return "Set name first.";
            }
            return Name;
        }
        public void set(String iname) {
            Name = iname;
        }
    }

    class PlayerAction
    {
        //Basic Player Action
        //can only return strings
        //Uppercase method ONLY
        private Player P;
        private Map current;
        public PlayerAction(Player iplayer, Map icurrent){
            P = iplayer;
            current = icurrent;
        }
        public void SETNAME(String name){
            P.set(name);
        }
        public String GETNAME(){
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
        public String GETPOS(){
            return "(" + P.Pos[0].ToString() + "," + P.Pos[1].ToString() + ")";
        }  
        
        //Experimental
        public String EXPLORE()
        {
            current.NEWROOM();
            return current.getCurrentStatus();
        }

        // Test Methods
        public String ECHO(string subject){
            return subject;
        }

        public String SUM(int FirstNum, int SecondNum, int ThirdNum) {
            return $"The sum is: {FirstNum + SecondNum + ThirdNum}";
        }

    }
}