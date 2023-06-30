using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicJournal.Resources.CustomElements
{
	public partial class CodeEntryPanel : UserControl
	{
		public CodeEntryPanel()
		{
			InitializeComponent();
		}

		public string Text => String.Concat(values: MainGrid.Children.OfType<TextBox>().Select(x => x.Text)).Trim();

		private void OnCharChanged(object sender, TextChangedEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			e.Handled = true;
			TextChange change = e.Changes.First();
			if (change.AddedLength > 0)
			{
				tb.Text = tb.Text.Substring(startIndex: change.Offset, length: 1);

				if (tb.Name.Equals(String.Empty))
					tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
		}

		private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			if (tb.Text.Length > 0)
				tb.SelectionStart = 1;
		}

		private void OnTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			if (e.Key == Key.Back && tb.Text.Length == 0)
				tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
		}
	}
}
