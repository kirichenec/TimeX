using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System.Diagnostics.CodeAnalysis;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Navigation;

namespace TimeXv2.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ActionContext>(true);
            SimpleIoc.Default.Register<INavigationService, NavigationService>(true);
            SimpleIoc.Default.Register<IDataService, DataService>(true);
            SimpleIoc.Default.Register<MainViewModel>(true);
            SimpleIoc.Default.Register<ActionSettingsViewModel>(true);
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the ActionSettings property.
        /// </summary>
        public ActionSettingsViewModel ActionSettings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ActionSettingsViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}