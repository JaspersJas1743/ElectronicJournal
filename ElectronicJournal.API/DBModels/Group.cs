﻿using System;
using System.Collections.Generic;

namespace ElectronicJournal.API.DBModels;

public partial class Group
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ClassTeacher { get; set; }

    public int Specialization { get; set; }

    public virtual Teacher? ClassTeacherNavigation { get; set; }

    public virtual ICollection<CoupleTiming> CoupleTimings { get; set; } = new List<CoupleTiming>();

    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

    public virtual Specialization SpecializationNavigation { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
