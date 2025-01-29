using System;
using System.Linq;
using SkolaDBConsoleApp.models;
using Microsoft.EntityFrameworkCore;

namespace SkolaDBConsoleApp

{
    internal class Program
    {
        static void Main(string[] args)
        {
            var metoder = new Metoder();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Välj en funktion: ");
                Console.WriteLine("1. Hämta alla elever");
                Console.WriteLine("2. Hämta alla elever från en specifik klass");
                Console.WriteLine("3. Lägga till ny personal");
                Console.WriteLine("4. Avsluta programmet");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Vill du sortera på förnamn eller efternamn? (Mata in 'förnamn' alternativ 'efternamn')");
                        var sortBy = Console.ReadLine();
                        Console.WriteLine("Vill du ha sorteringen från A-Ö eller Ö-A? (Mata in 'stigande' eller 'fallande')");
                        var sortOrder = Console.ReadLine();
                        metoder.GetAllStudents(sortBy, sortOrder);
                        break;

                    case "2":
                        metoder.GetStudentsByClass();
                        break;

                    case "3":
                        metoder.AddNewStaff();
                        break;

                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Gör om och gör rätt! ");
                        break;
                }
            }
        }
    }
}
