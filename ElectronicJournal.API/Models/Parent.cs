using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.Models;

public partial class Parent
{
    public int Id { get; set; }

    public int User { get; set; }

    public int Child { get; set; }

    public virtual Student ChildNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
