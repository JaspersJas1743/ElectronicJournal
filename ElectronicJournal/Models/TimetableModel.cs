using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System.Collections.Generic;

namespace ElectronicJournal.Models
{
    public class TimetableModel : TrackedObject
    {
        private IEnumerable<StudyDay> _days;
        private IEnumerable<Lesson> _lessons;
        private StudyDay _selectedDay;

        public IEnumerable<StudyDay> Days
        {
            get => _days;
            set
            {
                _days = value;
                OnPropertyChanged(propertyName: nameof(Days));
            }
        }

        public IEnumerable<Lesson> Lessons
        {
            get => _lessons;
            set
            {
                _lessons = value;
                OnPropertyChanged(propertyName: nameof(Lessons));
                OnPropertyChanged(propertyName: "DontHaveLessons");
            }
        }

        public StudyDay SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                OnPropertyChanged(propertyName: nameof(SelectedDay));
            }
        }
    }
}
