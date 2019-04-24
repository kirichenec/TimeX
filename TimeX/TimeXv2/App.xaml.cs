using GalaSoft.MvvmLight.Threading;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace TimeXv2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region ctor
        static App()
        {
            DispatcherHelper.Initialize();
        }
        #endregion

        #region Methods

        #region Application_LoadCompleted | Обработчик события загрузки приложения
        /// <summary>
        /// Обработчик события загрузки приложения
        /// </summary>
        private void Application_LoadCompleted(object sender, NavigationEventArgs e)
        {
            App.Current.MainWindow.ContentRendered += ClearMainWindowHistory;
        }
        #endregion

        #region ClearMainWindowHistory | Очистка истории навигации
        /// <summary>
        /// Очистка истории навигации
        /// </summary>
        private void ClearMainWindowHistory(object sender, EventArgs e)
        {
            var mainWindow = (NavigationWindow)App.Current.MainWindow;
            while (mainWindow.CanGoBack)
                mainWindow.RemoveBackEntry();
        }
        #endregion

        #endregion
    }
}
