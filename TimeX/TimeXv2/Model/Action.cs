using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.Model
{
    [Table("Actions")]
    public class Action : SimplePropertyChanged
    {
        #region ctor
        public Action() { Checkpoints = new ObservableCollection<Checkpoint>(); StartTime = DateTime.Now; }

        public Action(Action value)
        {
            if (value == null) return;

            this.Uid = value.Uid;
            this.Name = value.Name;
            this.StartTime = value.StartTime;
            this.Checkpoints = new ObservableCollection<Checkpoint>();
            foreach (var chk in value.Checkpoints)
            {
                this.Checkpoints.Add(new Checkpoint(chk));
            }
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

        #region Checkpoints
        private ObservableCollection<Checkpoint> _checkpoints;

        public virtual ObservableCollection<Checkpoint> Checkpoints
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

        #region Name
        private string _name;

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

        #region StartTime
        [NotMapped]
        public DateTime StartTime
        {
            get { return new DateTime(_startTimeTicks); }
            set
            {
                if (_startTimeTicks == value.Ticks)
                {
                    return;
                }
                _startTimeTicks = value.Ticks;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(StartTimeTicks));
            }
        }

        private Int64 _startTimeTicks;

        [Column("StartTimeTicks")]
        public Int64 StartTimeTicks
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
                NotifyPropertyChanged(nameof(StartTime));
            }
        }
        #endregion

        #endregion
    }
}