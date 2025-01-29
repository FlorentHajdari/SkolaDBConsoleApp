using System;
using System.Collections.Generic;

namespace SkolaDBConsoleApp.models;

public partial class Betyg
{
    public int Id { get; set; }

    public int? Elev { get; set; }

    public int? Ämne { get; set; }

    public char? Betyg1 { get; set; }

    public DateOnly? Datum { get; set; }

    public int? Lärare { get; set; }

    public virtual Elever? ElevNavigation { get; set; }

    public virtual Personal? LärareNavigation { get; set; }

    public virtual Ämnen? ÄmneNavigation { get; set; }
}
