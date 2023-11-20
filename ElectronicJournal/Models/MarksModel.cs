using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System.Collections.Generic;

namespace ElectronicJournal.Models
{
    public class MarksModel : VM
    {
        private IEnumerable<Lesson> _lessons;
        private IEnumerable<Mark> _marks;
        private Lesson _selectedLesson;
        private Mark _selectedMark;
        private double _average;
        private string _description;

        public IEnumerable<Lesson> Lessons
        {
            get => _lessons;
            set
            {
                _lessons = value;
                OnPropertyChanged(propertyName: nameof(Lessons));
            }
        }

        public IEnumerable<Mark> Marks
        {
            get => _marks;
            set
            {
                _marks = value;
                OnPropertyChanged(propertyName: nameof(Marks));
            }
        }

        public Lesson SelectedLesson
        {
            get => _selectedLesson;
            set
            {
                _selectedLesson = value;
                OnPropertyChanged(propertyName: nameof(SelectedLesson));
            }
        }
 
        public Mark SelectedMark
        {
            get => _selectedMark;
            set
            {
                _selectedMark = value;
                OnPropertyChanged(propertyName: nameof(SelectedMark));
            }
        }

        public double Average
        {
            get => _average;
            set { 
                _average = value;
                OnPropertyChanged(propertyName: nameof(Average)); 
            }
        }
        
        public string Description
        {
            get => _description;
            set { 
                _description = value;
                OnPropertyChanged(propertyName: nameof(Description)); 
            }
        }
    }
}
