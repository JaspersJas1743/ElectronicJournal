using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicJournal.Resources.CustomElements
{
    public partial class ToggleSwitch : UserControl
    {
        #region Fields
        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
            name: "IsOn", propertyType: typeof(Boolean), ownerType: typeof(ToggleSwitch)
        );

        public static readonly DependencyProperty ToggledCommandProperty = DependencyProperty.Register(
            name: "ToggledCommand", propertyType: typeof(ICommand), ownerType: typeof(ToggleSwitch)
        );

        public static readonly DependencyProperty OnToggledCommandParameterProperty = DependencyProperty.Register(
            name: "OnToggledCommandParameter", propertyType: typeof(Object), ownerType: typeof(ToggleSwitch)
        );

        public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(
            name: "OnContent", propertyType: typeof(Object), ownerType: typeof(ToggleSwitch)
        );

        public static readonly DependencyProperty OffToggledCommandParameterProperty = DependencyProperty.Register(
            name: "OffToggledCommandParameter", propertyType: typeof(Object), ownerType: typeof(ToggleSwitch)
        );

        public static readonly DependencyProperty OffContentProperty = DependencyProperty.Register(
            name: "OffContent", propertyType: typeof(Object), ownerType: typeof(ToggleSwitch)
        );
        #endregion Fields

        #region Constructors
        public ToggleSwitch()
        {
            InitializeComponent();
        }
        #endregion Constructors

        #region Properties
        public bool IsOn
        {
            get => (bool)GetValue(dp: IsOnProperty);
            set => SetValue(dp: IsOnProperty, value: value);
        }

        public ICommand ToggledCommand
        {
            get => (ICommand)GetValue(dp: ToggledCommandProperty);
            set => SetValue(dp: ToggledCommandProperty, value: value);
        }

        public object OnToggledCommandParameter
        {
            get => GetValue(dp: OnToggledCommandParameterProperty);
            set => SetValue(dp: OnToggledCommandParameterProperty, value: value);
        }

        public object OffToggledCommandParameter
        {
            get => GetValue(dp: OffToggledCommandParameterProperty);
            set => SetValue(dp: OffToggledCommandParameterProperty, value: value);
        }

        public object OnContent
        {
            get => GetValue(dp: OnContentProperty);
            set => SetValue(dp: OnContentProperty, value: value);
        }

        public object OffContent
        {
            get => GetValue(dp: OffContentProperty);
            set => SetValue(dp: OffContentProperty, value: value);
        }
        #endregion Properties
    }
}
