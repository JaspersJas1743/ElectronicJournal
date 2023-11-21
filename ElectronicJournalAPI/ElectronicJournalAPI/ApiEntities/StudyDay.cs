using ElectronicJournalAPI.Utilities;
using System;
using System.Collections.Generic;

namespace ElectronicJournalAPI.ApiEntities
{
    public class StudyDay
    {
        public int CountLesson { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string StudyTime => CountLesson == 0 ? String.Empty : $"{StartTime.Value.ToString("hh\\:mm")} - {EndTime.Value.ToString("hh\\:mm")}";
        public string CountLessonString => CountLesson == 0 ? "Нет пар": $"{CountLesson} {WordFormulator.GetForm(count: CountLesson, forms: new string[] { "пар", "пара", "пары" })}";
        public IEnumerable<Lesson> Lessons { get; set; }
    }
}
