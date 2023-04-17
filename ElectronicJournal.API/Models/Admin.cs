using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.Models;

public partial class Admin
{
    public int Id { get; set; }

    public int User { get; set; }

    public int Role { get; set; }

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
