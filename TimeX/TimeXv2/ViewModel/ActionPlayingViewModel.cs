using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
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
            _navigationService = navigationService;

            MessengerInstance
                .Register<ActionPlayingMessage>(this, apm =>
                {
                    this.PlayedAction =
                        apm.Uid == null ?
                        new ActionForPlaying() :
                        new ActionForPlaying(_dataService.GetActionByUid(apm.Uid));
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
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
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
                    () =>
                    {

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

        public ActionForPlaying(ModelAction value)
        {
            this.Checkpoints = new ObservableCollection<Checkpoint>();
            value?.Checkpoints?.ForEach(chk => this.Checkpoints.Add(chk));

            this.Uid = value.Uid;
            this.Name = value.Name;
            this.StartTime = value.StartTime;
            this.EndTime = this.StartTime.Add(this.Checkpoints.GetDuration());
            this.CurrentTime = DateTime.Now;
        }
        #endregion

        #region Properties

        #region EndTime
        private DateTime _endTime;

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

        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(this.LeftTime));
                NotifyPropertyChanged(nameof(this.RemainingTime));
            }
        }
        #endregion

        #endregion
    }
}
