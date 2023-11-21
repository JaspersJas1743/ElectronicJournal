using ElectronicJournal.Models;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.ViewModels
{
    public class MarksVM : VM
    {
        private MarksModel _model;

        private Lazy<Command> _loaded;
        private Lazy<Command> _lessonsLoaded;
        private Lazy<Command> _lessonsSelectionChanged;
        private Lazy<Command> _marksSelectionChanged;

        public MarksVM() 
        {
            _model = new MarksModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _loaded = Command.CreateLazyCommand(action: async _ =>
            {
                try
                {
                    Lessons = await ExecuteTask(taskForExecute: async () => await User.GetLessons());
                }
                catch (Exception)
                {
                }
            });

            _lessonsLoaded = Command.CreateLazyCommand(action: async arg =>
            {
                (arg as ListView).SelectedIndex = 0;
            });

            _lessonsSelectionChanged = Command.CreateLazyCommand(action: async _ =>
            {
                try
                {
                    await ExecuteTask(async () => await SelectedLesson.GetMarks());
                    Marks = SelectedLesson.Marks.Count() == 0 ? null : SelectedLesson.Marks;
                    Average = SelectedLesson.Average;
                } catch(Exception)
                {
                }
            });

            _marksSelectionChanged = Command.CreateLazyCommand(action: _ =>
            {
                Description = SelectedMark?.Description;
            });
        }

        public IEnumerable<Subject> Lessons 
        {
            get => _model.Lessons;
            set => _model.Lessons = value;
        }

        public IEnumerable<Mark> Marks
        {
            get => _model.Marks;
            set => _model.Marks = value;
        }

        public Subject SelectedLesson
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
        public Command LessonsLoaded => _lessonsLoaded.Value;
        public Command LessonsSelectionChanged => _lessonsSelectionChanged .Value;
        public Command MarksSelectionChanged => _marksSelectionChanged.Value;
    }
}
