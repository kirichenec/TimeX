using System;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.Extensions
{
    public class SpecialTimeSpan : SimplePropertyChanged
    {
        #region ctor
        public SpecialTimeSpan(TimeSpan value)
        {
            this.Days = value.Days;
            this.Minutes = value.Minutes;
            this.Hours = value.Hours;
        }

        public SpecialTimeSpan(int ticks) : this(new TimeSpan(ticks))
        {
        }
        #endregion

        #region Properties

        #region Days
        private int _days;

        public int Days
        {
            get { return _days; }
            set
            {
                if (value < 0)
                {
                    _days = 0;
                }
                else
                {
                    _days = value;
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AsTimeSpan));
            }
        }
        #endregion

        #region Hours
        private int _hours;

        public int Hours
        {
            get { return _hours; }
            set
            {
                if (value < 0)
                {
                    if (this.Days > 0)
                    {
                        _hours = 24 + value;
                        this.Days -= 1;
                    }
                    else
                    {
                        _hours = 0;
                    }
                }
                else if (value > 23)
                {
                    _hours = 0;
                    this.Days += 1;
                }
                else
                {
                    _hours = value;
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AsTimeSpan));
            }
        }
        #endregion

        #region Minutes
        private int _minutes;

        public int Minutes
        {
            get { return _minutes; }
            set
            {
                if (value < 0)
                {
                    if (this.Hours > 0)
                    {
                        _minutes = 60 + value;
                        this.Hours -= 1;
                    }
                    else
                    {
                        _minutes = 0;
                    }
                }
                else if (value > 59)
                {
                    _minutes = 0;
                    this.Hours += 1;
                }
                else
                {
                    _minutes = value;
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AsTimeSpan));
            }
        }
        #endregion

        #region AsTimeSpan
        public TimeSpan AsTimeSpan
        {
            get { return new TimeSpan(this.Days, this.Hours, this.Minutes, 0); }
        }
        #endregion

        #endregion

        #region Methods

        #region ToString
        public override string ToString()
        {
            return this.AsTimeSpan.ToString();
        }
        #endregion

        #endregion
    }
}
