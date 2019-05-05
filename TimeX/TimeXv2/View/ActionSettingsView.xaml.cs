using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace TimeXv2.View
{
    /// <summary>
    /// Логика взаимодействия для ActionSettingsView.xaml
    /// </summary>
    public partial class ActionSettingsView : Page
    {
        public ActionSettingsView()
        {
            InitializeComponent();
        }

        private void CloseDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
