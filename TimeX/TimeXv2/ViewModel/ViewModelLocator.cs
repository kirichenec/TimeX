using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using TimeXv2.Model.Data;
using TimeXv2.ViewModel.Navigation;

namespace TimeXv2.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, NavigationService>(true);
            SimpleIoc.Default.Register<ActionContext>();
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<GlobalViewModel>(true);

            SimpleIoc.Default.Register<MainViewModel>(true);
            SimpleIoc.Default.Register<ActionPlayingViewModel>(true);
            SimpleIoc.Default.Register<ActionSettingsViewModel>(true);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ActionPlayingViewModel ActionPlaying
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ActionPlayingViewModel>();
            }
        }

        public ActionSettingsViewModel ActionSettings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ActionSettingsViewModel>();
            }
        }

        public GlobalViewModel GlobalViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GlobalViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}