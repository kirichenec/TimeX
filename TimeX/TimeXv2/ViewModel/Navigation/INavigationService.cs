namespace TimeXv2.ViewModel.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Переход на страницу
        /// </summary>
        /// <param name="page">Имя страницы</param>
        void Navigate(NavPage page);
    }

    /// <summary>
    /// Вид страницы
    /// </summary>
    public enum NavPage
    {
        /// <summary>
        /// Главная страница
        /// </summary>
        Main,
        /// <summary>
        /// Страница настройки мероприятия
        /// </summary>
        ActionSettings
    }
}
