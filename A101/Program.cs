using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A101
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //2
            int first;
            int second;

            Console.WriteLine("what is the first number");
            first = int.Parse(Console.ReadLine());

            Console.WriteLine("what is the second number");
            second = int.Parse(Console.ReadLine());

            if (first > second)
            {
                Console.WriteLine("first");
            }
            else if (first < second) 
            {
                Console.WriteLine("second");
            }
            else { Console.WriteLine("these numbers are equal"); }

            //3
            int num;

            Console.WriteLine("what is the number");
            num = int.Parse(Console.ReadLine());

            if (num > 999 & num < 5001)
            {
                Console.WriteLine("CORRECT!");
            }
            else if (num < 1000)
            {
                Console.WriteLine("too small");
            }
            else { Console.WriteLine("too big"); }

            //3
            float radius;
            int slicesTotal;
            int slicesEaten;
            int height = 2;
            double pi = Math.PI;

            Console.WriteLine("what is the diameter");
            radius = float.Parse(Console.ReadLine()) / 2;

            Console.WriteLine("how many slices total");
            slicesTotal = int.Parse(Console.ReadLine());

            Console.WriteLine("how many slices eaten");
            slicesEaten = int.Parse(Console.ReadLine());

            Console.WriteLine(pi * radius * radius * height * slicesEaten / slicesTotal + "cm^3 eaten");












            Console.ReadKey();
            Console.ReadKey();
        }
    }
}
