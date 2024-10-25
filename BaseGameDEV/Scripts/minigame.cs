using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using PlayerEnd;
using System.Text.Json;
using System.Collections.Concurrent;
namespace minigame
{
  class Minigame
  {
    Random random = new Random();
    private int diff;
    public Minigame(int idiff)
    {
      diff = idiff;
    }
    public bool MathQuestion()
    {
      int a = random.Next(-diff, diff);
      int b = random.Next(-diff, diff);
      Console.WriteLine($"What is {a} + {b}");
      if (Console.ReadLine() == $"{a + b}")
      {
        return true;
      }
      return false;
    }

    public bool DiceRoll()
    {
      int result = random.Next(1, 25);
      Console.WriteLine($"Time for a Dice roll \n You will need : {diff} \n You rolled : {result}");
      if (diff < result)
      {
        return true;
      }
      return false;
    }

    //Perhaps keyboard input...
    public bool MazeGame()
    {
      String[,] x = new String[8, 8];
      bool hassword = false;
      int posx = 0;
      int posy = 7;
      int swordx = random.Next(0, 8);
      int swordy = random.Next(0, 4);
      int demnx = random.Next(1, 7);
      int demny = random.Next(0, 6);
      Console.WriteLine("Welcome to the Maze! In this place a demon lurks. Find the sword and slay the demon, before the demon catches you!");
      for (int i = 0; i < 8; i++)
      {
        for (int j = 0; j < 8; j++)
        {
          if (i == posy && j == posx)
          {
            x[posx, posy] = "x";
            Console.Write(x[j, i] + " ");
            continue;
          }
          x[j, i] = "o";
          Console.Write(x[j, i] + " ");
        }
        Console.WriteLine();
      }

      Console.WriteLine();
      while (true)
      {
        x[posx, posy] = "o";
        int rand = random.Next(1, 9);
        int move = random.Next(1, 3);// demon can move 1 or 2 tiles
        ConsoleKeyInfo keyInfo = Console.ReadKey(true); // Read key press
        ConsoleKey key = keyInfo.Key;
        if (key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow)
        {
          Console.WriteLine("Invalid input. Use arrow keys to move");
          continue;
        }
        if (key == ConsoleKey.UpArrow)
        {
          posy--;
        }
        else if (key == ConsoleKey.RightArrow)
        {
          posx++;
        }
        else if (key == ConsoleKey.LeftArrow)
        {
          posx--;
        }
        else if (key == ConsoleKey.DownArrow)
        {
          posy++;
        }
        if (posy >= 8)
        {
          Console.WriteLine("dead end");
          posy = 7;
          continue;
        }
        if (posy <= -1)
        {
          Console.WriteLine("dead end");
          posy = 0;
          continue;
        }
        if (posx >= 8)
        {
          Console.WriteLine("dead end");
          posx = 7;
          continue;
        }
        if (posx <= -1)
        {
          Console.WriteLine("dead end");
          posx = 0;
          continue;
        }

        if (rand == 1)
        {
          demnx += move;
        }
        if (rand == 2)
        {
          demnx -= move;
        }
        if (rand == 3)
        {
          demny += move;
        }
        if (rand == 4)
        {
          demny -= move;
        }
        if (rand == 5)
        {
          demny += move;
          demnx += move;
        }
        if (rand == 6)
        {
          demny -= move;
          demnx -= move;
        }
        if (rand == 7)
        {
          demny -= move;
          demnx += move;
        }
        if (rand == 8)
        {
          demny += move;
          demnx -= move;
        }
        if (demnx >= 8)
        {
          demnx = 7;
        }
        if (demny >= 8)
        {
          demny = 7;
        }
        if (demnx <= -1)
        {
          demnx = 0;
        }
        if (demny <= -1)
        {
          demny = 0;
        }
        x[posx, posy] = "x";
        for (int i = 0; i < 8; i++)
        {
          for (int j = 0; j < 8; j++)
          {
            Console.Write(x[j, i] + " ");
          }
          Console.WriteLine();
        }
        Console.WriteLine();
        if (posx == swordx && posy == swordy && !hassword)
        {
          hassword = true;
          Console.WriteLine("You found the sword. Find the Demon and kill it!!!!");
        }
        if (posx == demnx && posy == demny && hassword)
        {
          Console.WriteLine("You killed the demon!!! You may move on to the next room.");
          return true;
        }
        else if (posx == demnx && posy == demny && !hassword)
        {
          Console.WriteLine("The demon caught you!! You died");
          return false;
        }
        Console.WriteLine("Demon is " + (Math.Abs(posx - demnx)) + " tiles away horizontally and " + (Math.Abs(posy - demny)) + " tiles away vertically");
      }
    }

