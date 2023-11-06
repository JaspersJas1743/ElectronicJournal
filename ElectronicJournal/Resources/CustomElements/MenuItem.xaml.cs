using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.Resources.CustomElements
{
    public partial class MenuItem : UserControl
    {
        #region Fields
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            name: nameof(Image), propertyType: typeof(Style), ownerType: typeof(MenuItem)
        );

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: nameof(Text), propertyType: typeof(String), ownerType: typeof(MenuItem)
        );
        #endregion Fields

        #region Constructors
        public MenuItem()
            => InitializeComponent();
        #endregion Constructors

        #region Properties
        public Style Image
        {
            get => (Style)GetValue(dp: ImageProperty);
            set => SetValue(dp: ImageProperty, value: value);
        }

        public string Text
        {
            get => (string)GetValue(dp: TextProperty);
            set => SetValue(dp: TextProperty, value: value);
        }
        #endregion Properties
    }
}
