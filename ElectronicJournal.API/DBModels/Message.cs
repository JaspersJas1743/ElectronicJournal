using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Message
{
    public int Id { get; set; }

    public int? Sender { get; set; }

    public string? Text { get; set; }

    public DateTime SendDatetime { get; set; }

    public DateTime? ReadDatetime { get; set; }

    public virtual User? SenderNavigation { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
