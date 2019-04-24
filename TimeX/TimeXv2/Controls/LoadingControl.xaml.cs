using System.Windows;
using System.Windows.Controls;

namespace TimeXv2.Controls
{
    /// <summary>
    /// Interaction logic for LoadingControl.xaml
    /// </summary>
    public partial class LoadingControl : UserControl
    {
        #region ctor
        public LoadingControl()
        {
            InitializeComponent();
        }
        #endregion

        #region LoadingText
        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadingText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register(
                nameof(LoadingText),
                typeof(string),
                typeof(LoadingControl),
                new PropertyMetadata(string.Empty));
        #endregion
    }
}
