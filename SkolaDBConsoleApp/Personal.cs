using SkolaDBConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaDBConsoleApp.Models
{
    public class Personal
    {
        public int Id { get; set; }
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public string Befattning { get; set; }
        public string Avdelning { get; set; }
        public int ArbetadeÅr { get; set; }



        public ICollection<Klasser> Klassers { get; set; }
        public ICollection<Betyg> Betygs { get; set; }
    }
}
