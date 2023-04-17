using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.Models;

public partial class Attachment
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
