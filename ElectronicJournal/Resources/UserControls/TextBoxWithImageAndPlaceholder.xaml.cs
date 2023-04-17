using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.Resources.UserControls
{
    public partial class TextBoxWithImageAndPlaceholder : UserControl
    {
		#region Fields
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
        );

        public static readonly DependencyProperty LightThemeImageSourceProperty = DependencyProperty.Register(
            name: "LightThemeImageSource", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
        );

		public static readonly DependencyProperty DarkThemeImageSourceProperty = DependencyProperty.Register(
			name: "DarkThemeImageSource", propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
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

        public string LightThemeImageSource
		{
            get => (string)GetValue(dp: LightThemeImageSourceProperty);
            set => SetValue(dp: LightThemeImageSourceProperty, value: value);
        }

		public string DarkThemeImageSource
		{
			get => (string)GetValue(dp: DarkThemeImageSourceProperty);
			set => SetValue(dp: DarkThemeImageSourceProperty, value: value);
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