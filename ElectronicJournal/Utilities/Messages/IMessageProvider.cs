using static ElectronicJournal.Resources.Windows.MessageWindow;

namespace ElectronicJournal.Utilities.Messages
{
    public interface IMessageProvider
    {
        string File { get; set; }

        MessageWindowResult Show(string text, string windowTitle, MessageWindowImage image, MessageWindowButton buttons);
        MessageWindowResult ShowError(string text);
        MessageWindowResult ShowWarning(string text);
        MessageWindowResult ShowInformation(string text);
        void ShowMessage(string text);
        string OpenFile();
        string[] OpenManyFile();
        string OpenFile(string filter);
        string[] OpenManyFile(string filter);
        string SaveFile();
        string BrowseFolder();
    }
}
