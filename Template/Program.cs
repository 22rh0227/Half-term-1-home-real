using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
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
                case "q1": Q1(); break;
                case "q2": Q2(); break;
                case "q3": Q3(); break;
                case "q4": Q4(); break;
                case "q5": Q5(); break;
            }
        }
        static void Q1()
        {
            Random rnd = new Random();
            Console.WriteLine("how many sides would you like your die to have?");
            int sides = int.Parse(Console.ReadLine());
            Console.WriteLine("how many times would you like to roll your die?");
            int amountOfRolls = int.Parse(Console.ReadLine());

            for (int i = 0; i < amountOfRolls; i++)
            {
                Console.WriteLine(rnd.Next(1, sides + 1));
            }
        }
        static void Q2()
        {

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
