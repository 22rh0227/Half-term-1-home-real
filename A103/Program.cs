using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A103
{
    internal class Program
    {
        static void Menu()
        {
            Console.WriteLine("Which question would you like? (type just the number)");
            string questionChoice = Console.ReadLine().ToLower();
            Console.Clear();

            switch (questionChoice)
            {
                case "1": Q1(); break;
                case "2": Q2(); break;
            }
        }
        static void Q1()
        {
            Console.WriteLine("what is your number");
            int num = int.Parse(Console.ReadLine());
            int counter = 0;

            while (counter <= num)
            {
                Console.WriteLine(counter);
                counter++;
            }
        }
        static void Q2()
        {
            Console.WriteLine("choose a power multiple");
            int num = int.Parse(Console.ReadLine());
            Console.WriteLine("choose a power maximum");
            int power = int.Parse(Console.ReadLine());


            for (int i = 0; i <= power; i++)
            {
                if (i % 2 == 0) { Console.ForegroundColor = ConsoleColor.Blue; }
                else { Console.ForegroundColor = ConsoleColor.White; }
                if (i < 10) { Console.WriteLine(num + " ^ " + i + "  = " + Math.Pow(num, i)); }
                else { Console.WriteLine(num + " ^ " + i + " = " + Math.Pow(num, i)); }
            }
        }
        static void Main(string[] args)
        {
            Menu();
            Console.ReadKey();
        }
    }
}
