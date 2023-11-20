using ElectronicJournalAPI.Utilities;
using System;
using System.Drawing;

namespace ElectronicJournalAPI.ApiEntities
{
    public class Homework
    {
        public string Lesson { get; set; }
        public string Text { get; set; }
        public string Teacher { get; set; }
        public DateTime CompletionDate { get; set; }
        public Attachment Attachment { get; set; }
        public bool HaveAttachment => Attachment != null;
        public string Deadline
        {
            get
            {
                TimeSpan difference = CompletionDate.Subtract(value: DateTime.Now);
                if (difference.TotalHours < 1)
                    return GetMinutesString(minutes: difference.Minutes);

                if (difference.TotalDays < 1)
                    return GetHoursString(hours: difference.Hours);

                return GetDaysString(days: difference.Days);
            }
        }

        public int CountRemainDays => CompletionDate.Subtract(value: DateTime.Now).Days;
        public string CountRemainDaysString
        {
            get
            {
                TimeSpan difference = CompletionDate.Subtract(value: DateTime.Now);
                if (difference.TotalHours < 1)
                    return GetTimeForm(count: difference.Minutes, func: GetMinutesForm);

                if (difference.TotalDays < 1)
                    return GetTimeForm(count: difference.Hours, func: GetHourForm);

                return GetTimeForm(count: difference.Days, func: GetDayForm);
            }
        }

        private string GetMinutesString(int minutes)
            => GetTimes(count: minutes, func: GetMinutesForm);

        private string GetHoursString(int hours)
            => GetTimes(count: hours, func: GetHourForm);

        private string GetDaysString(int days)
            => GetTimes(count: days, func: GetDayForm);

        private string GetTimes(int count, Func<int, string> func)
            => $"{(count % 10 == 1 && count != 11 ? "Остался" : "Осталось")} {GetTimeForm(count: count, func: func)}";

        private string GetTimeForm(int count, Func<int, string> func)
            => $"{count} {func(count)}";

        private string GetMinutesForm(int count)
            => WordFormulator.GetForm(count: count, forms: new[] { "минут", "минута", "минуты" });

        private string GetDayForm(int count)
            => WordFormulator.GetForm(count: count, forms: new[] { "дней", "день", "дня" });

        private string GetHourForm(int count)
            => WordFormulator.GetForm(count: count, forms: new[] { "часов", "час", "часа" });
    }
}
