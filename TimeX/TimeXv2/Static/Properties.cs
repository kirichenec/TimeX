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

        #region Properties

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
