using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaDBConsoleApp.Models
{
    public class Klasser
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public int Mentor { get; set; }

        public Personal MentorNavigation { get; set; }
        public ICollection<Elever> Elevers { get; set; }
    }
}
