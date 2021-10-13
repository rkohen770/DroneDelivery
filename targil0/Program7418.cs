using System;

namespace targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome7418();
            welcome2156();
            Console.ReadKey();
        }

        private static void welcome7418()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
        static partial void welcome2156();
    }
}
