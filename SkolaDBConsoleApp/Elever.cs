using SkolaDBConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaDBConsoleApp.Models
{
    public class Elever
    {
        public int Id { get; set; }
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public string Personnummer { get; set; }
        public int Klass { get; set; }

        public Klasser KlassNavigation { get; set; }
        public ICollection<Betyg> Betygs { get; set; }
    }
}
