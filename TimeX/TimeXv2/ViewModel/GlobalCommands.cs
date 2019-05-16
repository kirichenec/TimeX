using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using TimeXv2.Model;
using TimeXv2.ViewModel.Navigation;

namespace TimeXv2.ViewModel
{
    public class GlobalViewModel : ViewModelBase
    {
        #region ctor
        public GlobalViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Services
        private readonly INavigationService _navigationService;
        #endregion

        #region Commands

        #region ChangeIsDarkCommand
        private RelayCommand<bool> _changeIsDarkCommand;

        public RelayCommand<bool> ChangeIsDarkCommand
        {
            get
            {
                return _changeIsDarkCommand
                    ?? (_changeIsDarkCommand = new RelayCommand<bool>(
                        isDarkTheme =>
                        {
                            new PaletteHelper().SetLightDark(isDarkTheme);
                        }));
            }
        }
        #endregion

        #region ChangeMediaCommand
        private RelayCommand<LightSettings> _changeMediaCommand;

        public RelayCommand<LightSettings> ChangeMediaCommand
        {
            get
            {
                return _changeMediaCommand
                    ?? (_changeMediaCommand = new RelayCommand<LightSettings>(
                        settings =>
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter =
                                "Audio Files|" +
                                "*.wav;*.aac;*.mp3;*.WMA;*.MP3;";
                            if (openFileDialog.ShowDialog() == true)
                                settings.AlarmRing = new Uri(openFileDialog.FileName);
                        }));
            }
        }
        #endregion

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

        #region CloseSettingsCommand
        private RelayCommand _closeSettingsCommand;

        public RelayCommand CloseSettingsCommand
        {
            get
            {
                return _closeSettingsCommand
                    ?? (_closeSettingsCommand = new RelayCommand(
                        () =>
                        {
                            new PaletteHelper().SetLightDark(App.Settings.IsDarkTheme);
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

        #region SaveSettingsCommand
        private RelayCommand<LightSettings> _saveSettingsCommand;

        public RelayCommand<LightSettings> SaveSettingsCommand
        {
            get
            {
                return _saveSettingsCommand
                    ?? (_saveSettingsCommand = new RelayCommand<LightSettings>(
                        lightSettings =>
                        {
                            if (lightSettings != null)
                            {
                                lightSettings.FillSettings(App.Settings);
                            }
                            App.SaveSettings();
                            Static.Properties.Instance.AlarmRing = App.Settings.AlarmRing;
                        }));
            }
        }
        #endregion

        #region TestCommand
        //Command="{Binding GlobalViewModel.TestCommand, Source={StaticResource Locator}}"

        private RelayCommand _testCommand;

        public RelayCommand TestCommand
        {
            get
            {
                return _testCommand
                    ?? (_testCommand = new RelayCommand(
                    () =>
                    {
                        var i = 1;
                    }));
            }
        }
        #endregion

        #endregion
    }
}
