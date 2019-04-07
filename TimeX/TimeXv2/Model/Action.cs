using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using UniversalKLibrary.Classic.Helpers;
using UniversalKLibrary.Classic.Simplificators;

namespace TimeXv2.Model
{
    public class Action : SimplePropertyChanged
    {
        #region ctor
        public Action() { Checkpoints = new ObservableCollection<Checkpoint>(); }

        public Action(Action value)
        {
            value.CopyPropertiesTo(this);
            this.Checkpoints = new ObservableCollection<Checkpoint>();
            foreach (var chk in value.Checkpoints)
            {
                this.Checkpoints.Add(new Checkpoint(chk));
            }
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

        #region Checkpoints
        private ObservableCollection<Checkpoint> _checkpoints;

        public ObservableCollection<Checkpoint> Checkpoints
        {
            get { return _checkpoints; }
            set { _checkpoints = value; NotifyPropertyChanged(); }
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
        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; NotifyPropertyChanged(); }
        }
        #endregion
    }
}