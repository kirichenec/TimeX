using System.Windows;
using System.Windows.Controls;

namespace TimeXv2.Controls
{
    /// <summary>
    /// Interaction logic for NumberEditor.xaml
    /// </summary>
    public partial class NumberEditor : UserControl
    {
        #region ctor
        public NumberEditor()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties

        #region HintText
        public string HintText
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(nameof(HintText), typeof(string), typeof(NumberEditor), new PropertyMetadata(nameof(HintText)));
        #endregion

        #region Value
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumberEditor), new PropertyMetadata(0));
        #endregion

        #endregion

        #region Methods

        #region ValueUp
        private void ValueUp(object sender, RoutedEventArgs e)
        {
            this.Value += 1;
        }
        #endregion

        #region ValueDown
        private void ValueDown(object sender, RoutedEventArgs e)
        {
            this.Value -= 1;
        }
        #endregion

        #endregion
    }
}
