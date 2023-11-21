using ElectronicJournal.Models;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using ElectronicJournalAPI.Utilities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ElectronicJournal.ViewModels
{
    public class HomeworksVM : VM
    {
        private readonly IEventAggregator _eventAggregator;

        private HomeworksModel _model;

        private readonly Lazy<Command> _loaded;
        private readonly Lazy<Command> _moreInfo;

        public HomeworksVM(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _model = new HomeworksModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _moreInfo = Command.CreateLazyCommand(action: obj =>
            {
                GoToHomeworkViewerEventArgs e = new GoToHomeworkViewerEventArgs
                {
                    Homework = (obj as HomeworksVM).SelectedHomework,
                    User = User
                };
                _eventAggregator.GetEvent<GoToHomeworkViewerEvent>().Publish(e);
            });

            _loaded = Command.CreateLazyCommand(action: async _ =>
            {
                await ExecuteTask(taskForExecute: async () =>
                {
                    Homeworks = await User.GetHomeworks();
                    Header = $"Необходимо выполнить {Homeworks.Count()} {WordFormulator.GetForm(count: Homeworks.Count(), forms: new string[] { "заданий", "задание", "задания" })}: ";
                    if (Homeworks.Count() == 0)
                    {
                        Header = String.Empty;
                        Homeworks = null;
                    } 
                });
            });
        }

        public User User { get; set; }

        public IEnumerable<Homework> Homeworks
        {
            get => _model.Homeworks;
            set => _model.Homeworks = value;
        }

        public Homework SelectedHomework
        {
            get => _model.SelectedHomework;
            set => _model.SelectedHomework = value;
        }

        public string Header
        {
            get => _model.Header;
            set => _model.Header = value;
        }
        public Command Loaded => _loaded.Value;
        public Command MoreInfo => _moreInfo.Value;
    }
}
