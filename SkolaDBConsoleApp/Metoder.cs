using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
                var students = context.Elevers.Where(e => e.Klass == classChoice).Include(e => e.KlassNavigation).ToList();

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

        public void GetAllActiveCourses()
        {
            using (var context = new SkolaDbContext())
            {
                var courses = context.Ämnens.ToList();

                foreach (var course in courses)
                {
                    Console.WriteLine($"Kurs: {course.Namn}");
                }
            }
        }

        public void GetStaffByDepartment()
        {
            using (var context = new SkolaDbContext())
            {
                var staffByDept = context.Personals
                .GroupBy(p => p.Avdelning)
                .Select(group => new
                {
                    Department = group.Key,
                    StaffCount = group.Count()
                }).ToList();

                foreach (var item in staffByDept)
                {
                    Console.WriteLine($"Avdelning: {item.Department}, Antal: {item.StaffCount}");
                }
            }
        }

        public void GetAllStaffOverview()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT Förnamn, Efternamn, Befattning, (EXTRACT(YEAR FROM AGE(NOW(), Anställningsdatum))) AS År FROM personal", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Förnam"]} {reader["Efternamn"]} - Befattning: {reader["Befattning"]} - År: {reader["År"]}");
                    }
                }
            }
        }

        public void GetGradesForStudent()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Ange elevens ID: ");
                var studentID = Console.ReadLine();
                var command = new NpgsqlCommand("SELECT * FROM betyg WHERE elev = @elevId", connection);
                command.Parameters.AddWithValue("@elevId", studentID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Betyg: {reader["betyg"]}, Ämne: {reader["ämne"]}, Datum: {reader["datum"]}, Lärare: {reader["lärare"]}");
                    }
                }
            }
        }

        public void GetDepartmentSalaryCosts()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT befattning, SUM(lön) AS TotalLön FROM personal GROUP BY befattning", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Befattning: {reader["befattning"]}, Total Lön: {reader["TotalLön"]}");
                    }
                }
            }
        }

        public void GetAverageSalariesByDepartment()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT befattning, ABG(lön) AS Medellön FROM personal GROUP BY befattning", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Befattning: {reader["befattning"]}, Medellön: {reader["Medellön"]}");
                    }
                }
            }
        }

        public void SetGradeWithTransaction()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Ange elevens ID: ");
                        var studentId = Console.ReadLine();
                        Console.WriteLine("Ange ämnets ID: ");
                        var subjectId = Console.ReadLine();
                        Console.WriteLine("Ange betyget: ");
                        var grade = Console.ReadLine();
                        Console.WriteLine("Ange lärarens ID: ");
                        var teacherId = Console.ReadLine();
                        Console.WriteLine("Ange datum (YYYY-MM-DD): ");
                        var date = Console.ReadLine();

                        var command = new NpgsqlCommand("INSERT INTO betyg (elev, ämne, betyg, lärare, datum) VALUES (@elev, @ämne, @betyg, @lärare, @datum)", connection);
                        command.Parameters.AddWithValue("@elev", studentId);
                        command.Parameters.AddWithValue("@ämne", subjectId);
                        command.Parameters.AddWithValue("@betyg", grade);
                        command.Parameters.AddWithValue("@lärare", teacherId);
                        command.Parameters.AddWithValue("@datum", date);

                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine("Betyget sattes dit lätt som en plätt! ");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Följande fel inträffade: " + ex.Message);
                    }
                }
            }
        }

        public void GetStudentInfoById()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Ange elevens ID: ");
                var studentID = int.Parse(Console.ReadLine());
                var command = new NpgsqlCommand("SELECT * FROM GetStudentInfoById(@elevId)", connection);
                command.Parameters.AddWithValue("@elevId", studentID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Förnamn: {reader["Förnamn"]}, Efternamn: {reader["Efternamn"]}, Klass: {reader["Klass"]}, Personnummer: {reader["Personnummer"]}");
                    }
                }
            }
        }
    }
}
