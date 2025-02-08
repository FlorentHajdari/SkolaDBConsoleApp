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
                Console.WriteLine("4. Visa alla aktiva kurser");
                Console.WriteLine("5. Visa antal lärare per avdelning");
                Console.WriteLine("6. Visa detalj-information om samtliga elever");
                Console.WriteLine("7. Visa information om all personal");
                Console.WriteLine("8. Visa alla betyg för en enskild elev");
                Console.WriteLine("9. Visa lönekostander för respektive avdelning");
                Console.WriteLine("10. Visa medellön för respektive avdelning");
                Console.WriteLine("11. Sätt betyg på en elev med hjälp av en transaktion");
                Console.WriteLine("12. Visa information för specifik elev med en Stored Procedure");
                Console.WriteLine("13. Avsluta programmet");
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
                        metoder.GetAllActiveCourses();
                        break;
                    case "5":
                        metoder.GetStaffByDepartment();
                        break;
                    case "6":
                        metoder.GetAllStudentDetails();
                        break;
                    case "7":
                        metoder.GetAllStaffOverview();
                        break;
                    case "8":
                        metoder.GetGradesForStudent();
                        break;
                    case "9":
                        metoder.GetDepartmentSalaryCosts();
                        break;
                    case "10":
                        metoder.GetAverageSalariesByDepartment();
                        break;
                    case "11":
                        metoder.SetGradeWithTransaction();
                        break;

                    case "12":
                        metoder.GetStudentInfoById();
                        break;

                    case "13":
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
