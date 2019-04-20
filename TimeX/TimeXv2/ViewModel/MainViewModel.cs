using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            }
        }
        #endregion

        #region Services
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        #endregion

        #region Properties

        #region Actions
        /// <summary>
        /// The <see cref="Actions" /> property's name.
        /// </summary>
        public const string ActionsPropertyName = "Actions";

        private ObservableCollection<ModelAction> _actions = new ObservableCollection<ModelAction>();

        /// <summary>
        /// Sets and gets the Actions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ModelAction> Actions
        {
            get
            {
                return _actions;
            }
            set
            {
                Set(ActionsPropertyName, ref _actions, value);
            }
        }
        #endregion

        #endregion

        #region Commands

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
                        this.Actions.Remove(action);
                        _dataService.DeleteAction(action.Uid);
                        DialogHost.CloseDialogCommand.Execute(null, null);
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
                        MessengerInstance.Send(new ActionSettingsMessage(action?.Uid));
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
                        var actions = _dataService.QueryableActions().ToList();
                        actions?.ForEach(a => Actions.Add(a));
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
                        MessengerInstance.Send((string)null);
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
                            MessengerInstance.Send(new ActionPlayingMessage(action?.Uid));
                            _navigationService.Navigate(NavPage.ActionPlaying);
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
    }
}