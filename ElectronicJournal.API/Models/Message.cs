using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.Models;

public partial class Message
{
    public int Id { get; set; }

    public int Receiver { get; set; }

    public int Sender { get; set; }

    public string? Text { get; set; }

    public DateTime SendDatetime { get; set; }

    public DateTime? ReadDatetime { get; set; }

    public virtual User ReceiverNavigation { get; set; } = null!;

    public virtual User SenderNavigation { get; set; } = null!;

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
