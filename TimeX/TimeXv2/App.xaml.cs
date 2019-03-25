using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace TimeXv2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string FilePath { get; set; } = "1.txt";

        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
