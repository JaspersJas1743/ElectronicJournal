using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicJournal.Resources.CustomElements
{
	public partial class CodeEntryPanel : UserControl
	{
		#region Fields
		private static readonly DependencyProperty EntryCodeProperty = DependencyProperty.Register(
			name: "EntryCode", propertyType: typeof(String), ownerType: typeof(CodeEntryPanel), new PropertyMetadata()
		);

		public const int MaxCountOfCell = 6;
		#endregion Fields

		#region Constructors
		public CodeEntryPanel()
		{
			InitializeComponent();
		}
		#endregion Constructors

		#region Properties
		public string EntryCode
		{
			get => (string)GetValue(dp: EntryCodeProperty);
			set => SetValue(dp: EntryCodeProperty, value: value);
		}
		#endregion Properties

		#region Methods
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
			tb.SelectionStart = tb.Text.Length;
			SetValue(EntryCodeProperty, String.Concat(values: MainGrid.Children.OfType<TextBox>().Select(x => x.Text)).Trim());
		}

		private void OnTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			if (e.Key == Key.Back && tb.Text.Length == 0)
				tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
		}
		#endregion Methods
	}
}
