using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeXv2.Model;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Navigation;
using ModelAction = TimeXv2.Model.Action;

namespace TimeXv2.ViewModel
{
    public class ActionSettingsViewModel : ViewModelBase
    {
        #region ctor
        public ActionSettingsViewModel(INavigationService navigationService, IDataService dataService)
        {
            _dataService = dataService;
            _navigationService = navigationService;

            MessengerInstance
                .Register<ActionSettingsMessage>(this, asm =>
                {
                    _actionForLoad = asm;
                });

            if (IsInDesignMode)
            {
                var chk = new System.Collections.ObjectModel.ObservableCollection<Checkpoint>
                {
                    new Checkpoint()
                    {
                        StartTime = TimeSpan.FromMinutes(0),
                        Duration = TimeSpan.FromMinutes(5),
                        IsOrderNeeded = true,
                        ParentAction = this.EditedAction,
                        Name = "new"
                    }
                };
                this.EditedAction =
                    new ModelAction()
                    {
                        Name = "Name",
                        StartTime = DateTime.Now,
                        StartTimeTicks = 0,
                        Checkpoints = chk
                    };
                IsQueryExecuted = true;
            }
        }
        #endregion

        #region Services
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        #endregion

        #region Fields

        #region _actionUidForLoad
        private ActionSettingsMessage _actionForLoad;
        #endregion

        #region _currentTask
        private Task _currentTask;
        #endregion

        #endregion

        #region Properties

        #region EditedAction
        private ModelAction _editedAction = null;

        /// <summary>
        /// Sets and gets the EditedAction property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ModelAction EditedAction
        {
            get
            {
                return _editedAction;
            }

            set
            {
                if (_editedAction == value)
                {
                    return;
                }

                _editedAction = value;
                RaisePropertyChanged(nameof(EditedAction));
            }
        }
        #endregion

        #region EditedCheckpoint
        private Checkpoint _editedCheckpoint = new Checkpoint();

        /// <summary>
        /// Sets and gets the EditedCheckpoint property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Checkpoint EditedCheckpoint
        {
            get
            {
                return _editedCheckpoint;
            }

            set
            {
                if (_editedCheckpoint == value)
                {
                    return;
                }

                _editedCheckpoint = value;
                RaisePropertyChanged(nameof(EditedCheckpoint));
            }
        }
        #endregion

        #region EditedCheckpointDate
        private DateTime _editedCheckpointDate = new DateTime(0);

        /// <summary>
        /// Sets and gets the EditedCheckpointDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime EditedCheckpointDate
        {
            get
            {
                return _editedCheckpointDate;
            }
            set
            {
                if (_editedCheckpointDate == value)
                {
                    return;
                }
                _editedCheckpointDate = value;
                RaisePropertyChanged(nameof(EditedCheckpointDate));
            }
        }
        #endregion

        #region EditedCheckpointTime
        private DateTime _editedCheckpointTime = new DateTime(0);

        /// <summary>
        /// Sets and gets the EditedCheckpointDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime EditedCheckpointTime
        {
            get
            {
                return _editedCheckpointTime;
            }
            set
            {
                if (_editedCheckpointTime == value)
                {
                    return;
                }
                _editedCheckpointTime = value;
                RaisePropertyChanged(nameof(EditedCheckpointTime));
            }
        }
        #endregion

        #region EditedDate
        private DateTime _editedDate = new DateTime(0);

        /// <summary>
        /// Sets and gets the EditedDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime EditedDate
        {
            get
            {
                return _editedDate;
            }

            set
            {
                if (_editedDate == value)
                {
                    return;
                }

                _editedDate = value;
                RaisePropertyChanged(nameof(EditedDate));
            }
        }
        #endregion

        #region EditedTime
        private DateTime _editedTime = new DateTime(0);

        /// <summary>
        /// Sets and gets the EditedTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime EditedTime
        {
            get
            {
                return _editedTime;
            }

            set
            {
                if (_editedTime == value)
                {
                    return;
                }

                _editedTime = value;
                RaisePropertyChanged(nameof(EditedTime));
            }
        }
        #endregion

