using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SkolaDBConsoleApp.Models;

namespace SkolaDBConsoleApp
{
    public class Metoder
    {
        public void GetAllStudents(string sortBy, string sortOrder)
        {
            using (var context = new SkolaDbContext())
            {
                IQueryable<Elever> students = context.Elevers;

                if (sortBy.ToLower() == "förnamn")
                {
                    students = sortOrder.ToLower() == "stigande"
                    ? students.OrderBy(e => e.Förnamn)
                    : students.OrderByDescending(e => e.Förnamn);
                }
                else if (sortBy.ToLower() == "efternamn")
                {
                    students = sortOrder.ToLower() == "stigande"
                        ? students.OrderBy(e => e.Efternamn)
                        : students.OrderByDescending(e => e.Efternamn);
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Visar därför en osorterad lista av eleverna. ");
                }
                var studentList = students.Include(e => e.KlassNavigation).ToList();

                foreach (var student in studentList)
                {
                    Console.WriteLine($"{student.Förnamn} {student.Efternamn} - Klass: {student.KlassNavigation.Namn}");
                }
            }
        }

        public void GetStudentsByClass()
        {
            using (var context = new SkolaDbContext())
            {
                var classes = context.Klassers.ToList();
                Console.WriteLine("Välj en klass: ");
                foreach (var classItem in classes)
                {
                    Console.WriteLine($"{classItem.Id}. {classItem.Namn}");
                }

                var classChoice = int.Parse(Console.ReadLine());
                var students = context.Elevers.Where(e => e.Klass == classChoice).Include(e => e.KlassNavigation). ToList();

                foreach (var student in students)
                {
                    Console.WriteLine($"{student.Förnamn} {student.Efternamn} - Klass: {student.KlassNavigation.Namn}");
                }
            }
        }

        public void AddNewStaff()
        {
            using (var context = new SkolaDbContext())
            {
                Console.WriteLine("Ange förnamn: ");
                var firstName = Console.ReadLine();
                Console.WriteLine("Ange efternamn: ");
                var lastName = Console.ReadLine();
                Console.WriteLine("Ange befattning: ");
                var position = Console.ReadLine();

                var newStaff = new Personal
                {
                    Förnamn = firstName,
                    Efternamn = lastName,
                    Befattning = position
                };

                context.Personals.Add(newStaff);
                context.SaveChanges();

                Console.WriteLine("Ny personal är tillagd! ");
            }
        }
    }
}
