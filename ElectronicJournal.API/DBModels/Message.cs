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

    public int? Attachment { get; set; }

    public virtual Attachment AttachmentNavigation { get; set; } = null!;

    public virtual User? SenderNavigation { get; set; }

    public virtual ICollection<User> Receivers { get; set; } = new List<User>();
}
