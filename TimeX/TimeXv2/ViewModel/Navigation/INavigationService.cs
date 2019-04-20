namespace TimeXv2.ViewModel.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Переход на страницу
        /// </summary>
        /// <param name="page">Имя страницы</param>
        void Navigate(NavPage page);

        NavPage CurrentPage { get; }
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
        ActionSettings,
        /// <summary>
        /// Ход мероприятия
        /// </summary>
        ActionPlaying
    }
}
