using ElectronicJournal.Models;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicJournal.ViewModels
{
    public class TimetableVM : VM
    {
        #region Fields
        private TimetableModel _model;

        private Lazy<Command> _loaded;
        private Lazy<Command> _daysLoaded;
        private Lazy<Command> _daySelectionChanged;
        #endregion Fields

        #region Constructor
        public TimetableVM()
        {
            _model = new TimetableModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _loaded = Command.CreateLazyCommand(action: async obj =>
            {
                try
                {
                    Days = await ExecuteTask(taskForExecute: async () => await UpdateDays(selectedDate: DateTime.Now));
                }
                catch { }
            });

            _daysLoaded = Command.CreateLazyCommand(action: obj =>
            {
                (obj as ListView).SelectedIndex = 2;
            });

            _daySelectionChanged = Command.CreateLazyCommand(action: async obj =>
            {
                if (SelectedDay == null)
                    return;

                try
                {
                    Days = await ExecuteTask(taskForExecute: async () => await UpdateDays(SelectedDay.Date));
                }
                catch { }
            });
        }
        #endregion Constructor

        #region Properties
        public IEnumerable<StudyDay> Days
        {
            get => _model.Days;
            set => _model.Days = value;
        }

        public StudyDay SelectedDay
        {
            get => _model.SelectedDay;
            set
            {
                _model.SelectedDay = value;
                Lessons = value.Lessons;
            }
        }

        public IEnumerable<Lesson> Lessons
        {
            get => _model.Lessons;
            set => _model.Lessons = value;
        }

        public bool DontHaveLessons => Lessons.Count() == 0;

        public User User { get; set; }

        public Command Loaded => _loaded.Value;
        public Command DaysLoaded => _daysLoaded.Value;
        public Command DaySelectionChanged => _daySelectionChanged.Value;
        #endregion Properties

        public async Task<IEnumerable<StudyDay>> UpdateDays(DateTime selectedDate)
        {
            DateTime start = selectedDate.Subtract(value: new TimeSpan(2, 0, 0, 0));
            DateTime end = start.AddDays(value: 4);

            return await new Timetable(startDate: start, endDate: end).GetTimetable();
        }
    }
}
