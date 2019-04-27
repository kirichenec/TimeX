using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversalKLibrary.Classic.Helpers;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.Model
{
    [Table("Checkpoints")]
    public class Checkpoint : SimplePropertyChanged
    {
        #region ctor
        public Checkpoint() { }

        public Checkpoint(Checkpoint value)
        {
            value.CopyPropertiesTo(this);
        }
        #endregion

        #region Properties

        #region Uid
        private string _uid;

        [Column("Uid")]
        [Key]
        public string Uid
        {
            get { return _uid; }
            set { _uid = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region CheckedDate
        private DateTime? _checkedDate;

        /// <summary>
        /// Дата отметки о выполнении
        /// </summary>
        [NotMapped]
        public DateTime? CheckedDate
        {
            get { return _checkedDate; }
            set { _checkedDate = value; NotifyPropertyChanged(); }
        }

        [Column("CheckedDate")]
        public Int64? CheckedDateTicks
        {
            get { return _checkedDate?.Ticks; }
            set { CheckedDate = value == null ? (DateTime?)null : new DateTime(value.Value); NotifyPropertyChanged(); }
        }
        #endregion

        #region Duration
        /// <summary>
        /// Продолжительность
        /// </summary>
        [NotMapped]
        public TimeSpan Duration
        {
            get { return TimeSpan.FromTicks(_durationTicks); }
            set { _durationTicks = value.Ticks; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DurationTicks)); NotifyPropertyChanged(nameof(EndTime)); }
        }

        private Int64 _durationTicks = 0;

        [Column("DurationTicks")]
        public Int64 DurationTicks
        {
            get { return _durationTicks; }
            set { _durationTicks = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(Duration)); NotifyPropertyChanged(nameof(EndTime)); }
        }
        #endregion

        #region EndTime
        /// <summary>
        /// Время окончания
        /// </summary>
        [NotMapped]
        public TimeSpan EndTime { get { return StartTime + Duration; } }
        #endregion

        #region IsOrderNeeded
        private bool _isOrderNeeded = true;

        /// <summary>
        /// Необходимость распоряжения
        /// </summary>
        [Column("IsOrderNeeded")]
        public bool IsOrderNeeded
        {
            get { return _isOrderNeeded; }
            set { _isOrderNeeded = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Name
        private string _name;

        /// <summary>
        /// Наименование
        /// </summary>
        [Column("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region ParentAction
        /// <summary>
        /// Мероприятие события
        /// </summary>
        public virtual Action ParentAction { get; set; }
        #endregion

        #region StartTime
        /// <summary>
        /// Время начала мероприятия
        /// </summary>
        [NotMapped]
        public TimeSpan StartTime
        {
            get { return TimeSpan.FromTicks(_startTimeTicks); }
            set { _startTimeTicks = value.Ticks; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(StartTimeTicks)); NotifyPropertyChanged(nameof(EndTime)); }
        }

        private Int64 _startTimeTicks = 0;

        [Column("StartTimeTicks")]
        public Int64 StartTimeTicks
        {
            get { return _startTimeTicks; }
            set { _startTimeTicks = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(StartTime)); NotifyPropertyChanged(nameof(EndTime)); }
        }
        #endregion

        #endregion
    }
}
