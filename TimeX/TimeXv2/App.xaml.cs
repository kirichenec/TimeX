using GalaSoft.MvvmLight.Threading;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Navigation;
using TimeXv2.Model;
using UniversalKLibrary.Classic.Simplificators;
using FormsNamespace = System.Windows.Forms;

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

        #region Fields
        private bool isFirstLoad = true;
        private FormsNamespace.NotifyIcon _notifyIcon;
        private WindowState _notMinimazedState;
        #endregion

        #region Properties

        #region Settings
        public static Settings Settings { get; set; }
        #endregion

        #endregion

        #region Methods

        #region Application_LoadCompleted | Обработчик события загрузки приложения
        /// <summary>
        /// Обработчик события загрузки приложения
        /// </summary>
        private void Application_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (isFirstLoad)
            {
                CreateNotifyicon();

                var window = App.Current.MainWindow;
                window.ContentRendered += ClearMainWindowHistory;
                window.Closing += SaveSettings;
                window.Closing += DeleteNotifyicon;
                window.StateChanged += OnWindowStateChangoed;

                // %LocalAppData%\IsolatedStorage

                LoadSettings(window);

                isFirstLoad = false;
            }
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

        #region CreateNotifyicon
        private void CreateNotifyicon()
        {
            var closeAppMenuItem = new FormsNamespace.MenuItem
            {
                Index = 0,
                Text = "Выход"
            };
            closeAppMenuItem.Click += OnMenuItemAppCloseClick;

            var resetAppWindowSettingsMenuItem = new FormsNamespace.MenuItem
            {
                Index = 1,
                Text = "Сброс настроек окна"
            };
            resetAppWindowSettingsMenuItem.Click += OnMenuItemAppWindowResetSettings;

            var contextMenu = new FormsNamespace.ContextMenu();
            contextMenu.MenuItems.AddRange(new FormsNamespace.MenuItem[] { resetAppWindowSettingsMenuItem, closeAppMenuItem });

            _notifyIcon = new FormsNamespace.NotifyIcon(new Container())
            {
                Icon = new Icon(@"Media\Action.ico"),

                ContextMenu = contextMenu,

                Text = "Мероприятия",
                Visible = true
            };

            _notifyIcon.DoubleClick += OnAppNotifyIconDoubleClick;
        }
        #endregion

        #region DeleteNotifyicon
        public void DeleteNotifyicon(object sender, EventArgs e)
        {
            _notifyIcon.Dispose();
        }
        #endregion

        #region LoadSettings | Загрузка настроек
        private static void LoadSettings(Window window)
        {
            var loadedSettings =
                            IsolatedIO.LoadData<LightSettings>(nameof(TimeXv2))
                            ?? new LightSettings()
                            {
                                Top = 10,
                                Left = 10,
                                Width = 800,
                                Height = 600,
                                IsDarkTheme = false,
                                WindowState = WindowState.Normal
                            };

            Settings = new Settings(window);
            loadedSettings.FillSettings(Settings);

            new MaterialDesignThemes.Wpf.PaletteHelper().SetLightDark(App.Settings.IsDarkTheme);
        }
        #endregion

        #region SaveSettings | Сохранение настроек
        public static void SaveSettings()
        {
            IsolatedIO.SaveData(Settings, nameof(TimeXv2));
        }

        private void SaveSettings(object sender, EventArgs e)
        {
            SaveSettings();
        }
        #endregion

        #region OnAppNotifyIconDoubleClick
        private void OnAppNotifyIconDoubleClick(object Sender, EventArgs e)
        {
            var window = Application.Current.MainWindow;
            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = _notMinimazedState;
            }
            else
            {
                window.WindowState = WindowState.Minimized;
            }
        }
        #endregion

        #region OnMenuItemAppCloseClick
        private void OnMenuItemAppCloseClick(object Sender, EventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        #endregion

        #region OnMenuItemAppWindowResetSettings
        private void OnMenuItemAppWindowResetSettings(object Sender, EventArgs e)
        {
            Settings.Top = 10;
            Settings.Left = 10;
            Settings.Width = 800;
            Settings.Height = 600;
            Settings.IsDarkTheme = false;
            Settings.WindowState = WindowState.Normal;
        }
        #endregion

        #region OnWindowStateChangoed
        private void OnWindowStateChangoed(object sender, EventArgs e)
        {
            var window = Application.Current.MainWindow;
            if (window.WindowState == WindowState.Minimized)
            {
                return;
            }
            _notMinimazedState = window.WindowState;
        }
        #endregion

        #endregion
    }
}
