using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowing_Effect
{
    public class Snow
    {
        private static Random rnd = new Random();
        public struct Snowflake
        {
            public int x, y, v, layer;
            public ConsoleColor color;
            public bool falling;
            public char symbol;
        }
        public static Snowflake RandomFlake()
        {
            Snowflake flake = new Snowflake();
            flake.x = rnd.Next(0, Console.WindowWidth);
            flake.v = rnd.Next(2, 9);
            flake.y = flake.v - 1;
            flake.layer = rnd.Next(0, 6);
            if (flake.layer == 0)
            {
                flake.color = ConsoleColor.White;
            }
            else if (flake.layer < 3)
            {
                flake.layer = 1;
                flake.color = ConsoleColor.Gray;
            }
            else
            {
                flake.layer = 2;
                flake.color = ConsoleColor.DarkGray;
            }
            int storeRnd = rnd.Next(0, 4);
            if (storeRnd == 0)
            {
                flake.symbol = '*';
            }
            else if (storeRnd == 1)
            {
                flake.symbol = '0';
            }
            else if (storeRnd == 2)
            {
                flake.symbol = '#';
            }
            else
            {
                flake.symbol = 'o';
            }
            flake.falling = true;
            PrintSnowFlakes(flake);
            return flake;
        }
        private static void PrintSnowFlakes(Snowflake flake)
        {
            Console.ForegroundColor = flake.color;
            Console.SetCursorPosition(flake.x, flake.y);
            Console.WriteLine(flake.symbol);
        }
        public static Snowflake IsFalling(Snowflake flake, List<List<int>> snowLevels, List<int> highestLevels)
        {
            Console.ForegroundColor = flake.color;
            Console.SetCursorPosition(flake.x, flake.y);
            Console.WriteLine(' ');
            if (flake.y + flake.v >= highestLevels[flake.x])
            {
                while (flake.x != 0 && snowLevels[flake.layer][flake.x] < snowLevels[flake.layer][flake.x - 1] - 1)
                {
                    flake.x--;
                }
                while (flake.x != Console.WindowWidth - 1 && snowLevels[flake.layer][flake.x] < snowLevels[flake.layer][flake.x + 1] - 1)
                {
                    flake.x++;
                }
                snowLevels[flake.layer][flake.x]--;
                if (snowLevels[flake.layer][flake.x] < highestLevels[flake.x])
                {
                    highestLevels[flake.x] = snowLevels[flake.layer][flake.x];
                }
                if (flake.layer != 0)
                {
                    if (snowLevels[flake.layer][flake.x] >= snowLevels[flake.layer - 1][flake.x])
                    {
                        return flake;
                    }
                }
                if (flake.layer == 2)
                {
                    if (snowLevels[2][flake.x] >= snowLevels[0][flake.x])
                    {
                        return flake;
                    }
                }
                
                Console.SetCursorPosition(flake.x, snowLevels[flake.layer][flake.x]);
                Console.Write('█');
                flake = RandomFlake();
            }
            else
            {
                //Console.WriteLine();
                //Console.WriteLine(": " + flake.y);
                flake.y += flake.v;
                //Console.WriteLine(": " + flake.y);
                //Console.ReadKey();
                Console.SetCursorPosition(flake.x, flake.y);
                Console.WriteLine(flake.symbol);
            }
            return flake;
        }
    }
}
