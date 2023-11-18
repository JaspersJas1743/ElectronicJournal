using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.Resources.CustomElements
{
    public partial class TextBoxWithImageAndPlaceholder : UserControl
    {
        #region Fields
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: nameof(Text), propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
        );

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            name: nameof(Image), propertyType: typeof(Style), ownerType: typeof(TextBoxWithImageAndPlaceholder)
        );

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            name: nameof(Placeholder), propertyType: typeof(String), ownerType: typeof(TextBoxWithImageAndPlaceholder)
        );

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            name: nameof(MaxLength), propertyType: typeof(Int32), ownerType: typeof(TextBoxWithImageAndPlaceholder)
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

        public int MaxLength
        {
            get => (int)GetValue(dp: MaxLengthProperty);
            set => SetValue(dp: MaxLengthProperty, value: value);
        }

        public void SetSelectionStart(int selectionStart)
            => MainTextBox.SelectionStart = selectionStart;
        #endregion Properties

        #region Methods
        public new void Focus()
            => MainTextBox.Focus();
        #endregion Methods
    }
}