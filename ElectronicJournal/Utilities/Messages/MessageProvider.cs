using ElectronicJournal.Resources.Windows;
using Microsoft.Win32;
using System.Windows;
using Ookii.Dialogs.Wpf;
using System;

namespace ElectronicJournal.Utilities.Messages
{
    public class MessageProvider : IMessageProvider
    {
        public string File { get; set; }
        public string Path { get; set; }

        public string BrowseFolder()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
            {
                Description = "Обзор...",
                UseDescriptionForTitle = true,
                Multiselect = false
            };

            _ = dialog.ShowDialog(owner: Application.Current.MainWindow);
            return dialog.SelectedPath;
        }

        public string OpenFile()
            => OpenFile(filter: String.Empty);  

        public string[] OpenManyFile()
            => OpenManyFile(filter: String.Empty);  
        
        public string OpenFile(string filter)
        {
            return ShowDialog(dialog: new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                Multiselect = false,
                Filter = filter,
                Title = "Открыть...",
                ValidateNames = true
            });
        }

        public string[] OpenManyFile(string filter)
        {
            return ShowDialogForManyFile(dialog: new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                Multiselect = true,
                Filter = filter,
                Title = "Открыть...",
                ValidateNames = true
            });
        }

        public string SaveFile()
        {
            return ShowDialog(dialog: new SaveFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                Title = "Сохранить...",
                ValidateNames = true
            });
        }

        private string ShowDialog(FileDialog dialog)
        {
            _ = dialog.ShowDialog(owner: Application.Current.MainWindow);
            return dialog.FileName;
        }

        private string[] ShowDialogForManyFile(FileDialog dialog)
        {
            _ = dialog.ShowDialog(owner: Application.Current.MainWindow);
            return dialog.FileNames;
        }

        public MessageWindow.MessageWindowResult Show(string text, string windowTitle, MessageWindow.MessageWindowImage image, MessageWindow.MessageWindowButton buttons)
            => MessageWindow.Show(text: text, windowTitle: windowTitle, image: image, buttons: buttons);

        public MessageWindow.MessageWindowResult ShowError(string text)
            => MessageWindow.ShowError(text: text);

        public MessageWindow.MessageWindowResult ShowInformation(string text)
            => MessageWindow.ShowInformation(text: text);

        public void ShowMessage(string text)
            => MessageWindow.ShowMessage(text: text);

        public MessageWindow.MessageWindowResult ShowWarning(string text)
            => MessageWindow.ShowWarning(text: text);
    }
}