        #region IsEdited
        private bool _isEdited = false;

        /// <summary>
        /// Sets and gets the IsEdited property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsEdited
        {
            get
            {
                return _isEdited;
            }

            set
            {
                if (_isEdited == value)
                {
                    return;
                }

                _isEdited = value;
                RaisePropertyChanged(nameof(IsEdited));
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

        #endregion

        #region Commands

        #region CancelCommand
        private RelayCommand _cancelCommand;

        /// <summary>
        /// Gets the CancelCommand.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                    ?? (_cancelCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.Navigate(NavPage.Main);
                    }));
            }
        }
        #endregion

        #region DeleteCheckpointCommand
        private RelayCommand<Checkpoint> _deleteCheckpointCommand;

        /// <summary>
        /// Gets the DeleteCheckpointCommand.
        /// </summary>
        public RelayCommand<Checkpoint> DeleteCheckpointCommand
        {
            get
            {
                return _deleteCheckpointCommand
                    ?? (_deleteCheckpointCommand = new RelayCommand<Checkpoint>(
                    checkpoint =>
                    {
                        this.EditedAction.Checkpoints.Remove(checkpoint);
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
                            .RunTheMethod(_dataService.GetActionByUidAsync, _actionForLoad.Uid)
                            .ContinueWith(
                                actionAnswer =>
                                {
                                    var answer = actionAnswer.Result;
                                    if (string.IsNullOrEmpty(answer.Message))
                                    {
                                        this.EditedAction = new ModelAction(answer.Result, _actionForLoad.IsCopy);

                                        this.EditedDate = this.EditedAction?.StartTime.Date ?? DateTime.Now;
                                        this.EditedTime = new DateTime(0).Add(this.EditedAction?.StartTime.TimeOfDay ?? TimeSpan.FromTicks(0));
                                    }
                                    else
                                    {
                                        Static.Properties.ShowMessage(answer.Message);
                                    }
                                    IsQueryExecuted = true;
                                },
                                TaskScheduler.FromCurrentSynchronizationContext());
                    }));
            }
        }
        #endregion

        #region SaveCheckpointCommand
        private RelayCommand<Checkpoint> _saveCheckpointCommand;

        /// <summary>
        /// Gets the SaveCheckpointCommand.
        /// </summary>
        public RelayCommand<Checkpoint> SaveCheckpointCommand
        {
            get
            {
                return _saveCheckpointCommand
                    ?? (_saveCheckpointCommand = new RelayCommand<Checkpoint>(
                    checkpoint =>
                    {
                        if (checkpoint.Uid == 0)
                        {
                            this.EditedAction.Checkpoints.Add(checkpoint);
                        }
                        else
                        {
                            var item = this.EditedAction.Checkpoints.FirstOrDefault(chk => chk.Uid == checkpoint.Uid);
                            var index = this.EditedAction.Checkpoints.IndexOf(item);
                            this.EditedAction.Checkpoints[index] = checkpoint;
                        }

                        this.EditedCheckpoint = new Checkpoint();
                    }));
            }
        }
        #endregion

        #region SaveCommand
        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(
                    () =>
                    {
                        IsQueryExecuted = false;
                        EditedAction.StartTime = this.EditedDate.Date.Add(EditedTime.TimeOfDay);

                        var function =
                            EditedAction.Uid == 0 ?
                            new Func<ModelAction, Task<bool>>(_dataService.AddActionAsync) :
                            new Func<ModelAction, Task<bool>>(_dataService.UpdateActionAsync);

                        new RetryingDataService<bool, ModelAction>()
                            .RunTheMethod(function, this.EditedAction)
                            .ContinueWith(
                                isSaveAnswer =>
                                {
                                    var answer = isSaveAnswer.Result;
                                    if (!string.IsNullOrEmpty(answer.Message))
                                    {
                                        Static.Properties.ShowMessage(answer.Message);
                                    }
                                    else if (answer.Result)
                                    {
                                        _navigationService.Navigate(NavPage.Main);
                                    }
                                    IsQueryExecuted = true;
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
                        this.EditedAction = null;
                    }));
            }
        }
        #endregion

        #endregion
    }
}
