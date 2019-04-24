﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
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
                    _actionUidForLoad = asm.Uid;
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
            }
        }
        #endregion

        #region Services
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        #endregion

        #region Fields

        #region _actionUidForLoad
        private string _actionUidForLoad;
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
                        DialogHost.CloseDialogCommand.Execute(null, null);
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
                        this.EditedAction =
                            _actionUidForLoad == null ?
                            new ModelAction() :
                            await _dataService.GetActionByUidAsync(_actionUidForLoad);
                        this.EditedDate = this.EditedAction?.StartTime.Date ?? DateTime.Now;
                        this.EditedTime = new DateTime(0).Add(this.EditedAction?.StartTime.TimeOfDay ?? TimeSpan.FromTicks(0));
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
                        if (checkpoint.Uid == null)
                        {
                            checkpoint.Uid = Guid.NewGuid().ToString();
                            this.EditedAction.Checkpoints.Add(checkpoint);
                        }
                        else
                        {
                            var item = this.EditedAction.Checkpoints.FirstOrDefault(chk => chk.Uid == checkpoint.Uid);
                            var index = this.EditedAction.Checkpoints.IndexOf(item);
                            this.EditedAction.Checkpoints[index] = checkpoint;
                        }

                        this.EditedCheckpoint = new Checkpoint();
                        DialogHost.CloseDialogCommand.Execute(null, null);
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
                    async () =>
                    {
                        this.EditedAction.StartTime = this.EditedDate.Date.Add(this.EditedTime.TimeOfDay);
                        if (this.EditedAction.Uid == null)
                        {
                            await _dataService.AddActionAsync(this.EditedAction);
                        }
                        else
                        {
                            await _dataService.UpdateActionAsync(this.EditedAction);
                        }
                        _navigationService.Navigate(NavPage.Main);
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
}
