using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Lesson
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<CoupleTiming> CoupleTimings { get; set; } = new List<CoupleTiming>();

    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

    public virtual ICollection<LessonTopic> LessonTopics { get; set; } = new List<LessonTopic>();

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
