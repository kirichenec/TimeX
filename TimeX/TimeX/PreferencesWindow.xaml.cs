using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;
using TsivanyukModulus;

namespace TimeX
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Все мероприятия
        /// </summary>
        private Actions allActions = new Actions();
        /// <summary>
        /// Имя файла данных
        /// </summary>
        private string dataPath = "Data.ini";
        /// <summary>
        /// Редактируемое мероприятие
        /// </summary>
        private Action editedAction = new Action();
        /// <summary>
        /// Окно отображения мероприятия
        /// </summary>
        private VisualWindow outWindow = new VisualWindow();
        /// <summary>
        /// Имя файла настроек
        /// </summary>
        private string propertiesPath = "Properties.ini";

        /// <summary>
        /// Класс сохраняемых настроек окна
        /// </summary>
        [Serializable]
        public class WindowPreferences
        {
            /// <summary>
            /// Высота
            /// </summary>
            public double Height;

            /// <summary>
            /// Позиция левого края
            /// </summary>
            public double Left;

            /// <summary>
            /// Позиция верхнего края
            /// </summary>
            public double Top;

            /// <summary>
            /// Ширина
            /// </summary>
            public double Width;

            /// <summary>
            /// Состояние окна
            /// </summary>
            public WindowState WindowState;

            /// <summary>
            /// Конструктор класса сохраняемых настроек окна
            /// </summary>
            public WindowPreferences() { }
        }

        /// <summary>
        /// Конструктор. Первоначальная загрузка настроек окна и мероприятий
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // загрузка параметров окна
            try
            {
                FileXml<WindowPreferences> LoadWindowParams = new FileXml<WindowPreferences>();
                LoadWindowParams.path = propertiesPath;
                LoadWindowParams.Read();
                this.Height = LoadWindowParams.obj.Height;
                this.Left = LoadWindowParams.obj.Left;
                this.Top = LoadWindowParams.obj.Top;
                this.Width = LoadWindowParams.obj.Width;
                this.WindowState = LoadWindowParams.obj.WindowState;
            }
            catch
            {
                MessageBox.Show("Ошибка открытия файла настроек!\rУстановлены параметры окна по умолчанию", propertiesPath);
            }
            // загрузка мероприятий 
            try
            {
                FileXml<Actions> LoadActions = new FileXml<Actions>();
                LoadActions.path = dataPath;
                LoadActions.Read();
                allActions = LoadActions.obj;
                ActionsListBox.ItemsSource = allActions;
            }
            catch
            {
                MessageBox.Show("Ошибка открытия файла c мероприятиями!", dataPath);
            }
            // показ настроек мероприятий и скрытие отображения настройки мероприятия
            ActionsEditionGrid.Visibility = Visibility.Visible;
            MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            ActionEditionGrid.Visibility = Visibility.Collapsed;
            MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Auto);
            
            var voiceList = new SpeechSynthesizer().GetInstalledVoices();
            //testBox.ItemsSource = voiceList;
        }

        /// <summary>
        /// Сохранение времени при нажатии клавиши Enter в поле часов мероприятия на главном окне
        /// </summary>
        private void ActionHour_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ActionHour_LostFocus(sender, new RoutedEventArgs());
        }

        /// <summary>
        /// Сохранение времени при потере фокуса поля часов мероприятия на главном окне 
        /// </summary>
        private void ActionHour_LostFocus(object sender, RoutedEventArgs e)
        {
            int editedIndex = allActions.IndexOf((Action)((TextBox)sender).Tag);
            DateTime tempDate = allActions[editedIndex].StartTime;
            int x;
            if (!(((TextBox)sender).Text).Equals(""))
                if (Int32.TryParse(((TextBox)sender).Text, out x))
                {
                    tempDate = new DateTime(
                        tempDate.Year,
                        tempDate.Month,
                        tempDate.Day,
                        x,
                        tempDate.Minute,
                        0);
                }
                else
                {
                    ((TextBox)sender).Undo();
                }
            allActions[editedIndex].StartTime = tempDate;
            SaveActions();
        }

        /// <summary>
        /// Проверка ввода часов
        /// </summary>
        private void ActionHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            int x;
            if (!(((TextBox)sender).Text).Equals(""))
                if (int.TryParse(((TextBox)sender).Text, out x))
                    if (x > 23)
                        ((TextBox)sender).Text = "23";
        }

        /// <summary>
        /// Сохранение времени при нажатии клавиши Enter в поле минут мероприятия на главном окне
        /// </summary>
        private void ActionMinute_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ActionMinute_LostFocus(sender, new RoutedEventArgs());
        }

        /// <summary>
        /// Сохранение времени при потере фокуса поля минут мероприятия на главном окне
        /// </summary>
        private void ActionMinute_LostFocus(object sender, RoutedEventArgs e)
        {
            int x;
            int editedIndex = allActions.IndexOf((Action)((TextBox)sender).Tag);
            DateTime tempDate = allActions[editedIndex].StartTime;
            if (!(((TextBox)sender).Text).Equals(""))
                if (int.TryParse(((TextBox)sender).Text, out x))
                {
                    tempDate = new DateTime(
                        tempDate.Year,
                        tempDate.Month,
                        tempDate.Day,
                        tempDate.Hour,
                        x,
                        0);
                }
                else
                {
                    ((TextBox)sender).Undo();
                }
            allActions[editedIndex].StartTime = tempDate;
            SaveActions();
        }

        /// <summary>
        /// Проверка ввода минут
        /// </summary>
        private void ActionMinute_TextChanged(object sender, TextChangedEventArgs e)
        {
            int x;
            if (!(((TextBox)sender).Text).Equals(""))
                if (int.TryParse(((TextBox)sender).Text, out x))
                    if (x > 59)
                        ((TextBox)sender).Text = "59";
        }

        /// <summary>
        /// Добавление мероприятия
        /// </summary>
        private void AddImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ActionsListBox.SelectedIndex = -1;
            editedAction = new Action("Новое мероприятие");
            editedAction.StartTime = DateTime.Now;
            // Привязка к пустому мероприятию
            Binding bind = new Binding();
            bind.Source = editedAction;
            ActionEditionGrid.SetBinding(Grid.DataContextProperty, bind);
            ChangeProperties();
        }

        /// <summary>
        /// Отмена изменений редактирования мероприятия
        /// </summary>
        private void BackAction_Click(object sender, RoutedEventArgs e)
        {
            ChangeProperties();
        }

        /// <summary>
        /// Отмена изменений редактирования события
        /// </summary>
        private void BackEvent_Click(object sender, RoutedEventArgs e)
        {
            EventsListBox.SelectedIndex = -1;
            ClearEvent();
        }

        /// <summary>
        /// Смена вида окна редактора
        /// </summary>
        private void ChangeProperties()
        {
            if (ActionsEditionGrid.Visibility == Visibility.Visible)
            {
                ActionsEditionGrid.Visibility = Visibility.Collapsed;
                MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
                ActionEditionGrid.Visibility = Visibility.Visible;
                MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                ActionsEditionGrid.Visibility = Visibility.Visible;
                MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                ActionEditionGrid.Visibility = Visibility.Collapsed;
                MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Auto);
            }

        }

        /// <summary>
        /// Очистка поля редактирования события
        /// </summary>
        private void ClearEvent()
        {
            EventStartDay.Text = EventStartHour.Text = EventStartMins.Text =
                EventDurationDay.Text = EventDurationHour.Text = EventDurationMins.Text = "0";
            EventName.Text = "Новое событие";
            EditedEventTitle.Content = "Добавить событие:";
        }

        /// <summary>
        /// Удаление мероприятия
        /// </summary>
        private void DeleteActionImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (outWindow.Action == (Action)ActionsListBox.SelectedItem)
            {
                outWindow.Action.Started = false;
                outWindow.Close();
            }
            allActions.RemoveAt(ActionsListBox.SelectedIndex);
            SaveActions();
        }

        /// <summary>
        /// Удаление события
        /// </summary>
        private void DeleteEventImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            editedAction.Items.RemoveAt(EventsListBox.SelectedIndex);
        }

        /// <summary>
        /// Сохранение мероприятий при изменении даты в главном меню
        /// </summary>
        private void DatePicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SaveActions();
        }

        /// <summary>
        /// Редактирование мероприятия
        /// </summary>
        private void EditActionImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //EditedActionIndex = ActionsListBox.SelectedIndex;
            ChangeProperties();
            editedAction.CopyFrom((Action)ActionsListBox.SelectedItem);
            Binding bind = new Binding();
            bind.Mode = BindingMode.OneWay;
            bind.Source = editedAction;
            ActionEditionGrid.SetBinding(Grid.DataContextProperty, bind);
        }

        /// <summary>
        /// Редактироваие события
        /// </summary>
        private void EditEventImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Event temp = new Event((Event)EventsListBox.SelectedItem);
            EventName.Text = temp.Name;
            EventStartDay.Text = temp.StartTime.Days.ToString();
            EventStartHour.Text = temp.StartTime.Hours.ToString();
            EventStartMins.Text = temp.StartTime.Minutes.ToString();
            EventDurationDay.Text = temp.Duration.Days.ToString();
            EventDurationHour.Text = temp.Duration.Hours.ToString();
            EventDurationMins.Text = temp.Duration.Minutes.ToString();
            EditedEventTitle.Content = "Редактировать событие:";
        }

        /// <summary>
        /// Запуск/остановка мероприятия
        /// </summary>
        private void NowStartTimeImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ActionsListBox.SelectedIndex == -1)
            {
                editedAction.StartTime = DateTime.Now;
                editedAction.StartTime = DateTime.Now;
            }
            else
            {
                ((Action)ActionsListBox.SelectedItem).StartTime = DateTime.Now;
                ((Action)ActionsListBox.SelectedItem).StartTime = DateTime.Now;
                SaveActions();
            }
        }

        /// <summary>
        /// Установка текущего времени мероприятию в настройках
        /// </summary>
        private void NowTimeImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            editedAction.StartTime = DateTime.Now;
        }

        private void NumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int x;
            if (!int.TryParse(((TextBox)sender).Text, out x))
                ((TextBox)sender).Undo();
        }

        /// <summary>
        /// Запуск/остановка мероприятия
        /// </summary>
        private void PlayActionImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((outWindow.Action == null) || (outWindow.Action != (Action)ActionsListBox.SelectedItem) || (outWindow.Action.Started==false))
            {
                if (outWindow.Action != null) outWindow.Action.Started = false;
                ((Action)ActionsListBox.SelectedItem).Started = true;
                outWindow.Action = ((Action)ActionsListBox.SelectedItem);
                outWindow.Visibility = Visibility.Visible;
                outWindow.Activate();
            }
            else
            {
                outWindow.Action.Started = false;
                outWindow.Action = null;
                outWindow.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Закрытие окна программы
        /// </summary>
        private void PreferencesForm_Closing(object sender, CancelEventArgs e)
        {
            // закрытие окна отображения
            outWindow.AppClosing();
            // сохранение параметров окна
            WindowPreferences temp = new WindowPreferences();
            temp.Height = this.Height;
            temp.Width = this.Width;
            temp.Top = this.Top;
            temp.Left = this.Left;
            temp.WindowState = this.WindowState;
            FileXml<WindowPreferences> SaveWindowParams = new FileXml<WindowPreferences>();
            SaveWindowParams.path = propertiesPath;
            SaveWindowParams.obj = temp;
            SaveWindowParams.Write();
        }

        /// <summary>
        /// Сохранение мероприятий
        /// </summary>
        private void SaveAction_Click(object sender, RoutedEventArgs e)
        {

            editedAction.StartTime =
                new DateTime(
                    editedAction.StartTime.Year,
                    editedAction.StartTime.Month,
                    editedAction.StartTime.Day,
                    Convert.ToInt32(ActionHour.Text == "" ? "0" : ActionHour.Text),
                    Convert.ToInt32(ActionMinute.Text == "" ? "0" : ActionMinute.Text),
                    0);
            if (ActionsListBox.SelectedIndex == -1)
            {
                Action temp = new Action();
                temp.CopyFrom(editedAction);
                allActions.Add(temp);
                ActionsListBox.SelectedItem = temp;
            }
            else
                allActions[ActionsListBox.SelectedIndex].CopyFrom(editedAction);
            SaveActions();
        }

        /// <summary>
        /// Сохранение мероприятий
        /// </summary>
        private void SaveActions()
        {
            FileXml<Actions> SaveActions = new FileXml<Actions>();
            SaveActions.path = dataPath;
            SaveActions.obj = allActions;
            SaveActions.Write();
        }

        /// <summary>
        /// Сохранение события
        /// </summary>
        private void SaveEvent_Click(object sender, RoutedEventArgs e)
        {
            Event temp = new Event();
            temp.Name = EventName.Text;
            EventName.Text = "Новое событие";
            temp.StartTime = new TimeSpan(
                Convert.ToInt32(EventStartDay.Text != "" ? EventStartDay.Text : "0"),
                Convert.ToInt32(EventStartHour.Text != "" ? EventStartHour.Text : "0"),
                Convert.ToInt32(EventStartMins.Text != "" ? EventStartMins.Text : "0"),
                0);
            temp.Duration = new TimeSpan(
                Convert.ToInt32(EventDurationDay.Text != "" ? EventDurationDay.Text : "0"),
                Convert.ToInt32(EventDurationHour.Text != "" ? EventDurationHour.Text : "0"),
                Convert.ToInt32(EventDurationMins.Text != "" ? EventDurationMins.Text : "0"),
                0);
            ClearEvent();
            temp.ActionStartTime = editedAction.StartTime;
            if (EventsListBox.SelectedIndex != -1)
            {
                editedAction.Items.RemoveAt(EventsListBox.SelectedIndex);
            }
            editedAction.AddEvent(temp);
        }

        /// <summary>
        /// Выделение текста в TextBox'ах при получении фокуса
        /// </summary>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        /// <summary>
        /// Фильтр ввода чисел
        /// </summary>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //ss.SelectVoice(((InstalledVoice)testBox.SelectedItem == null ? (InstalledVoice)testBox.Items[0] : (InstalledVoice)testBox.SelectedItem).VoiceInfo.Name);
            //ss.Rate = 1;
            //ss.Volume = 100;
            //ss.SpeakAsync(testing.Content.ToString());
        }
    }
}