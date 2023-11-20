using ElectronicJournal.Models;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
    public class MarksVM : VM
    {
        private MarksModel _model;

        private Lazy<Command> _loaded;
        private Lazy<Command> _lessonsSelectionChanged;
        private Lazy<Command> _marksSelectionChanged;

        public MarksVM() 
        {
            _model = new MarksModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _loaded = Command.CreateLazyCommand(action: async _ =>
            {
                Lessons = await User.GetLessons();
            });

            _lessonsSelectionChanged = Command.CreateLazyCommand(action: async _ =>
            {
                await SelectedLesson.GetMarks();
                Marks = SelectedLesson.Marks;
                Average = SelectedLesson.Average;
            });

            _marksSelectionChanged = Command.CreateLazyCommand(action: _ =>
            {
                Description = SelectedMark?.Description;
            });
        }

        public IEnumerable<Lesson> Lessons 
        {
            get => _model.Lessons;
            set => _model.Lessons = value;
        }

        public IEnumerable<Mark> Marks
        {
            get => _model.Marks;
            set => _model.Marks = value;
        }

        public Lesson SelectedLesson
        {
            get => _model.SelectedLesson;
            set => _model.SelectedLesson = value;
        }

        public Mark SelectedMark
        {
            get => _model.SelectedMark;
            set => _model.SelectedMark = value;
        }

        public double Average
        {
            get => _model.Average;
            set => _model.Average = value;
        }
        
        public string Description
        {
            get => _model.Description;
            set => _model.Description = value;
        }

        public User User { get; set; }

        public Command Loaded => _loaded.Value;
        public Command LessonsSelectionChanged => _lessonsSelectionChanged .Value;
        public Command MarksSelectionChanged => _marksSelectionChanged.Value;
    }
}
