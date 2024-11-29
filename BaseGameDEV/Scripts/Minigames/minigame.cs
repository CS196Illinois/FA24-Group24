using System.Dynamic;
using System.Net;
using System.Reflection;
namespace minigame
{
    class Minigame 
    {
        Random random = new Random();
        private int diff;
        public Minigame(int idiff){ 
            diff = idiff;
        }
        public bool MathQuestion(){
            int a = random.Next(-diff, diff);
            int b = random.Next(-diff, diff);
            Console.WriteLine($"What is {a} + {b}");
            if (Console.ReadLine() == $"{a + b}") {
                return true;
            }
            return false;
        }

        public bool DiceRoll() {
            int result = random.Next(1, 25);
            Console.WriteLine($"Time for a Dice roll \n You will need : {diff} \n You rolled : {result}");
            if (diff < result) {
                return true;
            }
            return false;     
        }
        
        //Perhaps keyboard input...


    }
    class minigameWrapper
    {
        Random random = new Random();
        Minigame current;
        MethodInfo methodInfo;
        public minigameWrapper(int diff) {
            string minigame = "";
            Random random = new Random();
            if (random.Next(0, 2) == 0) {
                minigame = "DiceRoll";
            } else {
                minigame = "MathQuestion";
            }
            current = new Minigame(diff);
            methodInfo = typeof(Minigame).GetMethod(minigame);
        }

        //Work In Progress
        public bool checkResult() {
            if (methodInfo == null) {
                Console.WriteLine("Free Square");
                return true;
            } 
            dynamic response = methodInfo.Invoke(current, null);
            return response;
        }

    }
}