using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimeXv2.Extensions;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Navigation;
using ModelAction = TimeXv2.Model.Action;

namespace TimeXv2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region ctor
        public MainViewModel(INavigationService navigationService, IDataService dataService)
        {
            _dataService = dataService;
            _navigationService = navigationService;

            if (IsInDesignMode)
            {
                this.Actions = new ObservableCollection<ModelAction>
                {
                    new ModelAction() { Name = "Name", StartTime = DateTime.Now }
                };
                IsQueryExecuted = true;
            }
        }
        #endregion

        #region Services
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        #endregion

        #region Properties

        #region Actions
        private ObservableCollection<ModelAction> _actions = new ObservableCollection<ModelAction>();

        /// <summary>
        /// Sets and gets the Actions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ModelAction> Actions
        {
            get { return _actions; }
            set { Set(nameof(Actions), ref _actions, value); }
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

        #region CopyActionCommand
        private RelayCommand<ModelAction> _copyActionCommand;

        public RelayCommand<ModelAction> CopyActionCommand
        {
            get
            {
                return _copyActionCommand
                    ?? (_copyActionCommand = new RelayCommand<ModelAction>(
                        action =>
                        {

                            MessengerInstance.Send(new ActionSettingsMessage(action?.Uid ?? 0, isCopy: true));
                            _navigationService.Navigate(NavPage.ActionSettings);
                        }));
            }
        }
        #endregion

        #region DeleteActionCommand
        private RelayCommand<ModelAction> _deleteActionCommand;

        /// <summary>
        /// Gets the DeleteActionCommand.
        /// </summary>
        public RelayCommand<ModelAction> DeleteActionCommand
        {
            get
            {
                return _deleteActionCommand
                    ?? (_deleteActionCommand = new RelayCommand<ModelAction>(
                        action =>
                        {
                            IsQueryExecuted = false;
                            new RetryingDataService<bool, int>()
                                .RunTheMethod(_dataService.DeleteActionAsync, action.Uid).ContinueWith(
                                isDeletedAnswer =>
                                {
                                    var answer = isDeletedAnswer.Result;
                                    if (answer.Message != null)
                                    {
                                        Static.Properties.ShowMessage(answer.Message);
                                    }
                                    else if (answer.Result)
                                    {
                                        this.Actions.Remove(action);
                                    }
                                    IsQueryExecuted = true;
                                },
                            TaskScheduler.FromCurrentSynchronizationContext());
                        }));
            }
        }
        #endregion

        #region EditActionCommand
        private RelayCommand<ModelAction> _editActionCommand;

        /// <summary>
        /// Gets the EditActionCommand.
        /// </summary>
        public RelayCommand<ModelAction> EditActionCommand
        {
            get
            {
                return _editActionCommand
                    ?? (_editActionCommand = new RelayCommand<ModelAction>(
                    action =>
                    {
                        MessengerInstance.Send(new ActionSettingsMessage(action?.Uid ?? 0));
                        _navigationService.Navigate(NavPage.ActionSettings);
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

                        new RetryingDataService<List<ModelAction>, bool>()
                            .RunTheMethod(new Func<Task<List<ModelAction>>>(() => _dataService.GetActionsListAsync())).ContinueWith(
                            actionsAnswer =>
                            {
                                var answer = actionsAnswer.Result;
                                if (answer.Message != null)
                                {
                                    Static.Properties.ShowMessage(answer.Message);
                                }
                                else
                                {
                                    answer.Result?.ForEach(a => Actions.Add(a));
                                }
                                IsQueryExecuted = true;
                            },
                            TaskScheduler.FromCurrentSynchronizationContext());
                    }));
            }
        }
        #endregion

        #region NewActionCommand
        private RelayCommand _newActionCommand;

        /// <summary>
        /// Gets the NewActionCommand.
        /// </summary>
        public RelayCommand NewActionCommand
        {
            get
            {
                return _newActionCommand
                    ?? (_newActionCommand = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send(new ActionSettingsMessage());
                        _navigationService.Navigate(NavPage.ActionSettings);
                    }));
            }
        }
        #endregion

        #region PlayActionCommand
        private RelayCommand<ModelAction> _playActionCommand;

        public RelayCommand<ModelAction> PlayActionCommand
        {
            get
            {
                return _playActionCommand
                    ?? (_playActionCommand = new RelayCommand<ModelAction>(
                        action =>
                        {
                            SendPlayingMessageAndNavigate(action?.Uid ?? 0);
                        }));
            }
        }
        #endregion

        #region PlayActionNowCommand
        private RelayCommand<ModelAction> _playActionNowCommand;

        public RelayCommand<ModelAction> PlayActionNowCommand
        {
            get
            {
                return _playActionNowCommand
                    ?? (_playActionNowCommand = new RelayCommand<ModelAction>(
                        action =>
                        {
                            SendPlayingMessageAndNavigate(action?.Uid ?? 0, DateTime.Now);
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
                        Actions.Clear();
                    }));
            }
        }
        #endregion

        #endregion

        #region Methods

        #region SendPlayingMessageAndNavigate
        private void SendPlayingMessageAndNavigate(int actionUid, DateTime? startTime = null)
        {
            MessengerInstance.Send(new ActionPlayingMessage(actionUid, startTime));
            _navigationService.Navigate(NavPage.ActionPlaying);
        }
        #endregion

        #endregion
    }
}