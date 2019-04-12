using System;
using System.Windows;

namespace TimeXv2.Static
{
    public class Properties : DependencyObject
    {
        public bool Topmost
        {
            get { return (bool)GetValue(TopmostProperty); }
            set { SetValue(TopmostProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Topmost.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopmostProperty =
            DependencyProperty.Register("Topmost", typeof(bool), typeof(Properties), new PropertyMetadata(false));

        public static Properties Instance { get; private set; }

        static Properties()
        {
            Instance = new Properties();
        }
    }
}
