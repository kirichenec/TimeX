using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
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
                        new ModelAction() :
                        _dataService.GetActionByUid(apm.Uid);
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
                        ParentAction = this.PlayedAction,
                        Name = "new"
                    }
                };
                this.PlayedAction =
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

        #region Properties

        #region PlayedAction
        private ModelAction _playedAction = null;

        /// <summary>
        /// Sets and gets the <see cref="PlayedAction"/> property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ModelAction PlayedAction
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
}
