using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Attachment
{
    public int Id { get; set; }

    public string? Path { get; set; }

    public virtual Message? Message { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();
}
