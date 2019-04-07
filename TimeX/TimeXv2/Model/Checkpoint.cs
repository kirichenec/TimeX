using System;
using System.ComponentModel.DataAnnotations;
using UniversalKLibrary.Classic.Helpers;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.Model
{
    public class Checkpoint : SimplePropertyChanged
    {
        #region ctor
        public Checkpoint() { }

        public Checkpoint(Checkpoint value)
        {
            value.CopyPropertiesTo(this);
        }
        #endregion

        #region Uid
        private Guid? _uid;

        [Key]
        public Guid? Uid
        {
            get { return _uid; }
            set { _uid = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region CheckedDate
        private DateTime? _checkedDate;

        public DateTime? CheckedDate
        {
            get { return _checkedDate; }
            set { _checkedDate = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Duration
        private TimeSpan _duration;

        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(EndTime)); }
        }
        #endregion

        #region EndTime
        public TimeSpan EndTime { get { return StartTime + Duration; } }
        #endregion

        #region IsOrderNeeded
        private bool _isOrderNeeded;

        public bool IsOrderNeeded
        {
            get { return _isOrderNeeded; }
            set { _isOrderNeeded = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Name
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region StartTime
        private TimeSpan _startTime;

        public TimeSpan StartTime
        {
            get { return _startTime; }
            set { _startTime = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(EndTime)); }
        }
        #endregion
    }
}
