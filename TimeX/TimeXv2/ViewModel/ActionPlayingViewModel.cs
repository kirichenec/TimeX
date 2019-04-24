using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TimeXv2.Extensions;
using TimeXv2.Model;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Navigation;
using ModelAction = TimeXv2.Model.Action;

namespace TimeXv2.ViewModel
{
    public class ActionPlayingViewModel : ViewModelBase
    {
        #region ctor
        public ActionPlayingViewModel(INavigationService navigationService, IDataService dataService)
        {
            _dataService = dataService;

            MessengerInstance
                .Register<ActionPlayingMessage>(this, apm =>
                {
                    _actionUidForLoad = apm.Uid;
                });

            if (IsInDesignMode)
            {
                var chk = new ObservableCollection<Checkpoint>
                {
                    new Checkpoint()
                    {
                        StartTime = TimeSpan.FromMinutes(0),
                        Duration = TimeSpan.FromMinutes(5),
                        IsOrderNeeded = true,
                        ParentAction = this.PlayedAction,
                        Name = "new"
                    }
                };
                this.PlayedAction =
                    new ActionForPlaying(new ModelAction()
                    {
                        Name = "Name",
                        StartTime = DateTime.Now,
                        StartTimeTicks = 0,
                        Checkpoints = chk
                    });
            }
        }
        #endregion

        #region Services
        private readonly IDataService _dataService;
        #endregion

        #region Fields

        #region _actionUidForLoad
        private string _actionUidForLoad;
        #endregion

        #endregion

        #region Properties

        #region PlayedAction
        private ActionForPlaying _playedAction = null;

        /// <summary>
        /// Sets and gets the <see cref="PlayedAction"/> property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ActionForPlaying PlayedAction
        {
            get
            {
                return _playedAction;
            }

            set
            {
                if (_playedAction == value)
                {
                    return;
                }

                _playedAction = value;
                RaisePropertyChanged(nameof(PlayedAction));
            }
        }
        #endregion

        #region IsExpanded
        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                {
                    return;
                }
                _isExpanded = value;
                RaisePropertyChanged(nameof(IsExpanded));
            }
        }
        #endregion

        #endregion

        #region Commands

        #region ChangeIsExpandedCommand
        private RelayCommand _changeIsExpandedCommand;

        public RelayCommand ChangeIsExpandedCommand
        {
            get
            {
                return _changeIsExpandedCommand
                    ?? (_changeIsExpandedCommand = new RelayCommand(
                        () =>
                        {
                            this.IsExpanded = !this.IsExpanded;
                        }));
            }
        }

        #endregion

        #region LoadCommand
        private RelayCommand _loadCommand;

        /// <summary>
        /// Gets the LoadCommand.
        /// </summary>
        public RelayCommand LoadCommand
        {
            get
            {
                return _loadCommand
                    ?? (_loadCommand = new RelayCommand(
                    async () =>
                    {
                        this.PlayedAction =
                            _actionUidForLoad == null ?
                            new ActionForPlaying() :
                            new ActionForPlaying(await _dataService.GetActionByUidAsync(_actionUidForLoad));
                    }));
            }
        }
        #endregion

        #region UnloadCommand
        private RelayCommand _unloadCommand;

        /// <summary>
        /// Gets the UnloadCommand.
        /// </summary>
        public RelayCommand UnloadCommand
        {
            get
            {
                return _unloadCommand
                    ?? (_unloadCommand = new RelayCommand(
                    () =>
                    {

                    }));
            }
        }
        #endregion

        #endregion
    }

    [NotMapped]
    public class ActionForPlaying : ModelAction
    {
        #region ctor
        public ActionForPlaying() { }

        public ActionForPlaying(ModelAction value) : base(value)
        {
            this.Checkpoints = new ObservableCollection<CheckpointForPlaying>();
            value?.Checkpoints?.ForEach(chk => this.Checkpoints.Add(new CheckpointForPlaying(chk, this)));

            this.EndTime = this.StartTime.Add(value.Checkpoints.GetDuration());
            this.CurrentTime = DateTime.Now;
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler CurrentTimeChanged;
        #endregion

        #region Properties

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
                _endTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(this.LeftTime));
                NotifyPropertyChanged(nameof(this.RemainingTime));
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
                _currentTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(this.LeftTime));
                NotifyPropertyChanged(nameof(this.RemainingTime));
                CurrentTimeChanged(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
            }
        }
        #endregion

        #region Checkpoints
        private ObservableCollection<CheckpointForPlaying> _checkpoints;

        /// <summary>
        /// События
        /// </summary>
        public new ObservableCollection<CheckpointForPlaying> Checkpoints
        {
            get { return _checkpoints; }
            set
            {
                _checkpoints = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #endregion
    }

    [NotMapped]
    public class CheckpointForPlaying : Checkpoint
    {
        #region ctor
        public CheckpointForPlaying() { }

        public CheckpointForPlaying(Checkpoint baseValue, ActionForPlaying parent) : base(baseValue)
        {
            this.ParentAction = parent;
            parent.CurrentTimeChanged += UpdateCheckpointProperties;
        }

        private void UpdateCheckpointProperties(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(CurrentPercent));
        }
        #endregion

        #region ParentAction
        private ActionForPlaying _parentAction;

        public new ActionForPlaying ParentAction
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
        public long CurrentPercent
        {
            get
            {
                long _currentPercent =
                    ((this.ParentAction.CurrentTime - this.ParentAction.StartTime) + this.StartTime).Ticks /
                    this.Duration.Ticks;
                _currentPercent =
                    _currentPercent < 0 ?
                    0 :
                    _currentPercent > 100 ?
                    100 : _currentPercent;
                return _currentPercent;
            }
        }
        #endregion
    }
}
