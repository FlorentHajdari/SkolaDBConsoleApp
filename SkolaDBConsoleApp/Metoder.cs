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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod: {ex.Message}");
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
            ConsoleCleaner();

        }

        public void AddNewStaff()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                Console.WriteLine("Ange förnamn: ");
                var firstName = Console.ReadLine();
                Console.WriteLine("Ange efternamn: ");
                var lastName = Console.ReadLine();
                Console.WriteLine("Ange befattning: ");
                var position = Console.ReadLine();
                Console.WriteLine("Ange avdelning: ");
                var department = Console.ReadLine();
                Console.WriteLine("Ange antalet års erfarenhet på skolan: ");
                var Experience = int.Parse(Console.ReadLine());
                Console.WriteLine("Ange månadslön: ");
                var salary = decimal.Parse(Console.ReadLine());

                var command = new NpgsqlCommand(
                    @"INSERT INTO personal (Förnamn, Efternamn, Befattning, Avdelning, ArbetadeÅr, Månadslön) VALUES (@Förnamn, @Efternamn, @Befattning, @ArbetadeÅr, @Måndadslön)", connection);
                command.Parameters.AddWithValue("@Förnamn", firstName);
                command.Parameters.AddWithValue("@Efternamn", lastName);
                command.Parameters.AddWithValue("@Befattning", position);
                command.Parameters.AddWithValue("@Avdelning", department);
                command.Parameters.AddWithValue("@ArbetadeÅr", Experience);
                command.Parameters.AddWithValue("@Måndadslön", salary);

                command.ExecuteNonQuery();
                Console.WriteLine("Ny personal är tillagd! ");
            }
            ConsoleCleaner();

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
            ConsoleCleaner();

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
            ConsoleCleaner();

        }

        public void GetAllStaffOverview()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT Förnamn, Efternamn, Befattning, ArbetadeÅr FROM personal", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Förnamn"]} {reader["Efternamn"]} - Befattning: {reader["Befattning"]} - År: {reader["ArbetadeÅr"]}");
                    }
                }
            }
            ConsoleCleaner();

        }

        public void GetGradesForStudent()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Ange elevens ID: ");

                if (!int.TryParse(Console.ReadLine(), out int studentID))
                {
                    Console.WriteLine("ERROR. Ange ett giltigt ID som är ett heltal: ");
                    return;
                }
                var command = new NpgsqlCommand(
                    @"SELECT
                     e.förnamn || ' ' || e.efternamn AS elev_namn,
                     äm.namn AS ämne,
                     b.betyg,
                     b.datum,
                     p.förnamn || ' ' || p.efternamn AS lärare_namn
                     FROM
                     public.betyg b
                     JOIN
                     public.elever e ON b.elev = e.id
                     JOIN
                     public.""Ämnen"" äm ON b.""Ämne"" = äm.id
                     JOIN
                     public.personal p ON b.""lärare"" = p.id
                     WHERE
                     e.id = @elevId
                     ORDER BY
                     b.datum DESC;", connection);

                command.Parameters.AddWithValue("@elevId", studentID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Elev: {reader["elev_namn"]}, Ämne: {reader["ämne"]}, Betyg: {reader["betyg"]}, Datum: {reader["datum"]}, Lärare: {reader["lärare_namn"]}");
                    }
                }
            }
            ConsoleCleaner();

        }

   

        public void GetAverageSalariesByDepartment()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT avdelning, AVG(månadslön) AS Medellön FROM public.personal GROUP BY avdelning", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Avdelning: {reader["avdelning"]}, Medellön: {reader["medellön"]}");
                    }
                }
            }
            ConsoleCleaner();

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
                        if  (!int.TryParse(Console.ReadLine(), out int studentId))
                        {
                            Console.WriteLine("Ej godtagbart elev-ID. Ange ett heltal: ");
                            return;
                        }
                        Console.WriteLine("Ange ämnets ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int subjectId))
                        {
                            Console.WriteLine("Ej godtagbart ämnes-ID. Ange ett heltal: ");
                            return;
                        }
                        Console.WriteLine("Ange betyget (A-F): ");
                        var grade = Console.ReadLine()?.Trim().ToUpper();
                        if (string.IsNullOrEmpty(grade) || !"ABCDEF".Contains(grade))
                        {
                            Console.WriteLine("Fel typ av betyg satt. Ange en bokstav mellan A till F. ");
                            return;
                        }
                        Console.WriteLine("Ange lärarens ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int teacherId))
                        {
                            Console.WriteLine("Ej godtagbart lärar-ID. Ange ett heltal: ");
                            return;
                        }
                        Console.WriteLine("Ange datum (YYYY-MM-DD): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            Console.WriteLine("Felaktigt format på datum. Ange datum i detta format YYYY-MM-DD.) ");
                            return;
                        }

                        using (var command = new NpgsqlCommand(
                            "INSERT INTO betyg (elev, \"Ämne\", betyg, \"lärare\", datum) VALUES (@elev, @ämne, @betyg, @lärare, @datum)",
                            connection))
                        {
                            command.Parameters.AddWithValue("@elev", studentId);
                            command.Parameters.AddWithValue("@ämne", subjectId);
                            command.Parameters.AddWithValue("@betyg", grade);
                            command.Parameters.AddWithValue("@lärare", teacherId);
                            command.Parameters.AddWithValue("@datum", date);

                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }

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
            ConsoleCleaner();

        }

        public void GetStudentInfoById()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Ange elevens ID: ");

                if (!int.TryParse(Console.ReadLine(), out int studentID))
                {
                    Console.WriteLine("ERROR. Ange ett giltigt ID som är ett heltal. ");
                    return;
                }

                var command = new NpgsqlCommand("SELECT * FROM GetStudentInfoById(@elevId);", connection);
                command.Parameters.AddWithValue("@elevId", studentID);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Ingen elev matchade med ditt angivna ID. ");
                        return;
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"Förnamn: {reader["förnamn"]}, Efternamn: {reader["efternamn"]}, Klass: {reader["klass"]}, Personnummer: {reader["personnummer"]}, Längd (cm): {reader["längd"]}, Vikt (kg): {reader["vikt"]}");
                    }
                }
            }
            ConsoleCleaner();

        }

        public void GetAllStudentDetails()
        {
            using (var context = new SkolaDbContext())
            {
                var students = context.Elevers
                    .Include(e => e.KlassNavigation)
                    .ToList();

                foreach (var student in students)
                {
                    Console.WriteLine($"Förnamn: {student.Förnamn}, Efternamn: {student.Efternamn}, Klass: {student.KlassNavigation.Namn}, Personnummer: {student.Personnummer}, Längd: {student.Längd}, Vikt: {student.Vikt}");
                }
            }
            ConsoleCleaner();

        }

        public void ConsoleCleaner()
        {
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
            Console.Clear();
        }

        public void GetTotalSalaryByDepartment()
        {
            var connectionString = "Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(
                    @"SELECT avdelning, SUM(månadslön) AS total_månadslön
                      FROM public.personal
                      GROUP BY avdelning;", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Avdelning: {reader["avdelning"]} - Totala måndadslöner: {reader["total_månadslön"]}");
                    }
                }
            }
            ConsoleCleaner();
        }
    }
}
