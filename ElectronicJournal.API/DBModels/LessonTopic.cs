using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class LessonTopic
{
    public int Id { get; set; }

    public int Lesson { get; set; }

    public string Topic { get; set; } = null!;

    public virtual ICollection<CoupleTiming> CoupleTimings { get; set; } = new List<CoupleTiming>();

    public virtual Lesson LessonNavigation { get; set; } = null!;
}
