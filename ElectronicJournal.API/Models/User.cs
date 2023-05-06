using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.Models;

public partial class User
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Role { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string AuthKey { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Photo { get; set; }

    public string? RegistrationCode { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual ICollection<Message> MessageReceiverNavigations { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenderNavigations { get; set; } = new List<Message>();

    public virtual Parent? Parent { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
