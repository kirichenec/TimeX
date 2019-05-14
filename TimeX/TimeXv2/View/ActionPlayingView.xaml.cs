using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using TimeXv2.ViewModel;

namespace TimeXv2.View
{
    /// <summary>
    /// Interaction logic for ActionPlayingView.xaml
    /// </summary>
    public partial class ActionPlayingView : Page
    {
        #region ctor
        public ActionPlayingView()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region ActionMediaElementLoaded
        private void ActionMediaElementLoaded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).MediaEnded += ActionMediaElementRestartPlaying;
        }
        #endregion

        #region ActionMediaElementRestartPlaying
        private void ActionMediaElementRestartPlaying(object sender, RoutedEventArgs e)
        {
            var actionMediaElement = sender as MediaElement;
            actionMediaElement.Position = TimeSpan.Zero;
            actionMediaElement.Play();
        }
        #endregion

        #region ActionMediaElementUnloaded
        private void ActionMediaElementUnloaded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).MediaEnded -= ActionMediaElementRestartPlaying;
        }
        #endregion

        #region CloseDialog
        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        #endregion

        #endregion
    }
}
