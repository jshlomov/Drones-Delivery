using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome1696();
            welcome7162();
            Console.ReadKey();

        }

        static partial void welcome7162();

        private static void welcome1696()

        {
            Console.WriteLine("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application");
        }
    }
}
