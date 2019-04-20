using System;
using System.Windows.Navigation;

namespace TimeXv2.ViewModel.Navigation
{
    class NavigationService : INavigationService
    {
        public NavPage CurrentPage { get; private set; }

        /// <summary>
        /// Переход на страницу
        /// </summary>
        /// <param name="page">Имя страницы</param>
        public void Navigate(NavPage page)
        {
            if (CurrentPage != page)
            {
                Uri uri = new Uri($"View/{page.ToString()}View.xaml", UriKind.Relative);
                ((NavigationWindow)App.Current.MainWindow).Navigate(uri);
                CurrentPage = page;
            }
        }
    }
}
