using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Myself
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            //based vairables
            int baseDecimal;
            int rndStorage;
            int baseStarter;
            string base10;
            string base2 = "";
            string base16 = "";

            //getting random number
            rndStorage = rnd.Next(0, 256);
            baseDecimal = rndStorage;
            base10 = baseDecimal.ToString();
            Console.WriteLine(baseDecimal);

            //baseDecimal to base2
            for (int i = 128; i != 0; i /= 2)
            {

                if (baseDecimal >= i)
                {
                    base2 += '1';
                    baseDecimal -= i;
                }

                else
                {
                    base2 += '0';
                }

                if (i == 1)
                {
                    i = 0;
                }
            }

            Console.WriteLine("binary convertion: " + base2);

            //baseDecimal to base16
            baseDecimal = rndStorage;
            if (baseDecimal > 15)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (baseDecimal < (j + 1) * 16)
                    {
                        if (j > 9)
                        {
                            base16 += Convert.ToChar(j + 55);
                        }
                        else { base16 += j; }

                        baseDecimal -= j * 16;
                        if (baseDecimal > 9)
                        {
                            base16 += Convert.ToChar(baseDecimal + 55);
                        }
                        else { base16 += baseDecimal; }

                        j = 16;
                    }
                }
            }
            
            else if (baseDecimal > 9)
            {
                base16 += Convert.ToChar(baseDecimal + 55);
            }

            else { base16 += baseDecimal; }

            Console.WriteLine(base16);


            //gameshow time
            baseStarter = rnd.Next(1, 4);
            if (baseStarter == 1)
            {
                Console.WriteLine(base10);
                Console.WriteLine("what is this decimal number in binary?");
                if (base2 == Console.ReadLine())
                {
                    Console.WriteLine("congratulations!!! this is the correct answer");
                }
                else { Console.WriteLine("dude wtf, no its " + base2); }

                Console.WriteLine("what is this decimal number in hexadecimal?");
                if (base16 == Console.ReadLine())
                {
                    Console.WriteLine("congratulations!!! this is the correct answer");
                }
                else { Console.WriteLine("dude wtf, no its " + base16); }
            }

            else if (baseStarter == 2)
            {
                Console.WriteLine(base2);
                Console.WriteLine("what is this binary number in decimal?");
                if (base10 == Console.ReadLine())
                {
                    Console.WriteLine("congratulations!!! this is the correct answer");
                }
                else { Console.WriteLine("dude wtf, no its " + base10); }

                Console.WriteLine("what is this decimal number in hexadecimal?");
                if (base16 == Console.ReadLine())
                {
                    Console.WriteLine("congratulations!!! this is the correct answer");
                }
                else { Console.WriteLine("dude wtf, no its " + base16); }
            }

            else
            {
                Console.WriteLine(base16);
                Console.WriteLine("what is this hexadecimal number in binary?");
                if (base2 == Console.ReadLine())
                {
                    Console.WriteLine("congratulations!!! this is the correct answer");
                }
                else { Console.WriteLine("dude wtf, no its " + base2); }

                Console.WriteLine("what is this hexadecimal number in decimal?");
                if (base10 == Console.ReadLine())
                {
                    Console.WriteLine("congratulations!!! this is the correct answer");
                }
                else { Console.WriteLine("dude wtf, no its " + base10); }
            }

            Console.ReadKey();
        }
    }
}
