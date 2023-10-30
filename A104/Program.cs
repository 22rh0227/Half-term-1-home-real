using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A104
{
    internal class Program
    {
        static void Menu()
        {
            Console.WriteLine("Which question would you like? Q1, Q2, Q3, Q4, Q5");
            string questionChoice = Console.ReadLine().ToLower();
            Console.Clear();

            switch (questionChoice)
            {
                case "1": Q1(); break;
                case "2": Q2(); break;
                case "3": Q3(); break;
                case "4": Q4(); break;
                case "5": Q5(); break;
            }
        }
        static void Q1()
        {
            Console.WriteLine("welcome to the n sided die");
            Console.WriteLine();
            Random rnd = new Random();
            Console.WriteLine("how many sides would you like your die to have?");
            int sides = int.Parse(Console.ReadLine());
            Console.WriteLine("how many times would you like to roll your die?");
            int amountOfRolls = int.Parse(Console.ReadLine());
            Console.WriteLine("here are your rolls:");

            for (int i = 0; i < amountOfRolls; i++)
            {
                Console.WriteLine(rnd.Next(1, sides + 1));
            }
        }
        static void Q2()
        {
            Console.WriteLine("welcome to sum or factorial");
            Console.WriteLine();

            Console.WriteLine("enter a number");
            int num = int.Parse(Console.ReadLine());
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("would you like to sum all numbers up to " + num + " or find the factorial of " + num);
                Console.WriteLine("enter s for sum or f for factorial");

                switch (Console.ReadLine().ToLower())
                {
                    case "s":
                        Sum(num);
                        break;

                    case "f":
                        Factorial(num);
                        break;

                    default: Console.WriteLine(); continue;
                }
                break;
            }
        }
        static void Sum(int max)
        {
            int total = 0;
            for (int i = 1; i <= max; i++) { total += i; }
            Console.WriteLine(total);
        }
        static void Factorial(int max)
        {
            int total = 1;
            for (int i = 1; i <= max; i++) { total *= i; }
            Console.WriteLine(total);
        }
        static void Q3()
        {

        }
        static void Q4()
        {

        }
        static void Q5()
        {

        }
        static void Main(string[] args)
        {
            Menu();
            Console.ReadKey();
        }
    }
}
