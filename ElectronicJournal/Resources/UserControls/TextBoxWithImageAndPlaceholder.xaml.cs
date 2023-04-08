using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.Resources.UserControls
{
	public partial class TextBoxWithImageAndPlaceholder : UserControl
	{
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			name: "Text", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);

		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
			name: "ImageSource", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);

		public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
			name: "Placeholder", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
		);

		public TextBoxWithImageAndPlaceholder()
			=> InitializeComponent();

		public string Text
		{
			get => (string)GetValue(dp: TextProperty);
			set => SetValue(dp: TextProperty, value: value);
		}

		public string ImageSource
		{
			get => (string)GetValue(dp: ImageSourceProperty);
			set => SetValue(dp: ImageSourceProperty, value: value);
		}

		public string Placeholder
		{
			get => (string)GetValue(dp: PlaceholderProperty);
			set => SetValue(dp: PlaceholderProperty, value: value);
		}

		public int MaxLength => MainTextBox.MaxLength;
	}
}