using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System.Collections.Generic;

namespace ElectronicJournal.Models
{
    public class HomeworksModel : TrackedObject
    {
        private IEnumerable<Homework> _homeworks;
        private Homework _selectedHomeworks;
        private string _header;

        public IEnumerable<Homework> Homeworks
        {
            get => _homeworks;
            set
            {
                _homeworks = value;
                OnPropertyChanged(propertyName: nameof(Homeworks));
            }
        }

        public Homework SelectedHomework
        {
            get => _selectedHomeworks;
            set
            {
                _selectedHomeworks = value;
                OnPropertyChanged(propertyName: nameof(SelectedHomework));
            }
        }

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(propertyName: nameof(Header));
            }
        }
    }
}
