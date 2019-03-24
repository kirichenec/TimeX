using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace TimeX
{
    /// <summary>
    /// Событие
    /// </summary>
    [Serializable()]
    public class Event : INotifyPropertyChanged
    {
        /// <summary>
        /// Время начала мероприятия (хозяина)
        /// </summary>
        private DateTime actionStartTime;
        /// <summary>
        /// Текущее время
        /// </summary>
        private DateTime currentTime;
        /// <summary>
        /// Длительность события
        /// </summary>
        private TimeSpan duration;
        /// <summary>
        /// Таймер для отслеживания текущего времени
        /// </summary>
        private DispatcherTimer eventTimer;
        /// <summary>
        /// Имя события
        /// </summary>
        private string name;
        /// <summary>
        /// Время начала события
        /// </summary>
        private TimeSpan startTime;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Астрономическое время начала мероприятия
        /// </summary>
        [XmlIgnore]
        public DateTime ActionStartTime
        {
            get
            {
                return actionStartTime;
            }
            set
            {
                actionStartTime = value;
                OnPropertyChanged("CompletePercent");
                OnPropertyChanged("ActionStartTime");
                OnPropertyChanged("FinishTime");
            }
        }

        /// <summary>
        /// Процент завершения события
        /// </summary>
        [XmlIgnore]
        public double CompletePercent
        {
            get
            {
                double percent =
                    ((DateTime.Now - this.ActionStartTime) - this.StartTime).TotalSeconds /
                    this.Duration.TotalSeconds * 100;
                // Ограничение в 100% и 0%
                percent = percent > 100 ?
                    100 : (percent < 0 ?
                        0 : percent);
                return percent;
            }
        }

        /// <summary>
        /// Часы
        /// </summary>
        [XmlIgnore]
        public DateTime CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                currentTime = value;
                OnPropertyChanged("CompletePercent");
                OnPropertyChanged("CurrentTime");
            }
        }

        /// <summary>
        /// Продолжительность события
        /// </summary>
        [XmlIgnore]
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }

        }

        /// <summary>
        /// Свойство "длительность" для сохранения в файл
        /// </summary>
        [XmlElement("Duration")]
        public double DurationTicks
        {
            get
            {
                return duration.TotalSeconds;
            }
            set
            {
                duration = TimeSpan.FromSeconds(value);
            }
        }

        /// <summary>
        /// Время завершения события
        /// </summary>
        [XmlIgnore]
        public TimeSpan FinishTime
        {
            get
            {
                return this.startTime.Add(this.duration);
            }
        }

        /// <summary>
        /// Имя события
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Время Ч события
        /// </summary>
        [XmlIgnore]
        public TimeSpan StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        /// <summary>
        /// Свойство "время начала" для сохранения в файл
        /// </summary>
        [XmlElement("StartTime")]
        public double StartTimeTicks
        {
            get
            {
                return startTime.TotalSeconds;
            }
            set
            {
                startTime = TimeSpan.FromSeconds(value);
            }
        }

        /// <summary> 
        /// Конструктор события
        /// </summary>
        public Event()
        {
            InitializeClock();
        }

        /// <summary>
        /// Конструктор события
        /// </summary>
        /// <param name="value">Событие, из которого копируется описание</param>
        public Event(Event value)
        {
            this.Duration = value.Duration;
            this.Name = value.Name;
            this.StartTime = value.StartTime;
            this.ActionStartTime = value.ActionStartTime;
            InitializeClock();
        }

        /// <summary>
        /// Конструктор события
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="startTime">Время начала</param>
        /// <param name="duration">Продолжительность</param>
        public Event(string name, TimeSpan startTime, TimeSpan duration, DateTime actionStartTime)
        {
            this.Duration = duration;
            this.Name = name;
            this.StartTime = startTime;
            this.ActionStartTime = actionStartTime;
            InitializeClock();
        }

        /// <summary>
        /// Метод, изменяющий переменную текущего времени
        /// </summary>
        private void EventTimer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        /// <summary>
        /// Инициализация часов для события
        /// </summary>
        private void InitializeClock()
        {
            eventTimer = new DispatcherTimer();                     // Таймер для обновления инфы о мероприятии
            eventTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);    // Задание отрезка времени (1 мс)
            eventTimer.Tick += new EventHandler(EventTimer_Tick);   // Событие прошествия очередного отрезка времени
            eventTimer.Start();
        }

        /// <summary>
        /// Копирование события
        /// </summary>
        /// <param name="value">Копируемое событие</param>
        public void CopyFrom(Event value)
        {
            this.Name = value.Name;
            this.Duration = value.Duration;
            this.StartTime = value.StartTime;
            this.ActionStartTime = value.ActionStartTime;
        }

        /// <summary>
        /// Метод, передающий имя изменённого свойства
        /// </summary>
        /// <param name="property">Имя изменившегося свойства</param>
        private void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    /// <summary>
    /// Левая/правая ширина чекпоинта
    /// </summary>
    public class Checkpoint
    {
        /// <summary>
        /// Ширина левой части чекпоинта
        /// </summary>
        public GridLength Left { get; set; }

        /// <summary>
        /// Ширина правой части чекпоинта
        /// </summary>
        public GridLength Right { get; set; }

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public Checkpoint() { }

        /// <summary>
        /// Конструктор для создания чекпоинта
        /// </summary>
        /// <param name="value">Значение чекпоинта</param>
        /// <param name="full">Длина мероприятия</param>
        public Checkpoint(double value, double full)
        {
            this.Left = new GridLength(value, GridUnitType.Star);
            this.Right = new GridLength(full - value, GridUnitType.Star);
        }
    }

    /// <summary>
    /// Мероприятие
    /// </summary>
    [Serializable()]
    public class Action : INotifyPropertyChanged       
    {
        /// <summary>
        /// Таймер для отслеживания времени
        /// </summary>
        private DispatcherTimer actionTimer;
        /// <summary>
        /// Текущее время
        /// </summary>
        private DateTime currentTime;
        /// <summary>
        /// Список событий мероприятия
        /// </summary>
        private ObservableCollection<Event> items = new ObservableCollection<Event>();
        /// <summary>
        /// Название мероприятия
        /// </summary>
        private string name;
        /// <summary>
        /// Состояние мероприятия: запущено/остановлено
        /// </summary>
        private bool started;
        /// <summary>
        /// Время запуска мероприятия
        /// </summary>
        private DateTime startTime;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Время начала/окончания всех событий мероприятия
        /// </summary>
        [XmlIgnore]
        public List<Checkpoint> Checkpoints
        {
            get
            {
                List<double> temp = new List<double>();
                foreach (var ev in this.Items)
                {
                    // Отсечение нулевой метки и метки окончания мероприятия
                    if (ev.StartTime.TotalSeconds != 0 && ev.StartTime.TotalSeconds != this.Duration.TotalSeconds)
                    {
                        temp.Add(ev.StartTime.TotalSeconds);
                    }
                    if (ev.FinishTime.TotalSeconds != 0 && ev.FinishTime.TotalSeconds != this.Duration.TotalSeconds)
                    {
                        temp.Add(ev.FinishTime.TotalSeconds);
                    }
                }
                temp.Sort();                        // Сортировка чекпоинтов
                temp = temp.Distinct().ToList();    // Отсечение повторений
                List<Checkpoint> outList = new List<Checkpoint>();
                foreach (double value in temp)
                    outList.Add(new Checkpoint(value, this.Duration.TotalSeconds));
                return outList;
            }
        }

        /// <summary>
        /// Процент завершения мероприятия
        /// </summary> 
        [XmlIgnore]
        public double CompletePercent
        {
            get
            {    
                double percent =
                    (CurrentTime - StartTime).TotalSeconds /
                    Duration.TotalSeconds * 100;
                // Ограничение в 100% и 0%
                percent = percent > 100 ?
                    100 :
                    (percent < 0 ? 0 : percent);
                return percent;
            }
        }

        /// <summary>
        /// Возвращает выполняемые события и по одному прошедшее и следующее
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<Event> CurrentAndLastNext
        {
            get
            {
                ObservableCollection<Event> temp = new ObservableCollection<Event>();
                if(this.LastEvent!=null)
                    temp.Add(this.LastEvent);
                foreach (Event tempEvent in this.CurrentEvents)
                    temp.Add(tempEvent);
                if(this.NextEvent!=null)
                    temp.Add(this.NextEvent);
                return temp;
            }
        }

        /// <summary>
        /// Возвращает выполняемые события
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<Event> CurrentEvents
        {
            get
            {
                ObservableCollection<Event> temp = new ObservableCollection<Event>();
                for (int i = 0; i < this.Items.Count; i++)
                    if (DateTime.Now.Subtract(this.StartTime) >= Items[i].StartTime &&
                        DateTime.Now.Subtract(this.StartTime) < Items[i].StartTime + Items[i].Duration)
                        temp.Add(Items[i]);
                return temp;
            }
        }

        /// <summary>
        /// Часы
        /// </summary>
        [XmlIgnore]
        public DateTime CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                currentTime = value;
                OnPropertyChanged("CompletePercent");
                OnPropertyChanged("CurrentEvents");
                OnPropertyChanged("CurrentTime");
                OnPropertyChanged("ElapsedTime");
                OnPropertyChanged("Items");
                OnPropertyChanged("LastEvents");
                OnPropertyChanged("NextEvents");
                OnPropertyChanged("RemainingTime");
                OnPropertyChanged("CurrentAndLastNext");
            }
        }

        /// <summary>
        /// Длительность мероприятия
        /// </summary>
        [XmlIgnore]
        public TimeSpan Duration
        {
            get
            {
                TimeSpan duration = TimeSpan.Zero;
                foreach (Event value in this.Items)
                    if (duration < value.FinishTime)
                        duration = value.FinishTime;
                return duration;
            }
        }

        /// <summary>
        /// Прошло времени
        /// </summary>
        [XmlIgnore]
        public TimeSpan ElapsedTime
        {
            get
            {
                TimeSpan temp = TimeSpan.FromSeconds((int)((CurrentTime.Subtract(this.StartTime)).TotalSeconds));
                // Время не уходит в минус и не больше длительности
                temp = temp < TimeSpan.Zero ?
                    TimeSpan.Zero :
                    (temp > this.Duration ? this.Duration : temp);
                return temp;
            }
        }

        /// <summary>
        /// Астрономическое время завершения мероприятия
        /// </summary>
        [XmlIgnore]
        public DateTime FinishTime
        {
            get
            {
                return this.StartTime.Add(this.Duration);
            }
        }

        /// <summary>
        /// События мероприятия
        /// </summary>
        public ObservableCollection<Event> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        /// <summary>
        /// Возвращает прошедшие события мероприятия
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<Event> LastEvents
        {
            get
            {
                ObservableCollection<Event> temp = new ObservableCollection<Event>();
                for (int i = 0; i < this.Items.Count; i++)
                    if (DateTime.Now.Subtract(this.StartTime) > this.Items[i].StartTime + this.Items[i].Duration)
                        temp.Add(this.Items[i]);
                return temp;
            }
        }

        /// <summary>
        /// Последнее выполненное событие
        /// </summary>
        [XmlIgnore]
        public Event LastEvent
        {
            get
            {
                if (LastEvents.Count > 0)
                    return LastEvents[LastEvents.Count - 1];
                else
                    return null;
            }
        }

        /// <summary>
        /// Имя мероприятия
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Возвращает следующие события мероприятия
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<Event> NextEvents
        {
            get
            {
                ObservableCollection<Event> temp = new ObservableCollection<Event>();
                for (int i = 0; i < this.Items.Count; i++)
                    if (DateTime.Now.Subtract(this.StartTime) < this.Items[i].StartTime)
                        temp.Add(this.Items[i]);
                return temp;
            }
        }

        /// <summary>
        /// Следующее для выполнения событие
        /// </summary>
        [XmlIgnore]
        public Event NextEvent
        {
            get
            {
                if (NextEvents.Count > 0)
                    return NextEvents[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Остаток времени
        /// </summary>
        [XmlIgnore]
        public TimeSpan RemainingTime
        {
            get
            {
                TimeSpan temp = TimeSpan.FromSeconds((int)((this.FinishTime.Subtract(CurrentTime)).TotalSeconds));
                // Время не уходит в минус и не больше длительности
                temp = temp < TimeSpan.Zero ?
                    TimeSpan.Zero :
                    (temp > this.Duration ? this.Duration : temp);
                return temp;
            }
        }

        /// <summary>
        /// Индикатор запуска приложения
        /// </summary>
        [XmlIgnore]
        public bool Started
        {
            get
            {
                return started;
            }
            set
            {
                started = value;
                OnPropertyChanged("Started");
                if (value)
                    actionTimer.Start();
                else
                    actionTimer.Stop();
            }
        }

        /// <summary>
        /// Дата и время начала мероприятия
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                foreach (Event tempEvent in items)
                {
                    tempEvent.ActionStartTime = value;
                }
                OnPropertyChanged("FinishTime");
                OnPropertyChanged("StartTime");
            }
        }

        /// <summary>
        /// Индексатор для доступа к событиям без использования Items
        /// </summary>
        /// <param name="index">Индекс события</param>
        /// <returns>Событие</returns>
        [XmlIgnore]
        public Event this[int index]
        {
            get
            {
                return this.Items[index];
            }
            set
            {
                this.Items[index] = value;
            }
        }

        /// <summary>
        /// Конструктор мероприятия
        /// </summary>
        public Action()
        {
            InitializeClock();
        }

        /// <summary>
        /// Конструктор мероприятия
        /// </summary>
        public Action(string name)
        {
            InitializeClock();
            this.Name = name;
        }

        /// <summary>
        /// Метод, изменяющий переменную текущего времени
        /// </summary>
        private void ActionTimer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        /// <summary>
        /// Добавление события в список событий мероприятия по порядку
        /// </summary>
        /// <param name="value">Добавляемое событие</param>
        public void AddEvent(Event value)
        {
            int i = 0;
            while (i < this.Items.Count && value.StartTime >= this.Items[i].StartTime)
            {
                i++;
            }
            this.Items.Add(new Event());
            for (int j = this.Items.Count - 1; j > i; j--)
                this.Items[j] = this.Items[j - 1];
            this.Items[i] = value;
        }

        /// <summary>
        /// Добавление события в список событий мероприятия по порядку
        /// </summary>
        /// <param name="name">Имя события</param>
        /// <param name="startTime">Время начала события</param>
        /// <param name="duration">Продолжительность события</param>
        public void AddEvent(string name, TimeSpan startTime, TimeSpan duration)
        {
            this.AddEvent(new Event(name, startTime, duration, this.StartTime));
        }

        /// <summary>
        /// События мероприятия в задонное время
        /// </summary>
        /// <param name="selectedTime">Время для определения событий</param>
        /// <returns>Список событий в заданное время</returns>
        public List<Event> AtTime(TimeSpan selectedTime)
        {
            List<Event> temp = new List<Event>();
            foreach (Event ev in this.Items)
            {
                if (ev.StartTime <= selectedTime && ev.FinishTime >= selectedTime)
                    temp.Add(ev);
            }
            return temp;
        }

        /// <summary>
        /// События мероприятия в задонное время
        /// </summary>
        /// <param name="selectedTime">Время для определения событий</param>
        /// <returns>Список событий в заданное время</returns>
        public List<Event> AtTime(DateTime selectedTime)
        {
            return this.AtTime(selectedTime.Subtract(this.StartTime));
        }

        /// <summary>
        /// Копирование мероприятия по значению
        /// </summary>
        /// <param name="value">Мероприятие, из которого копируются объекты</param>
        public void CopyFrom(Action value)
        {
            this.Name = value.Name;
            this.StartTime = value.StartTime;
            this.Started = value.Started;
            for (int i = this.Items.Count - 1; i >= 0; i--)
                this.Items.RemoveAt(i);
            for (int i = 0; i < value.Items.Count; i++)
                this.AddEvent(value.Items[i].Name, value.Items[i].StartTime, value.Items[i].Duration);
        }

        /// <summary>
        /// Инициализация часов для мероприятия
        /// </summary>
        private void InitializeClock()
        {
            actionTimer = new DispatcherTimer();                     // Таймер для обновления инфы о мероприятии
            actionTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);      // Задание отрезка времени (1 мс)
            actionTimer.Tick += new EventHandler(ActionTimer_Tick);  // Событие прошествия очередного отрезка времени
            actionTimer.Start();
        }

        /// <summary>
        /// Метод, вызываемый при изменении объекта
        /// </summary>
        /// <param name="property">Имя изменившегося объекта</param>
        private void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    /// <summary>
    /// Список мероприятий
    /// </summary>
    [Serializable()]
    public class Actions : ObservableCollection<Action>
    {
        /// <summary>
        /// Все запущенные мероприятия
        /// </summary>
        [XmlIgnore]
        public Actions StartedActions
        {
            get
            {
                Actions value = new Actions();
                for (int i = 0; i < this.Count; i++)
                    if (this[i].Started)
                        value.Add(this[i]);
                return value;
            }
        }
    }
}