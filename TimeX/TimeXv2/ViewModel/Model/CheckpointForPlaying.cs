using System;
using System.ComponentModel;
using TimeXv2.Model;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.ViewModel.Model
{
    public class CheckpointForPlaying : SimplePropertyChanged
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
            this.StartTime = value.StartTime;

            this.ParentAction = parent;
            parent.CurrentTimeChanged += UpdateCheckpointProperties;
        }
        #endregion

        #region Properties

        #region Uid
        private string _uid;

        public string Uid
        {
            get { return _uid; }
            set { _uid = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region IsAlarmTime
        private bool _isAlarmTime;

        public bool IsAlarmTime
        {
            get { return _isAlarmTime; }
            set { _isAlarmTime = value; NotifyPropertyChanged(); }
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
            set { _checkedDate = value; NotifyPropertyChanged(); }
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
                _name = value;
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
                _parentAction = value;
                NotifyPropertyChanged();
            }
        }

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

        #region UpdateCheckpointProperties
        private void UpdateCheckpointProperties(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(CurrentPercent));
        }
        #endregion

        #endregion
    }
}
