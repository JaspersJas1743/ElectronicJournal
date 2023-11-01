using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Homework
{
    public int Id { get; set; }

    public int? Lesson { get; set; }

    public int? Group { get; set; }

    public string Text { get; set; } = null!;

    public virtual Group? GroupNavigation { get; set; }

    public virtual Lesson? LessonNavigation { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
