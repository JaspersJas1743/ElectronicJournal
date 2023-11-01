using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class CoupleTiming
{
    public int Id { get; set; }

    public int? Lesson { get; set; }

    public TimeSpan Start { get; set; }

    public TimeSpan End { get; set; }

    public DateTime Date { get; set; }

    public int? Group { get; set; }

    public string Auditorium { get; set; } = null!;

    public int Topic { get; set; }

    public virtual Group? GroupNavigation { get; set; }

    public virtual Lesson? LessonNavigation { get; set; }

    public virtual LessonTopic TopicNavigation { get; set; } = null!;
}
