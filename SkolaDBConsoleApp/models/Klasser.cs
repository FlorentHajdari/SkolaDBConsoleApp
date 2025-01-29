using System;
using System.Collections.Generic;

namespace SkolaDBConsoleApp.models;

public partial class Klasser
{
    public int Id { get; set; }

    public string? Namn { get; set; }

    public int? Mentor { get; set; }

    public virtual ICollection<Elever> Elevers { get; set; } = new List<Elever>();

    public virtual Personal? MentorNavigation { get; set; }
}
