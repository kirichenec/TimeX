using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using TimeXv2.ViewModel.Navigation;

namespace TimeXv2.ViewModel
{
    public class GlobalCommands : ViewModelBase
    {
        #region ctor
        public GlobalCommands(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Services
        private readonly INavigationService _navigationService;
        #endregion

        #region Commands

        #region CloseAppCommand
        private RelayCommand _closeAppCommand;

        /// <summary>
        /// Gets the CloseAppCommand.
        /// </summary>
        public RelayCommand CloseAppCommand
        {
            get
            {
                return _closeAppCommand
                    ?? (_closeAppCommand = new RelayCommand(
                    () =>
                    {
                        App.Current.MainWindow.Close();
                    }));
            }
        }
        #endregion

        #region InvertTopmostCommand
        private RelayCommand _invertTopmostCommand;

        /// <summary>
        /// Gets the InvertTopmostCommand.
        /// </summary>
        public RelayCommand InvertTopmostCommand
        {
            get
            {
                return _invertTopmostCommand
                    ?? (_invertTopmostCommand = new RelayCommand(
                    () =>
                    {
                        Static.Properties.Instance.Topmost = !Static.Properties.Instance.Topmost;
                    }));
            }
        }
        #endregion

        #region MainPageCommand
        private RelayCommand _mainPageCommand;

        public RelayCommand MainPageCommand
        {
            get
            {
                return _mainPageCommand
                    ?? (_mainPageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.Navigate(NavPage.Main);
                    }));
            }
        }
        #endregion

        #region OpenUriExternalCommand
        private RelayCommand<string> _openUriExternalCommand;

        public RelayCommand<string> OpenUriExternalCommand
        {
            get
            {
                return _openUriExternalCommand
                    ?? (_openUriExternalCommand = new RelayCommand<string>(
                    uri =>
                    {
                        if (!string.IsNullOrEmpty(uri))
                        {
                            Process.Start(uri);
                        }
                    }));
            }
        }
        #endregion

        #endregion
    }
}
