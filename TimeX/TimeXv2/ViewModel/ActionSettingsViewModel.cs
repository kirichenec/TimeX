using GalaSoft.MvvmLight;
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
            MessengerInstance.Register<ModelAction>(this, action => this.EditedAction = action == null ? new ModelAction() : new ModelAction(action));
        }
        #endregion

        #region Services
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        #endregion

        #region Properties

        #region EditedAction
        /// <summary>
        /// The <see cref="EditedAction" /> property's name.
        /// </summary>
        public const string EditedActionPropertyName = "EditedAction";

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
                RaisePropertyChanged(EditedActionPropertyName);
            }
        }
        #endregion

        #region EditedCheckpoint
        /// <summary>
        /// The <see cref="EditedCheckpoint" /> property's name.
        /// </summary>
        public const string EditedCheckpointPropertyName = "EditedCheckpoint";

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
                RaisePropertyChanged(EditedCheckpointPropertyName);
            }
        }
        #endregion

        #region IsEdited
        /// <summary>
        /// The <see cref="IsEdited" /> property's name.
        /// </summary>
        public const string IsEditedPropertyName = "IsEdited";

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
                RaisePropertyChanged(IsEditedPropertyName);
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
                            checkpoint.Uid = Guid.NewGuid();
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
                    () =>
                    {
                        if (this.EditedAction.Uid == null)
                        {
                            _dataService.AddAction(this.EditedAction);
                        }
                        else
                        {
                            _dataService.UpdateAction(this.EditedAction);
                        }
                        _navigationService.Navigate(NavPage.Main);
                    }));
            }
        }
        #endregion

        #endregion
    }
}
