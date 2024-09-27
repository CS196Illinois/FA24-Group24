using System.Dynamic;
using System.Net;
using System.Reflection;
namespace minigame
{
    class minigame 
    {
        Random random = new Random();
        private int diff;
        public minigame(int idiff){ 
            diff = idiff;
        }
        public bool MathQuestion(){
            int a = random.Next(-(int) Math.Pow(2, diff), (int) Math.Pow(2, diff));
            int b = random.Next(-(int) Math.Pow(2, diff), (int) Math.Pow(2, diff));
            Console.WriteLine($"What is {a} * {b}");
            if (Console.ReadLine() == $"{a * b}") {
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
        minigame current;
        MethodInfo methodInfo;
        public minigameWrapper(string command, int diff) {
            current = new minigame(diff);
            methodInfo = typeof(minigame).GetMethod(command);
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