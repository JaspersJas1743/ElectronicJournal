namespace ElectronicJournal.ViewModels
{
    class MessageWindowVM : TrackedObject
    {
        private string _imageSource;

        public MessageWindowVM()
        {

        }

        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(propertyName: nameof(ImageSource));
            }
        }
    }
}
