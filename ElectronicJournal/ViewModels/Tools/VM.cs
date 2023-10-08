using ElectronicJournal.Utilities;
using ElectronicJournalAPI;
using System;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels.Tools
{
    public class VM : TrackedObject
    {
        #region Fields
        private string _defaultButtonContent;

        private string _buttonContent;
        #endregion Fields

        #region Constructors
        protected VM(string defaultButtonContent = "")
        {
            _defaultButtonContent = "Войти";
            _buttonContent = _defaultButtonContent;
        }
        #endregion Constructors

        #region Properties
        public string ButtonContent
        {
            get => _buttonContent;
            set
            {
                _buttonContent = value;
                OnPropertyChanged(propertyName: nameof(ButtonContent));
            }
        }

        public bool CanMoveToAnotherPage => ButtonContent.Equals(_defaultButtonContent);
        #endregion Properties

        #region Methods
        protected async Task<TOut> ExecuteTask<TOut>(Func<Task<TOut>> taskForExecute)
        {
            try
            {
                var task = taskForExecute();
                await Exec(task: task);
                return await task;
            }
            catch (TaskCanceledException ex)
            {
                throw new ApiException(message: "Удаленный сервер не доступен в данный момент", inner: ex);
            }
            catch { throw; }
        }

        protected async Task ExecuteTask(Func<Task> taskForExecute)
        {
            try
            {
                var task = taskForExecute();
                await Exec(task: task);
                await task;
            } catch (TaskCanceledException ex)
            {
                throw new ApiException(message: "Удаленный сервер не доступен в данный момент", inner: ex);
            } catch { throw; }
        }

        private async Task Exec(Task task)
        {
            int count = 0;
            while (!task.IsCompleted)
            {
                if (count == 3)
                    count = 0;
                ButtonContent = "Загрузка" + new String(c: '.', count: ++count);

                await Task.Delay(millisecondsDelay: 250);
            }
            ButtonContent = _defaultButtonContent;
        }
        #endregion Methods
    }
}
