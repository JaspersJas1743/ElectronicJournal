using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Specialization
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
