using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicJournal.Resources.UserControls
{
	public partial class TextBoxWithImageAndPlaceholder : UserControl
	{
		#region Fields
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			name: "Text", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);

		public static readonly new DependencyProperty BorderBrushProperty = DependencyProperty.Register(
			name: "BorderBrush", propertyType: typeof(Brush), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);

		public static readonly new DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
			name: "BorderThickness", propertyType: typeof(Thickness), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);

		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
			name: "Image", propertyType: typeof(Style), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);

		public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
			name: "Placeholder", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);
		#endregion Fields

		#region Constructor
		public TextBoxWithImageAndPlaceholder()
			=> InitializeComponent();
		#endregion Constructor

		#region Properties
		public string Text
		{
			get => (string)GetValue(dp: TextProperty);
			set => SetValue(dp: TextProperty, value: value);
		}
		public new Brush BorderBrush
		{
			get => (Brush)GetValue(dp: BorderBrushProperty);
			set => SetValue(dp: BorderBrushProperty, value: value);
		}
		public new Thickness BorderThickness
		{
			get => (Thickness)GetValue(dp: BorderThicknessProperty);
			set => SetValue(dp: BorderThicknessProperty, value: value);
		}

		public Style Image
		{
			get => (Style)GetValue(dp: ImageProperty);
			set => SetValue(dp: ImageProperty, value: value);
		}

		public string Placeholder
		{
			get => (string)GetValue(dp: PlaceholderProperty);
			set => SetValue(dp: PlaceholderProperty, value: value);
		}

		public void SetSelectionStart(int selectionStart)
			=> MainTextBox.SelectionStart = selectionStart;

		public int MaxLength => MainTextBox.MaxLength;
		#endregion Properties

		#region Events
		public event TextChangedEventHandler TextChanged;
		#endregion Events

		#region Methods
		public new void Focus()
			=> MainTextBox.Focus();

		private void OnMainTextBoxChanged(object sender, TextChangedEventArgs e)
			=> TextChanged?.Invoke(sender: sender, e: e);
		#endregion Methods
	}
}