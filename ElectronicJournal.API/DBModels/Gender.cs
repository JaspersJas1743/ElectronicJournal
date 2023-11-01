using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Gender
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
