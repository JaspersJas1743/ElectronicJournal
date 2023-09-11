using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels.Tools
{
	public class TrackedObject : INotifyPropertyChanged
	{
		#region Fields
		protected string DefaultButtonContent;

		protected string _buttonContent;
		#endregion Fields

		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion Events

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
		#endregion Properties

		#region Methods
		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
			=> PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName));

		protected async Task<bool> TryExecuteTask(Func<Task<bool>> taskForExecute)
		{
			var task = taskForExecute();

			int count = 0;
			while (!task.IsCompleted)
			{
				if (count == 3)
					count = 0;
				ButtonContent = "Загрузка" + new String(c: '.', count: ++count);

				await Task.Delay(millisecondsDelay: 250);
			}

			bool result = await task;
			ButtonContent = DefaultButtonContent;
			return result;
		}
		#endregion Methods
	}
}
