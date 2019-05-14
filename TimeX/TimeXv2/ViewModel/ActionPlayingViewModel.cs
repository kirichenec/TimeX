using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Model;
using TimeXv2.ViewModel.Navigation;

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
                        StartTime = DateTime.Now.Date,
                        Uid = Guid.NewGuid().ToString()
                    };
                var chk = new ObservableCollection<CheckpointForPlaying>
                {
                    new CheckpointForPlaying()
                    {
                        StartTime = TimeSpan.FromMinutes(0),
                        Duration = TimeSpan.FromDays(1),
                        IsOrderNeeded = true,
                        Name = "new",
                        Uid = Guid.NewGuid().ToString()
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
                            _dataService.UpdateCheckpointAsync(value.ToCheckpoint()).ContinueWith(answer =>
                            {
                                #region if debug
#if DEBUG
                                var message = "Событие сохранено";
                                Static.Properties.Instance.MessageQueue.Enqueue(
                                    message,
                                    "OK",
                                    _ => { },
                                    message);
#endif
                                #endregion
                            },
                            TaskScheduler.FromCurrentSynchronizationContext());
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
                        _dataService.GetActionByUidAsync(_actionPlayingMessage?.Uid).ContinueWith(
                            action =>
                            {
                                if (action?.Result != null)
                                {
                                    action.Result.StartTime = _actionPlayingMessage.StartTime ?? action.Result.StartTime;
                                    this.PlayedAction = new ActionForPlaying(action.Result);

                                    IsPlay = true;
                                    new Task(() => Timer()).Start();
                                    IsQueryExecuted = true; 
                                }
                                else
                                {
                                    var message = "Ошибк загрузки мероприятия";
                                    Static.Properties.Instance.MessageQueue.Enqueue(
                                        message,
                                        "OK",
                                        _ => { },
                                        message);
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
                    }));
            }
        }
        #endregion

        #endregion

        #region Methods

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
