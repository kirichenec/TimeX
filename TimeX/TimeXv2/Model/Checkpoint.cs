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

        public Checkpoint(Checkpoint value, bool copyParent = false, Action parent = null)
        {
            value.CopyPropertiesTo(this, copyParent, parent);
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

        #region CheckedDate
        private DateTime? _checkedDate;

        /// <summary>
        /// Дата отметки о выполнении
        /// </summary>
        [NotMapped]
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
            set
            {
                if (_durationTicks == value.Ticks)
                {
                    return;
                }
                _durationTicks = value.Ticks;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DurationTicks));
                NotifyPropertyChanged(nameof(EndTime));
            }
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
        [Column("Name")]
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

        #region ParentAction
        /// <summary>
        /// Мероприятие события
        /// </summary>
        public virtual Action ParentAction { get; set; }
        #endregion

        #region ParentActionUID
        public string ParentActionUID { get; set; }
        #endregion

        #region StartTime
        /// <summary>
        /// Время начала мероприятия
        /// </summary>
        [NotMapped]
        public TimeSpan StartTime
        {
            get { return TimeSpan.FromTicks(_startTimeTicks); }
            set
            {
                if (_startTimeTicks == value.Ticks)
                {
                    return;
                }
                _startTimeTicks = value.Ticks;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(StartTimeTicks));
                NotifyPropertyChanged(nameof(EndTime));
            }
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

        #region Methods

        #region CopyPropertiesTo
        public void CopyPropertiesTo(Checkpoint target, bool copyParent = false, Action parent = null)
        {
            target.CheckedDate = this.CheckedDate;
            target.Duration = this.Duration;
            target.IsOrderNeeded = this.IsOrderNeeded;
            target.Name = this.Name;
            target.Order = this.Order;
            target.StartTime = this.StartTime;
            target.Uid = this.Uid;
            if (copyParent)
            {
                target.ParentActionUID = this.ParentActionUID;
                target.ParentAction = this.ParentAction;
            }
            else
            {
                target.ParentActionUID = parent.Uid;
                target.ParentAction = parent;
            }
        }
        #endregion

        #endregion
    }
}
