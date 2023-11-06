namespace ElectronicJournal.ViewModels.Tools
{
    public class ContentPresenter : VM
    {
        private VM _content;

        public VM Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(propertyName: nameof(Content));
            }
        }
    }
}
