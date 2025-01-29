using SkolaDBConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaDBConsoleApp.Models
{
    public class Ämnen
    {
        public int Id { get; set; }
        public string Namn { get; set; }

        public ICollection<Betyg> Betygs { get; set; }
    }
}
