using System;
using System.Collections.Generic;

namespace SkolaDBConsoleApp.models;

public partial class Personal
{
    public int Id { get; set; }

    public string? Förnamn { get; set; }

    public string? Efternamn { get; set; }

    public string? Befattning { get; set; }

    public virtual ICollection<Betyg> Betygs { get; set; } = new List<Betyg>();

    public virtual ICollection<Klasser> Klassers { get; set; } = new List<Klasser>();
}
