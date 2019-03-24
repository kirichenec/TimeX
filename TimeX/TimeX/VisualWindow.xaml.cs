using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimeX
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class VisualWindow : Window
    {
        /// <summary>
        /// Мероприятие для отображения
        /// </summary>
        private Action action;
        /// <summary>
        /// Таймер для выведения попапа
        /// </summary>
        private DispatcherTimer timer;
        /// <summary>
        /// Параметр закрытия(true)/скрытия(false) окна
        /// </summary>
        private bool applicationClosing = false;
        /// <summary>
        /// Показ/скрытие отображения подсказки с текущими событиями и временем Ч
        /// </summary>
        public bool ShowPopup;
        /// <summary>
        /// Время последнего шевеления над прогрессбыром мероприятия
        /// </summary>
        private DateTime showTime;

        /// <summary>
        /// Мероприятие для отображения
        /// </summary>
        public Action Action
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
                this.DataContext = action;
                if (value != null)
                {
                    // Отображение по умолчанию у мероприятия списка "План-Текущ-Вып"
                    Binding bind = new Binding("CurrentAndLastNext");
                    bind.Mode = BindingMode.OneWay;
                    EventsListBox.SetBinding(ListBox.ItemsSourceProperty, bind);
                    // Нанесение меток чекпоинтов
                    //CheckPoints.Children.Clear();
                    //for (int i = 0; i < Action.Checkpoints.Count; i++)   // нанесение штрихов
                    //{
                    //    Grid tempGrid = new Grid();
                    //    tempGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    //    tempGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    //    tempGrid.ColumnDefinitions[0].Width =
                    //        new GridLength(Action.Checkpoints[i], GridUnitType.Star);
                    //    tempGrid.ColumnDefinitions[1].Width =
                    //        new GridLength(Action.Duration.Ticks - Action.Checkpoints[i], GridUnitType.Star);
                    //    Border border = new Border();
                    //    border.Background = null;
                    //    border.BorderThickness = new Thickness(0, 0, 1, 0);
                    //    border.BorderBrush = Brushes.Black;
                    //    border.MouseEnter += CheckPoint_MouseEnter;
                    //    tempGrid.Children.Add(border);
                    //    Grid.SetColumn(border, 0);
                    //    CheckPoints.Children.Add(tempGrid);
                    //}
                }
                else
                {
                    EventsListBox.ItemsSource = null;
                    //CheckPoints.Children.Clear();
                }
            }
        }

        /// <summary>
        /// Простой конструктор
        /// </summary>
        public VisualWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();                     // Таймер для обновления инфы о мероприятии
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);    // Задание отрезка времени (1 мс)
            timer.Tick += new EventHandler(Timer_Tick);        // Событие прошествия очередного отрезка времени
            timer.Start();
        }

        /// <summary>
        /// Полное закрытие окна
        /// </summary>
        public void AppClosing()
        {
            applicationClosing = true;
            this.Close();
        }

        /// <summary>
        /// Изменение времени Ч при проведении над прогрессбаром курсора
        /// </summary>
        private void ActionProgress_MouseMove(object sender, MouseEventArgs e)
        {
            CursorPosition.Width = Mouse.GetPosition(ActionProgress).X; // ширина риски = положение мыши по оси Х относительно прогрессбара
            showTime = DateTime.Now;   // запоминание последнего времени шевеления
        }
        
        /// <summary>
        /// Вывставление позиции отметки курсора согласно ширине чекпоинта
        /// </summary>
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            CursorPosition.Width = ((Border)sender).ActualWidth;
        }

        /// <summary>
        /// Показ/скрытие полного списка событий мероприятия
        /// </summary>
        private void FullEventsList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (EventsListBox.ItemsSource == this.Action.Items)
            {
                Binding bind = new Binding("CurrentAndLastNext");
                bind.Mode = BindingMode.OneWay;
                EventsListBox.SetBinding(ListBox.ItemsSourceProperty, bind);
                FullEventsList.ToolTip = "c";
            }
            else
            {
                Binding bind = new Binding("Items");
                bind.Mode = BindingMode.OneWay;
                EventsListBox.SetBinding(ListBox.ItemsSourceProperty, bind);
                FullEventsList.ToolTip = "Отобразить краткий список событий";
                EventsListBox.ScrollIntoView(action.CurrentEvents.Count == 0 ? action.Items[0] : action.CurrentEvents[0]);
            }
        }
        
        /// <summary>
        /// Действия при открытии 
        /// </summary>
        private void ProgressBarPopup_Opened(object sender, EventArgs e)
        {
            double mouseXpos = CursorPosition.ActualWidth;
            //ProgressBarPopup.HorizontalOffset = mouseXpos - (PopupBorder.ActualWidth / 2) - 1;
            TimeSpan mouseTime =                                // время, на которое указывает мышь
                TimeSpan.FromSeconds(
                    action.Duration.TotalSeconds *              // длительность мероприятия
                    mouseXpos /                                 // мышь относительно прогрессбара
                    ActionProgress.ActualWidth);                // ширина попапа
            PopupLabel.Content = mouseTime.Days != 0 ?
                "Ч + " +
                mouseTime.Days + "д " + mouseTime.Hours.ToString("D2") + ":" + mouseTime.Minutes.ToString("D2") :
                "Ч + " +
                mouseTime.Hours.ToString("D2") + ":" + mouseTime.Minutes.ToString("D2");
            PopupEvents.ItemsSource = this.Action.AtTime(mouseTime);
            ProgressBarPopup.HorizontalOffset = mouseXpos - (PopupBorder.ActualWidth / 2) - 1;
        }

        /// <summary>
        /// Метод, изменяющий переменную текущего времени
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            ShowPopup = showTime.AddMilliseconds(10) < DateTime.Now;
            if (ShowPopup & ActionInfo.IsMouseCaptureWithin)
            {
                CursorPosition.Width = Mouse.GetPosition(ActionProgress).X;
            }
        }

        /// <summary>
        /// Скрытие/закрытие окна
        /// </summary>
        private void VisaulForm_Closing(object sender, CancelEventArgs e)
        {
            if (!applicationClosing)
            {
                e.Cancel = true;
                action.Started = false;
                this.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Полноэкранный режим F11
        /// </summary>
        private void VisaulForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (this.WindowStyle == WindowStyle.SingleBorderWindow)
                {
                    this.WindowState = WindowState.Maximized;
                    this.WindowStyle = WindowStyle.None;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                }
            }
        }
    }
}
