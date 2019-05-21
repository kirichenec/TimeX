using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using TimeXv2.Model;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Model;
using TimeXv2.ViewModel.Navigation;
using ModelAction = TimeXv2.Model.Action;

namespace TimeXv2.ViewModel
{
    public class ActionPlayingViewModel : ViewModelBase
    {
        #region ctor
        public ActionPlayingViewModel(IDataService dataService)
        {
            _dataService = dataService;

            MessengerInstance
                .Register<ActionPlayingMessage>(this, apm =>
                {
                    _actionPlayingMessage = apm;
                });

            if (IsInDesignMode)
            {
                this.PlayedAction =
                    new ActionForPlaying()
                    {
                        Name = "Name",
                        StartTime = DateTime.Now.Date
                    };
                var chk = new ObservableCollection<CheckpointForPlaying>
                {
                    new CheckpointForPlaying()
                    {
                        StartTime = TimeSpan.FromMinutes(0),
                        Duration = TimeSpan.FromDays(1),
                        IsOrderNeeded = true,
                        Name = "new"
                    }
                };
                PlayedAction.Checkpoints = chk;
                IsQueryExecuted = true;
            }
        }
        #endregion

        #region Services
        private readonly IDataService _dataService;
        #endregion

        #region Fields

        #region _actionUidForLoad
        private ActionPlayingMessage _actionPlayingMessage;
        #endregion

        #region _timerDelay
        private TimeSpan _timerDelay = TimeSpan.FromSeconds(0.1);
        #endregion

        #endregion

        #region Properties

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

        #region IsPlay
        private bool _isPlay = false;

        public bool IsPlay
        {
            get { return _isPlay; }
            set
            {
                if (_isPlay == value)
                {
                    return;
                }
                _isPlay = value;
                RaisePropertyChanged(nameof(IsPlay));
            }
        }
        #endregion

        #region IsQueryExecuted
        private bool _isQueryExecuted = false;

        public bool IsQueryExecuted
        {
            get { return _isQueryExecuted; }
            set { Set(nameof(IsQueryExecuted), ref _isQueryExecuted, value); }
        }
        #endregion

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

        #region CheckCheckpointCommand
        private RelayCommand<CheckpointForPlaying> _checkCheckpointCommand;

        public RelayCommand<CheckpointForPlaying> CheckCheckpointCommand
        {
            get
            {
                return _checkCheckpointCommand ??
                    (_checkCheckpointCommand = new RelayCommand<CheckpointForPlaying>(
                        value =>
                        {
                            value.CheckedDate = DateTime.Now;
                            new RetryingDataService<bool, Checkpoint>()
                                .RunTheMethod(_dataService.UpdateCheckpointAsync, value.ToCheckpoint())
                                .ContinueWith(
                                    boolAnswer =>
                                    {
                                        var answer = boolAnswer.Result;
                                        if (!string.IsNullOrEmpty(answer.Message))
                                        {
                                            Static.Properties.ShowMessage(answer.Message);
                                        }
                                        else if (answer.Result)
                                        {
                                            Static.Properties.ShowMessage("Событие обновлено");
                                        }
                                    },
                                    TaskScheduler.FromCurrentSynchronizationContext());
                        }));
            }
        }
        #endregion

        #region CheckpointsLoadedCommand
        private RelayCommand<ListCollectionView> _checkpointsLoadedCommand;

        public RelayCommand<ListCollectionView> CheckpointsLoadedCommand
        {
            get
            {
                return _checkpointsLoadedCommand ??
                    (_checkpointsLoadedCommand = new RelayCommand<ListCollectionView>(
                        checkpointsSource =>
                        {
                            checkpointsSource.Filter = FilterCheckpoints;
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
                        IsQueryExecuted = false;
                        new RetryingDataService<ModelAction, int>()
                            .RunTheMethod(_dataService.GetActionByUidAsync, _actionPlayingMessage.Uid)
                            .ContinueWith(
                                actionAnswer =>
                                {
                                    var answer = actionAnswer.Result;
                                    if (!string.IsNullOrEmpty(answer.Message))
                                    {
                                        Static.Properties.ShowMessage(answer.Message);
                                    }
                                    else
                                    {
                                        answer.Result.StartTime = _actionPlayingMessage.StartTime ?? answer.Result.StartTime;
                                        this.PlayedAction = new ActionForPlaying(answer.Result);

                                        IsPlay = true;
                                        new Task(() => Timer()).Start();
                                        IsQueryExecuted = true;
                                    }
                                },
                                TaskScheduler.FromCurrentSynchronizationContext());
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
                        this.IsPlay = false;
                        PlayedAction = null;
                    }));
            }
        }
        #endregion

        #endregion

        #region Methods

        #region FilterCheckpoints
        private bool FilterCheckpoints(object obj)
        {
            var value = obj as CheckpointForPlaying;
            return
                IsExpanded ||
                (value.CurrentPercent != 0 && value.CurrentPercent != 100);
        }
        #endregion

        #region Timer
        private async void Timer()
        {
            do
            {
                this.PlayedAction.CurrentTime = DateTime.Now;
                await Task.Delay(_timerDelay);
            } while (IsPlay);
        }
        #endregion

        #endregion
    }
}
