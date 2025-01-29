using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaDBConsoleApp.Models
{
    public class Betyg
    {
        public int Id { get; set; }
        public int Elev { get; set; }
        public Elever ElevNavigation { get; set; }
        public int Ämne { get; set; }
        public Ämnen ÄmneNavigation { get; set; }
        public string Betyg1 { get; set; }
        public DateTime Datum { get; set; }
        public int Lärare { get; set; }
        public Personal LärareNavigation { get; set; }
    }
}