    public Dictionary<int, (string, int)> storelist = new Dictionary<int, (string, int)> // Current possible room state
    {
      [1] = ("Strength Potion", 3),
      [2] = ("Health Potion", 3),
      [3] = ("Speed Potion", 3),
      [4] = ("Defense Potion", 3),
      [5] = ("Key", 10),
      [6] = ("Rock", 1)
    };

    const int shopsize = 3;
    public (string, int) Shop(int coins)
    {
      List<int> itemnums = new List<int>(); //Creates a list of numbers corresponding to what shop items will be in the shop
      for (int n = 0; n < shopsize; n++)
      {
        int itemnum = random.Next(1, storelist.Count);
        while (itemnums.Contains(itemnum))
        {
          itemnum = random.Next(1, storelist.Count);
        }
        itemnums.Add(itemnum);
      }

      Console.WriteLine($"Welcome to the shop!\nYou can buy:");
      for (int n = 0; n < itemnums.Count; n++)
      {
        Console.WriteLine($"Item {n}: " + storelist[itemnums[n]].Item1 + " Cost: " + storelist[itemnums[n]].Item2);
      }
      Console.WriteLine($"Enter the number of the item you'd like below");
      string entered = Console.ReadLine();
      for (int n = 0; n < itemnums.Count; n++)
      {
        if (entered == $"{n}" && coins >= storelist[itemnums[n]].Item2)
        {
          Console.WriteLine($"You bought " + storelist[itemnums[n]].Item1);
          Console.WriteLine($"The shop is now closed");
          return (storelist[itemnums[n]].Item1, storelist[itemnums[n]].Item2);
        }
        else if (entered == $"{n}" && coins < storelist[itemnums[n]].Item2)
        {
          Console.WriteLine($"You are " + storelist[itemnums[n]].Item2 + " coins short");
          return (null, 0);
        }
        else if (n + 1 == itemnums.Count)
        {
          Console.WriteLine($"You decide to not buy any items");
          return (null, 0);
        }
      }
      return (null, 0);
    }
    public (string, int) Treasure(bool instatus)
    {
      if (!instatus)
      {
        string treasurefound = storelist[random.Next(1,storelist.Count)].Item1;
        int coinsfound = random.Next(1,5);
        Console.WriteLine($"You found a {treasurefound}!");
        Console.WriteLine($"You also found {coinsfound} coins!");
        return (treasurefound, coinsfound);
      }
      else
      {
        Console.WriteLine("There was nothing left...");
        return (null, 0);
      }
    }
  }


  class minigameWrapper
  {


    Random random = new Random();
    Minigame current;
    MethodInfo methodInfo;
    bool roomstatus;
    public minigameWrapper(string command, int diff, bool instate)
    {
      current = new Minigame(diff);
      methodInfo = typeof(Minigame).GetMethod(command);
      roomstatus = instate;
    }

    //Work In Progress
    public bool checkResult()
    {
      if (methodInfo == null)
      {
        Console.WriteLine("Free Square");
        return true;
      }
      dynamic response = methodInfo.Invoke(current, null);
      return response;
    }
    public (string, int) shopResult(int coins) // Code for handling what happens with the shop
    {
      return current.Shop(coins);
    }
    public (string, int) treasureResult(bool roomstate) {
      return current.Treasure(roomstate);
    }
  }
}
