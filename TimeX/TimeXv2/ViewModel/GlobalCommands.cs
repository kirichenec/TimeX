using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace TimeXv2.ViewModel
{
    public class GlobalCommands : ViewModelBase
    {
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
    }
}
