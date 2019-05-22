using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TimeXv2.Static
{
    public class Properties : DependencyObject
    {
        #region ctor
        static Properties()
        {
            Instance = new Properties();
        }
        #endregion

        #region Fields
        private static readonly double _messageVisibilityDuration = 5;
        #endregion

        #region Properties

        #region AlarmRing
        public Uri AlarmRing
        {
            get { return (Uri)GetValue(AlarmRingProperty); }
            set { SetValue(AlarmRingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AlarmRing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlarmRingProperty =
            DependencyProperty.Register(
                nameof(AlarmRing),
                typeof(Uri),
                typeof(Properties),
                new PropertyMetadata(new Uri(@"C:\Windows\media\Alarm10.wav")));
        #endregion

        #region IsAlarmMuted
        public bool IsAlarmMuted
        {
            get { return (bool)GetValue(IsAlarmOnProperty); }
            set { SetValue(IsAlarmOnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAlarmOn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAlarmOnProperty =
            DependencyProperty.Register(
                nameof(IsAlarmMuted),
                typeof(bool),
                typeof(Properties),
                new PropertyMetadata(false));
        #endregion

        #region MessageQueue
        public SnackbarMessageQueue MessageQueue
        {
            get { return (SnackbarMessageQueue)GetValue(MessageQueueProperty); }
            set { SetValue(MessageQueueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageQueue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageQueueProperty =
            DependencyProperty.Register(
                nameof(MessageQueue),
                typeof(SnackbarMessageQueue),
                typeof(Properties),
                new PropertyMetadata(new SnackbarMessageQueue(TimeSpan.FromSeconds(_messageVisibilityDuration))));
        #endregion

        #region Topmost
        public bool Topmost
        {
            get { return (bool)GetValue(TopmostProperty); }
            set { SetValue(TopmostProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Topmost.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopmostProperty =
            DependencyProperty.Register(
                nameof(Topmost),
                typeof(bool),
                typeof(Properties),
                new PropertyMetadata(false));
        #endregion

        #endregion

        #region Methods
        public static void ShowMessage(string message)
        {
            Instance.MessageQueue.Enqueue(
                message,
                "OK",
                _ => { },
                message);
        }
        #endregion

        public static Properties Instance { get; private set; }
    }

    public class ValueToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((double)value * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)((int)value / 100);
        }
    }
}
