using System;

namespace TimeXv2.ViewModel.Navigation
{
    public class ActionPlayingMessage
    {
        public ActionPlayingMessage(string actionUid, DateTime? startTime = null)
        {
            Uid = actionUid;
            StartTime = startTime;
        }

        public string Uid { get; set; }

        public DateTime? StartTime { get; set; }
    }

    public class ActionSettingsMessage
    {
        public ActionSettingsMessage(string actionUid)
        {
            Uid = actionUid;
        }

        public ActionSettingsMessage() { }

        public string Uid { get; set; }
    }
}
