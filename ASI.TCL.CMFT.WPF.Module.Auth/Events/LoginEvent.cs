using Prism.Events;

namespace ASI.TCL.CMFT.WPF.Module.Auth.Events
{
    public class LoginEvent : PubSubEvent<LoginInfo> { }

    public class LoginInfo
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
}
