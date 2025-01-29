using System;
using System.Collections.Generic;

namespace SkolaDBConsoleApp.models;

public partial class Ämnen
{
    public int Id { get; set; }

    public string? Namn { get; set; }

    public virtual ICollection<Betyg> Betygs { get; set; } = new List<Betyg>();
}
