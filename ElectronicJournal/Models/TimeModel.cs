using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ElectronicJournal.Models
{
	internal class TimeModel : INotifyPropertyChanged
	{
		private DateTime _time = DateTime.Now;

		public DateTime Time
		{
			get => _time;
			set
			{
				_time = value;
				OnPropertyChanged("Time");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	}
}
