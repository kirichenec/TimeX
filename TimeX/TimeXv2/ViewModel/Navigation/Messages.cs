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

        public ActionSettingsMessage() { }

        public string Uid { get; set; }
    }
}
