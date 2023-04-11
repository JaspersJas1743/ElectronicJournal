﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ElectronicJournal.ViewModels
{
	public class BaseVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
			=> PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName));
	}
}
