using System.Windows;
using System.Windows.Controls;

namespace TimeXv2.Controls
{
    public class WaitingControl : Control
    {
        #region ctor
        static WaitingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitingControl), new FrameworkPropertyMetadata(typeof(WaitingControl)));
        }
        #endregion

        #region Properties
        
        #region Message
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                nameof(Message),
                typeof(string),
                typeof(WaitingControl),
                new PropertyMetadata(string.Empty));
        #endregion
        
        #endregion
    }
}
