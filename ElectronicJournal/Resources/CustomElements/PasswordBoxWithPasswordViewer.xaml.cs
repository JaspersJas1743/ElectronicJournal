using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ElectronicJournal.Resources.CustomElements
{
    public partial class PasswordBoxWithPasswordViewer : UserControl
    {
        #region Fields
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: nameof(Text), propertyType: typeof(String), ownerType: typeof(PasswordBoxWithPasswordViewer)
        );
        #endregion Fields

        #region Constructor
        public PasswordBoxWithPasswordViewer()
            => InitializeComponent();
        #endregion Constructor

        #region Properties
        public string Text
        {
            get => (string)GetValue(dp: TextProperty);
            set => SetValue(dp: TextProperty, value: value);
        }
        #endregion Properties

        #region Methods
        private void OnMainTextBoxPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
            => e.Handled = new[] { ApplicationCommands.Copy, ApplicationCommands.Cut }.Contains(e.Command);

        private void OnHiddenPasswordClick(object sender, RoutedEventArgs e)
        {
            Button hidden = (Button)sender;
            ChangeVisibilityButtons(weight: FontWeights.Normal, family: (FontFamily)Application.Current.FindResource(resourceKey: "PasswordFont"), show: (Button)hidden.Tag, hidden: hidden);
        }

        private void OnShowPasswordClick(object sender, RoutedEventArgs e)
        {
            Button show = (Button)sender;
            ChangeVisibilityButtons(weight: FontWeights.SemiBold, family: new FontFamily(familyName: "Raleway"), show: show, hidden: (Button)show.Tag);
        }

        private void ChangeVisibilityButtons(FontWeight weight, FontFamily family, Button show, Button hidden)
        {
            (show.Visibility, hidden.Visibility) = (hidden.Visibility, show.Visibility);
            MainTextBox.FontFamily = family;
            MainTextBox.FontWeight = weight;
            MainTextBox.Focus();
        }
        #endregion Methods
    }
}
