﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TimeXv2.Extensions;
using UniversalKLibrary.Classic.Simplificators;
using ModelAction = TimeXv2.Model.Action;

namespace TimeXv2.ViewModel.Model
{
    public class ActionForPlaying : SimplePropertyChanged
    {
        #region ctor
        public ActionForPlaying() { }

        public ActionForPlaying(ModelAction value)
        {
            if (value == null) return;

            this.Uid = value.Uid;
            this.Name = value.Name;
            this.StartTime = value.StartTime;
            this.CurrentTime = DateTime.Now;
            this.EndTime = this.StartTime.Add(value.Checkpoints.GetDuration());

            this.Checkpoints.Clear();
            value?.Checkpoints?.ForEach(chk => this.Checkpoints.Add(new CheckpointForPlaying(chk, this, value)));
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler CurrentTimeChanged;
        #endregion

        #region Properties

        #region Checkpoints
        private ObservableCollection<CheckpointForPlaying> _checkpoints = new ObservableCollection<CheckpointForPlaying>();

        /// <summary>
        /// События
        /// </summary>
        public ObservableCollection<CheckpointForPlaying> Checkpoints
        {
            get { return _checkpoints; }
            set
            {
                if (_checkpoints == value)
                {
                    return;
                }
                _checkpoints = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region CurrentTime
        private DateTime _currentTime;

        /// <summary>
        /// Текущее время
        /// </summary>
        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (_currentTime == value)
                {
                    return;
                }
                _currentTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(LeftTime));
                NotifyPropertyChanged(nameof(RemainingTime));
                NotifyPropertyChanged(nameof(CurrentPercent));
                CurrentTimeChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
                IsAlarmNeeded = (Checkpoints?.Where(chk => chk.IsOrderNeeded && chk.IsAlarmTime && chk.CheckedDate == null)?.Count() ?? -1) > 0;
            }
        }
        #endregion

        #region EndTime
        private DateTime _endTime;

        /// <summary>
        /// Время окончания
        /// </summary>
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime == value)
                {
                    return;
                }
                _endTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(this.LeftTime));
                NotifyPropertyChanged(nameof(this.RemainingTime));
            }
        }
        #endregion

        #region IsAlarmNeeded
        private bool _isAlarmNeeded;

        public bool IsAlarmNeeded
        {
            get { return _isAlarmNeeded; }
            set
            {
                if (_isAlarmNeeded == value)
                {
                    return;
                }
                _isAlarmNeeded = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region LeftTime
        /// <summary>
        /// Прошло времени
        /// </summary>
        public TimeSpan LeftTime
        {
            get
            {
                var value =
                    this.CurrentTime <= this.StartTime ?
                    new TimeSpan() :
                    this.CurrentTime <= this.EndTime ?
                    this.CurrentTime - this.StartTime :
                    this.EndTime - this.StartTime;
                return value;
            }
        }
        #endregion

        #region Name
        private string _name;

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

        #region CurrentPercent
        public double CurrentPercent
        {
            get
            {
                var duration = (this.EndTime - this.StartTime).Ticks;
                var _currentPercent =
                    duration > 0 ?
                    (double)(this.LeftTime.Ticks * 100 / duration) : 100;
                _currentPercent =
                    _currentPercent < 0 ?
                    0 :
                    _currentPercent > 100 ?
                    100 : _currentPercent;
                return _currentPercent;
            }
        }
        #endregion

        #region RemainingTime
        /// <summary>
        /// Осталось времени
        /// </summary>
        public TimeSpan RemainingTime
        {
            get
            {
                var value =
                    this.EndTime < this.CurrentTime ?
                    new TimeSpan() :
                    this.CurrentTime >= this.StartTime ?
                    this.EndTime - this.CurrentTime :
                    this.EndTime - this.StartTime;
                return value;
            }
        }
        #endregion

        #region StartTime
        private DateTime _startTimeTicks;

        public DateTime StartTime
        {
            get { return _startTimeTicks; }
            set
            {
                if (_startTimeTicks == value)
                {
                    return;
                }
                _startTimeTicks = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Uid
        private int _uid;

        public int Uid
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

        #endregion
    }
}
