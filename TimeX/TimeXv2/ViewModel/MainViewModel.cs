using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using TimeXv2.Helpers;
using TimeXv2.Model;
using TimeXv2.ViewModel.Navigation;

namespace TimeXv2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Services
        private readonly INavigationService _navigationService;
        #endregion

        #region ctor
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            var actions = new List<Action>();
            IDataWorker dataWorker = new FileWorker();
            try
            {
                actions = dataWorker.Load<List<Action>>(App.FilePath);
            }
            catch (System.Exception)
            {
                actions.Add(new Action() { Name = "FirstAction", StartTime = System.DateTime.Now });
                dataWorker.Save(actions, App.FilePath);
            }
            Actions.AddRange(actions);
        }
        #endregion

        #region Properties

        #region Actions
        /// <summary>
        /// The <see cref="Actions" /> property's name.
        /// </summary>
        public const string ActionsPropertyName = "Actions";

        private List<Action> _actions = new List<Action>();

        /// <summary>
        /// Sets and gets the Actions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<Action> Actions
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
                        _navigationService.Navigate(NavPage.ActionSettings);
                    }));
            }
        }
        #endregion

        #endregion
    }
}