using System;

namespace Pharmacy
{
    /// <summary>
    /// Main menu for Pharmacy Helper
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string command;
            do
            {
                Console.Clear();
                ConsoleEx.WriteLine("Choose desired option:", ConsoleColor.Blue);
                ConsoleEx.WriteLine("1. Medicines", ConsoleColor.Blue);
                ConsoleEx.WriteLine("2. Prescriptions", ConsoleColor.Blue);
                ConsoleEx.WriteLine("3. Orders", ConsoleColor.Blue);
                ConsoleEx.WriteLine("4. Exit", ConsoleColor.Blue);
                command = Console.ReadLine();

                if (command == "med" || command == "1")
                {
                    Console.Clear();
                    Medicine.Choice();
                }

                if (command == "pres" || command == "2")
                {
                    Console.Clear();
                    Prescription.Choice();
                }

                if (command == "order" || command == "3")
                {
                    Console.Clear();
                    Order.Choice();
                }

            } while (command != "exit" && command != "4");

        }
    }
}
