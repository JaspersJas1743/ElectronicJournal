using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class MarksEnum
{
    public int Id { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}
