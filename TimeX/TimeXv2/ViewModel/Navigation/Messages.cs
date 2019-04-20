using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeXv2.ViewModel.Navigation
{
    public class ActionPlayingMessage
    {
        public ActionPlayingMessage(string actionUid)
        {
            Uid = actionUid;
        }

        public string Uid { get; set; }
    }

    public class ActionSettingsMessage
    {
        public ActionSettingsMessage(string actionUid)
        {
            Uid = actionUid;
        }

        public string Uid { get; set; }
    }
}
