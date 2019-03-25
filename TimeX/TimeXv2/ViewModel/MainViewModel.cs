using GalaSoft.MvvmLight;
using System.Collections.Generic;
using TimeXv2.Helpers;
using TimeXv2.Model;

namespace TimeXv2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string Title { get; set; }

        public MainViewModel()
        {
            var actions = new List<Action>();
            IDataWorker dataWorker = new FileWorker();
            try
            {
                actions = dataWorker.Load<List<Action>>(App.FilePath);
            }
            catch (System.Exception)
            {
                actions.Add(new Action() { Name = "FirstAction" });
                dataWorker.Save(actions, App.FilePath);
            }
        }
    }
}