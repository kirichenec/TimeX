using GalaSoft.MvvmLight;

namespace TimeXv2.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public string Title { get; set; }

        public MainViewModel()
        {
            Title = "Hello, World!!";
        }
    }
}