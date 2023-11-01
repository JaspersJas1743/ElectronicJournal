using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Mark
{
    public int Id { get; set; }

    public int? Student { get; set; }

    public int? Teacher { get; set; }

    public int? Lesson { get; set; }

    public int? Mark1 { get; set; }

    public string? Description { get; set; }

    public virtual Lesson? LessonNavigation { get; set; }

    public virtual MarksEnum? Mark1Navigation { get; set; }

    public virtual Student? StudentNavigation { get; set; }

    public virtual Teacher? TeacherNavigation { get; set; }
}
