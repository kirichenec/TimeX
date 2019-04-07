using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeXv2.Controls
{
    /// <summary>
    /// Логика взаимодействия для CalendarControl.xaml
    /// </summary>
    public partial class CalendarControl : UserControl
    {
        public CalendarControl()
        {
            InitializeComponent();

            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue);
            CallsCalendar.BlackoutDates.Add(cdr);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void OnDateSelect(object sender, SelectionChangedEventArgs e)
        {
            if (CallsCalendar.SelectedDate != null)
            {
                CallsCalendar.DisplayDate = (DateTime)CallsCalendar.SelectedDate;
            }
        }

        private void CloseButton(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
