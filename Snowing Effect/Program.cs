using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowing_Effect
{
    //public struct LayerPoint
    //{
    //    public int f, s, t;
    //}
    internal class Program
    {
        
        
        static void Main(string[] args)
        {
            Console.ReadKey(true);
            Console.CursorVisible = false;
            List<Snow.Snowflake> snowflakes = new List<Snow.Snowflake>();
            List<List<int>> snowLevels = new List<List<int>>();
            List<int> highestLevels = new List<int>();
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.White;
            for (int j = 0; j < 3; j++)
            {
                snowLevels.Add(new List<int>());
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    if (j == 0)
                    {
                        highestLevels.Add(Console.WindowHeight - 1);
                        Console.Write('█');
                    }
                    snowLevels[j].Add(Console.WindowHeight - 1);
                }
            }
            for (int i = 0; i < 100; i++)
            {
                snowflakes.Add(Snow.RandomFlake());
            }
            while (true)
            {
                for (int i = 0; i < snowflakes.Count; i++)
                {
                    snowflakes[i] = Snow.IsFalling(snowflakes[i], snowLevels, highestLevels);
                }
                System.Threading.Thread.Sleep(100);
            }
            //Console.ReadLine();
        }
    }
}
