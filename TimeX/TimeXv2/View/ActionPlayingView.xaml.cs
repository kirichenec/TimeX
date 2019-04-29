using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace TimeXv2.View
{
    /// <summary>
    /// Interaction logic for ActionPlayingView.xaml
    /// </summary>
    public partial class ActionPlayingView : Page
    {
        public ActionPlayingView()
        {
            InitializeComponent();
        }

        private void CloseDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
