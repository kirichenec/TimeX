using System;

namespace TimeXv2.ViewModel.Navigation
{
    public class ActionPlayingMessage
    {
        public ActionPlayingMessage(int actionUid, DateTime? startTime = null)
        {
            Uid = actionUid;
            StartTime = startTime;
        }

        public int Uid { get; set; }

        public DateTime? StartTime { get; set; }
    }

    public class ActionSettingsMessage
    {
        public ActionSettingsMessage(int actionUid, bool isCopy = false)
        {
            Uid = actionUid;
            IsCopy = isCopy;
        }

        public ActionSettingsMessage() { }

        public int Uid { get; set; }

        public bool IsCopy { get; set; }
    }
}
