namespace PlayerEND
{
class Player
    {
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
        //basic movement so far
        //can only return strings
        //Uppercase method ONLY
        private Player P;
        public PlayerAction(Player iplayer){
            P = iplayer;
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

        // Test Methods
        public String ECHO(string subject){
            return subject;
        }

        public String SUM(int FirstNum, int SecondNum, int ThirdNum) {
            return $"The sum is: {FirstNum + SecondNum + ThirdNum}";
        }

    }
}