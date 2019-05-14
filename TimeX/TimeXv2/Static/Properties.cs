using MaterialDesignThemes.Wpf;
using System;
using System.Windows;

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
        private static double _messageVisibilityDuration = 5;
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
            DependencyProperty.Register("AlarmRing", typeof(Uri), typeof(Properties), new PropertyMetadata(new Uri(@"C:\Windows\media\Alarm01.wav")));
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

        public static Properties Instance { get; private set; }
    }
}
