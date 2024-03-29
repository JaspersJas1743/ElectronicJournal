﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicJournal.Resources.CustomElements
{
    public partial class PasswordBoxWithImageAndPasswordViewer : UserControl
    {
        #region Fields  
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: nameof(Text), propertyType: typeof(String), ownerType: typeof(PasswordBoxWithImageAndPasswordViewer)
        );

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            name: nameof(Image), propertyType: typeof(Style), ownerType: typeof(PasswordBoxWithImageAndPasswordViewer)
        );

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            name: nameof(Placeholder), propertyType: typeof(String), ownerType: typeof(PasswordBoxWithImageAndPasswordViewer)
        );
        #endregion Fields

        #region Constructor
        public PasswordBoxWithImageAndPasswordViewer()
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
        #endregion Properties

        #region Methods
        private void OnPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
            => e.Handled = new[] { ApplicationCommands.Copy, ApplicationCommands.Cut }.Contains(e.Command);

        private void OnHiddenPasswordClick(object sender, RoutedEventArgs e)
            => ChangeVisibilityButtons(weight: FontWeights.Normal);

        private void OnShowPasswordClick(object sender, RoutedEventArgs e)
            => ChangeVisibilityButtons(weight: FontWeights.SemiBold);

        private void ChangeVisibilityButtons(FontWeight weight)
        {
            (ShowPassword.Visibility, HiddenPassword.Visibility) = (HiddenPassword.Visibility, ShowPassword.Visibility);
            Tb.FontWeight = weight;
            Tb.Focus();
        }
        #endregion Methods
    }
}
