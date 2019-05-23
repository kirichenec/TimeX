using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using TimeXv2.Model;

namespace TimeXv2.Controls
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        #region ctor
        public SettingsControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties

        #region EditedSettings
        public LightSettings EditedSettings
        {
            get { return (LightSettings)GetValue(EditedSettingsProperty); }
            set { SetValue(EditedSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedSettingsProperty =
            DependencyProperty.Register(
                nameof(EditedSettings),
                typeof(LightSettings),
                typeof(SettingsControl),
                new PropertyMetadata(new LightSettings()));
        #endregion

        #region IsAdvancedSettings
        public bool IsAdvancedSettings
        {
            get { return (bool)GetValue(IsAdvancedSettingsProperty); }
            set { SetValue(IsAdvancedSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdvancedSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAdvancedSettingsProperty =
            DependencyProperty.Register("IsAdvancedSettings", typeof(bool), typeof(SettingsControl), new PropertyMetadata(false));
        #endregion

        #endregion

        #region Methods

        #region CloseDialog
        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        #endregion

        #region ControlLoaded
        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            this.EditedSettings = new LightSettings(App.Settings);
        }
        #endregion

        #region SaveDialog
        private void SaveDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        #endregion

        #endregion
    }
}
