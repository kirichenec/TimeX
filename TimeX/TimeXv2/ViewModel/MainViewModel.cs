﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Linq;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Navigation;
using ActionModel = TimeXv2.Model.Action;

namespace TimeXv2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region ctor
        public MainViewModel(INavigationService navigationService, IDataService dataService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
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

        private ObservableCollection<ActionModel> _actions = new ObservableCollection<ActionModel>();

        /// <summary>
        /// Sets and gets the Actions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ActionModel> Actions
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
        private RelayCommand<ActionModel> _deleteActionCommand;

        /// <summary>
        /// Gets the DeleteActionCommand.
        /// </summary>
        public RelayCommand<ActionModel> DeleteActionCommand
        {
            get
            {
                return _deleteActionCommand
                    ?? (_deleteActionCommand = new RelayCommand<ActionModel>(
                    action =>
                    {
                        this.Actions.Remove(action);
                        DialogHost.CloseDialogCommand.Execute(null, null);
                    }));
            }
        }
        #endregion

        #region EditActionCommand
        private RelayCommand<ActionModel> _editActionCommand;

        /// <summary>
        /// Gets the EditActionCommand.
        /// </summary>
        public RelayCommand<ActionModel> EditActionCommand
        {
            get
            {
                return _editActionCommand
                    ?? (_editActionCommand = new RelayCommand<ActionModel>(
                    action =>
                    {
                        MessengerInstance.Send(action);
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
                        MessengerInstance.Send((ActionModel)null);
                        _navigationService.Navigate(NavPage.ActionSettings);
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