using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class User
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int UserRole { get; set; }

    public string Gender { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Photo { get; set; }

    public string? RegistrationCode { get; set; }

    public string? ConfirmationCode { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Gender GenderNavigation { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Parent? Parent { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual UserRole UserRoleNavigation { get; set; } = null!;

    public virtual ICollection<Message> MessagesNavigation { get; set; } = new List<Message>();
}
