using System;

namespace ElectronicJournalAPI.ApiEntities
{
    public class Lesson
    {
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Duration => $"{Start.ToString("hh\\:mm")} - {End.ToString("hh\\:mm")}";
        public string Teacher { get; set; }
        public string Auditorium { get; set; }
    }
}
