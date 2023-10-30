using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fighting
{
    internal class Program
    {
        static int[] playerStats = { 10, 10, 100, 1 };
        static Random rnd = new Random();
        static int[] Goblin()
        {
            Console.WriteLine("[insert ascii art here]");
            Console.WriteLine("[insert ascii art here]");
            Console.WriteLine("[insert ascii art here]");
            Console.WriteLine("[insert ascii art here]");
            Console.WriteLine("[insert ascii art here]");
            int[] stats = { rnd.Next(15, 31), rnd.Next(5, 16), rnd.Next(1, 21) };

            return stats;
        }
        static int[] Demon()
        {
            Console.WriteLine("         ,     .\r\n        /(     )\\               A\r\n   .--.( `.___.' ).--.         /_\\\r\n   `._ `%_&%#%$_ ' _.'     /| <___> |\\\r\n      `|(@\\*%%/@)|'       / (  |L|  ) \\\r\n       |  |%%#|  |       J d8bo|=|od8b L\r\n        \\ \\$#%/ /        | 8888|=|8888 |\r\n        |\\|%%#|/|        J Y8P\"|=|\"Y8P F\r\n        | (.\".)%|         \\ (  |L|  ) /\r\n    ___.'  `-'  `.___      \\|  |L|  |/\r\n  .'#*#`-       -'$#*`.       / )|\r\n /#%^#%*_ *%^%_  #  %$%\\    .J (__)\r\n #&  . %%%#% ###%*.   *%\\.-'&# (__)\r\n %*  J %.%#_|_#$.\\J* \\ %'#%*^  (__)\r\n *#% J %$%%#|#$#$ J\\%   *   .--|(_)\r\n |%  J\\ `%%#|#%%' / `.   _.'   |L|\r\n |#$%||` %%%$### '|   `-'      |L|\r\n (#%%||` #$#$%%% '|            |L|\r\n | ##||  $%%.%$%  |            |L|\r\n |$%^||   $%#$%   |  VK/cf     |L|\r\n |&^ ||  #%#$%#%  |            |L|\r\n |#$*|| #$%$$#%%$ |\\           |L|\r\n ||||||  %%(@)$#  |\\\\          |L|\r\n `|||||  #$$|%#%  | L|         |L|\r\n      |  #$%|$%%  | ||l        |L|\r\n      |  ##$H$%%  | |\\\\        |L|\r\n      |  #%%H%##  | |\\\\|       |L|\r\n      |  ##% $%#  | Y|||       |L|\r\n      J $$#* *%#% L  |E/\r\n      (__ $F J$ __)  F/\r\n      J#%$ | |%%#%L\r\n      |$$%#& & %%#|\r\n      J##$ J % %%$F\r\n       %$# * * %#&\r\n       %#$ | |%#$%\r\n       *#$%| | #$*\r\n      /$#' ) ( `%%\\\r\n     /#$# /   \\ %$%\\\r\n    ooooO'     `Ooooo");
            int[] stats = { rnd.Next(10, 16), rnd.Next(1, 5), rnd.Next(17, 23) };
            return stats;
        }
        static int[] Skeleton()
        {
            Console.WriteLine("                              _.--\"\"-._\r\n  .                         .\"         \".\r\n / \\    ,^.         /(     Y             |      )\\\r\n/   `---. |--'\\    (  \\__..'--   -   -- -'\"\"-.-'  )\r\n|        :|    `>   '.     l_..-------.._l      .'\r\n|      __l;__ .'      \"-.__.||_.-'v'-._||`\"----\"\r\n \\  .-' | |  `              l._       _.'\r\n  \\/    | |                   l`^^'^^'j\r\n        | |                _   \\_____/     _\r\n        j |               l `--__)-'(__.--' |\r\n        | |               | /`---``-----'\"1 |  ,-----.\r\n        | |               )/  `--' '---'   \\'-'  ___  `-.\r\n        | |              //  `-'  '`----'  /  ,-'   I`.  \\\r\n      _ L |_            //  `-.-.'`-----' /  /  |   |  `. \\\r\n     '._' / \\         _/(   `/   )- ---' ;  /__.J   L.__.\\ :\r\n      `._;/7(-.......'  /        ) (     |  |            | |\r\n      `._;l _'--------_/        )-'/     :  |___.    _._./ ;\r\n        | |                 .__ )-'\\  __  \\  \\  I   1   / /\r\n        `-'                /   `-\\-(-'   \\ \\  `.|   | ,' /\r\n                           \\__  `-'    __/  `-. `---'',-'\r\n                              )-._.-- (        `-----'\r\n                             )(  l\\ o ('..-.\r\n                       _..--' _'-' '--'.-. |\r\n                __,,-'' _,,-''            \\ \\\r\n               f'. _,,-'                   \\ \\\r\n              ()--  |                       \\ \\\r\n                \\.  |                       /  \\\r\n                  \\ \\                      |._  |\r\n                   \\ \\                     |  ()|\r\n                    \\ \\                     \\  /\r\n                     ) `-.                   | |\r\n                    // .__)                  | |\r\n                 _.//7'                      | |\r\n               '---'                         j_| `\r\n                                            (| |\r\n                                             |  \\\r\n                                             |lllj\r\n                                             |||||");
            int[] stats = { rnd.Next(5, 11), rnd.Next(3, 8), rnd.Next(15, 21) };

            return stats;
        }
        static int[] GetEnemy(string enemy)
        {
            int[] stats = new int[3];
            switch (enemy)
            {
                case "skeleton": stats = Skeleton(); break;

                case "goblin": stats = Goblin(); break;

                case "demon": stats = Demon(); break;

                default: Console.WriteLine("somethings wrong"); break;
            }

            return stats;
        }
        static void BattleMenu(string enemy, int[] enemyStats)
        {
            int cursorRow = 1;
            int cursorCol = 90;
            Console.SetCursorPosition(cursorCol, 0);
            Console.WriteLine(enemy + " is opposing you. What will you do");
            Console.SetCursorPosition(cursorCol, 1);
            Console.WriteLine("  Attack");
            Console.SetCursorPosition(cursorCol, 2);
            Console.WriteLine("  Potions");
            Console.SetCursorPosition(cursorCol, 3);
            Console.WriteLine("  Inventory");
            Console.SetCursorPosition(cursorCol, 4);
            if (playerStats[3] == 1) { Console.WriteLine("  Magic"); }
            Console.SetCursorPosition(cursorCol, cursorRow);
            Console.Write(">");


            while (true)
            {
                ConsoleKeyInfo direction = Console.ReadKey(true);
                if (direction.Key == ConsoleKey.DownArrow && (cursorRow < 3 || playerStats[3] == 1 && cursorRow < 4))
                {
                    Console.SetCursorPosition(cursorCol, cursorRow);
                    Console.Write(" ");
                    cursorRow++;
                    Console.SetCursorPosition(cursorCol, 20);
                    Console.WriteLine(cursorRow);
                    Console.SetCursorPosition(cursorCol, cursorRow);
                    Console.Write(">");
                }
                else if (direction.Key == ConsoleKey.UpArrow && cursorRow > 1)
                {
                    Console.SetCursorPosition(cursorCol, cursorRow);
                    Console.Write(" ");
                    cursorRow--;
                    Console.SetCursorPosition(cursorCol, 20);
                    Console.WriteLine(cursorRow);
                    Console.SetCursorPosition(cursorCol, cursorRow);
                    Console.Write(">");
                }
                else if (direction.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    GetEnemy(enemy);
                    break;

                }
            }
            if (cursorRow == 1) { Attack(enemyStats); }
            else if (cursorRow == 2) { PotionMenu(); }
            else if (cursorRow == 3) { Inventory(); }
            else { Magic(); }

        }
        static void Attack(int[] enemyStats)
        {
            int die = rnd.Next(1, 21);
            if (die == 1) { playerStats[2] -= playerStats[0] - playerStats[1]; }
            else if (die > 1 && die < 8) { enemyStats[2] -= playerStats[0] / 2 - enemyStats[1]; }
            else if (die > 7 && die < 14) { enemyStats[2] -= playerStats[0] - enemyStats[1]; }
            else if (die > 13 && die < 20) { enemyStats[2] -= (int)(playerStats[0] * 7 / 4) - enemyStats[2]; }
            else { enemyStats[2] -= playerStats[0] * 2 - enemyStats[1]; }
        }
        static void PotionMenu() { }
        static void Inventory() { }
        static void Magic() { }
        static void Fight(string enemy)
        {
            Console.ReadKey();
            Console.Clear();
            int[] enemyStats = GetEnemy(enemy);

            BattleMenu(enemy, enemyStats);

        }
        static void Main(string[] args)
        {
            string[] enemyArray = { "skeleton" };
            Fight("demon");
            Console.ReadKey();
        }
    }
}
