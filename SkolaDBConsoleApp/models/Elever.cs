using System;
using System.Collections.Generic;

namespace SkolaDBConsoleApp.models;

public partial class Elever
{
    public int Id { get; set; }

    public string? Förnamn { get; set; }

    public string? Efternamn { get; set; }

    public string? Personnummer { get; set; }

    public int? Klass { get; set; }

    public virtual ICollection<Betyg> Betygs { get; set; } = new List<Betyg>();

    public virtual Klasser? KlassNavigation { get; set; }
}
