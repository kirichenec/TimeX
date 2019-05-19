using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.Model
{
    public class Settings : SimplePropertyChanged, IDisposable
    {
        #region ctor
        public Settings() { }

        public Settings(Window window)
        {
            this.Window = window;
        }
        #endregion

        #region Properties

        #region AlarmRing
        private Uri _alarmRing = new Uri(@"C:\Windows\media\Alarm01.wav");

        public Uri AlarmRing
        {
            get { return _alarmRing; }
            set
            {
                if (_alarmRing == value)
                {
                    return;
                }
                _alarmRing = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region IsDarkTheme
        private bool _isDarkTheme = false;

        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                if (_isDarkTheme == value)
                {
                    return;
                }
                _isDarkTheme = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Topmost
        public bool Topmost
        {
            get { return _window.Topmost; }
            set
            {
                if (_window.Topmost == value)
                {
                    return;
                }
                _window.Topmost = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Window
        private Window _window;

        [JsonIgnore]
        [NotMapped]
        public Window Window
        {
            get { return _window; }
            set
            {
                if (_window == value)
                {
                    return;
                }
                _window = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region WindowSize

        #region Height
        public double Height
        {
            get { return _window.Height; }
            set
            {
                if (_window.Height == value)
                {
                    return;
                }
                _window.Height = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Left
        public double Left
        {
            get { return _window.Left; }
            set
            {
                if (_window.Left == value)
                {
                    return;
                }
                _window.Left = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Top
        public double Top
        {
            get { return _window.Top; }
            set
            {
                if (_window.Top == value)
                {
                    return;
                }
                _window.Top = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Width
        public double Width
        {
            get { return _window.Width; }
            set
            {
                if (_window.Width == value)
                {
                    return;
                }
                _window.Width = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region WindowState
        public WindowState WindowState
        {
            get { return _window.WindowState; }
            set
            {
                if (_window.WindowState == value)
                {
                    return;
                }
                _window.WindowState = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #endregion

        #endregion

        #region Methods

        #region Dispose
        public void Dispose()
        {
            this.Window?.Close();
        }
        #endregion

        #endregion
    }

    public class LightSettings : SimplePropertyChanged
    {
        #region ctor
        public LightSettings() { }

        public LightSettings(Settings value)
        {
            this.AlarmRing = value.AlarmRing;
            this.Height = value.Height;
            this.IsDarkTheme = value.IsDarkTheme;
            this.Left = value.Left;
            this.Top = value.Top;
            this.Topmost = value.Topmost;
            this.Width = value.Width;
            this.WindowState = value.WindowState;
        }
        #endregion

        #region Properties

        #region AlarmRing
        private Uri _alarmRing = new Uri(@"C:\Windows\media\Alarm01.wav");

        public Uri AlarmRing
        {
            get { return _alarmRing; }
            set
            {
                if (_alarmRing == value)
                {
                    return;
                }
                _alarmRing = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region IsDarkTheme
        private bool _isDarkTheme;

        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                if (_isDarkTheme == value)
                {
                    return;
                }
                _isDarkTheme = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Topmost
        public bool Topmost { get; set; }
        #endregion

        #region WindowSize

        #region Height
        public double Height { get; set; }
        #endregion

        #region Left
        public double Left { get; set; }
        #endregion

        #region Top
        public double Top { get; set; }
        #endregion

        #region Width
        public double Width { get; set; }
        #endregion

        #region WindowState
        public WindowState WindowState { get; set; }
        #endregion

        #endregion

        #endregion

        #region Methods

        #region FillSettings
        public void FillSettings(Settings settings)
        {
            settings.AlarmRing = this.AlarmRing;
            settings.Height = this.Height;
            settings.IsDarkTheme = this.IsDarkTheme;
            settings.Left = this.Left;
            settings.Top = this.Top;
            settings.Topmost = this.Topmost;
            settings.Width = this.Width;
            settings.WindowState = this.WindowState;
        }
        #endregion

        #endregion
    }
}
