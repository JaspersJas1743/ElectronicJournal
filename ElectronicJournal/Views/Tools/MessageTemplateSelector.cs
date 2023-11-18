using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.Views.Tools
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MessageIsSelected { get; set; }
        public DataTemplate MessageIsNotSelected { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
            => item is null ? MessageIsNotSelected : MessageIsSelected;
    }
}
