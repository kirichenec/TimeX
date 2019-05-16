using System;
using System.ComponentModel;
using TimeXv2.Model;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.ViewModel.Model
{
    public class CheckpointForPlaying : SimplePropertyChanged, IDisposable
    {
        #region ctor
        public CheckpointForPlaying() { }

        public CheckpointForPlaying(Checkpoint value, ActionForPlaying parent)
        {
            this.Uid = value.Uid;
            this.CheckedDate = value.CheckedDate;
            this.Duration = value.Duration;
            this.IsOrderNeeded = value.IsOrderNeeded;
            this.Name = value.Name;
            this.Order = value.Order;
            this.StartTime = value.StartTime;

            this.ParentAction = parent;
            this.ParentActionUID = parent.Uid;

            this.ParentAction.CurrentTimeChanged += UpdateCheckpointProperties;
        }
        #endregion

        #region Properties

        #region Uid
        private string _uid;

        public string Uid
        {
            get { return _uid; }
            set
            {
                if (_uid == value)
                {
                    return;
                }
                _uid = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region IsAlarmTime
        private bool _isAlarmTime;

        public bool IsAlarmTime
        {
            get { return _isAlarmTime; }
            set
            {
                if (_isAlarmTime == value)
                {
                    return;
                }
                _isAlarmTime = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region CheckedDate
        private DateTime? _checkedDate;

        /// <summary>
        /// Дата отметки о выполнении
        /// </summary>
        public DateTime? CheckedDate
        {
            get { return _checkedDate; }
            set
            {
                if (_checkedDate == value)
                {
                    return;
                }
                _checkedDate = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Duration
        private TimeSpan _duration;

        /// <summary>
        /// Продолжительность
        /// </summary>
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value)
                {
                    return;
                }
                _duration = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(EndTime));
            }
        }

        #endregion

        #region EndTime
        /// <summary>
        /// Время окончания
        /// </summary>
        public TimeSpan EndTime { get { return StartTime + Duration; } }
        #endregion

        #region IsOrderNeeded
        private bool _isOrderNeeded = true;

        /// <summary>
        /// Необходимость распоряжения
        /// </summary>
        public bool IsOrderNeeded
        {
            get { return _isOrderNeeded; }
            set
            {
                if (_isOrderNeeded == value)
                {
                    return;
                }
                _isOrderNeeded = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Name
        private string _name;

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }
                _name = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Order
        private string _order;

        public string Order
        {
            get { return _order; }
            set
            {
                if (_order == value)
                {
                    return;
                }
                _order = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region StartTime
        private TimeSpan _startTime;

        /// <summary>
        /// Время начала мероприятия
        /// </summary>
        public TimeSpan StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime == value)
                {
                    return;
                }
                _startTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(EndTime));
            }
        }
        #endregion

        #region ParentAction
        private ActionForPlaying _parentAction;

        public ActionForPlaying ParentAction
        {
            get { return _parentAction; }
            set
            {
                if (_parentAction == value)
                {
                    return;
                }
                _parentAction = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region ParentActionUID
        public string ParentActionUID { get; set; }
        #endregion

        #region CurrentPercent
        public double CurrentPercent
        {
            get
            {
                double _currentPercent = (double)(ParentAction.CurrentTime - ParentAction.StartTime - StartTime).Ticks * 100 / Duration.Ticks;
                _currentPercent =
                    _currentPercent < 0 ?
                    0 :
                    _currentPercent > 100 ?
                    100 : _currentPercent;

                IsAlarmTime = _currentPercent > 90;
                if (_currentPercent == 100)
                {
                    ParentAction.CurrentTimeChanged -= UpdateCheckpointProperties;
                }
                return _currentPercent;
            }
        }
        #endregion

        #endregion

        #region Methods

        public void Dispose()
        {
            this.ParentAction.CurrentTimeChanged -= UpdateCheckpointProperties;
        }

        #region ToCheckpoint
        public Checkpoint ToCheckpoint()
        {
            var value = new Checkpoint
            {
                Uid = this.Uid,
                CheckedDate = this.CheckedDate,
                Duration = this.Duration,
                IsOrderNeeded = this.IsOrderNeeded,
                Name = this.Name,
                Order = this.Order,
                StartTime = this.StartTime,
            };
            return value;
        }
        #endregion

        #region UpdateCheckpointProperties
        private void UpdateCheckpointProperties(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(CurrentPercent));
        }
        #endregion

        #endregion
    }
}
