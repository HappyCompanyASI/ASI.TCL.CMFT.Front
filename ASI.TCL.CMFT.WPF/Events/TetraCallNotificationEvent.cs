using System;
using Prism.Events;

namespace ASI.TCL.CMFT.WPF.Events
{
    // 無線電來電通知事件
    public class TetraCallNotificationEvent : PubSubEvent<TetraCallNotificationEventArgs>
    {
    }

    public class TetraCallNotificationEventArgs
    {
        public string PhoneNumber { get; set; }
        public string BaseStation { get; set; }
        public string CallType { get; set; }
        public DateTime CallTime { get; set; }
        
        public TetraCallNotificationEventArgs(string phoneNumber, string baseStation, string callType)
        {
            PhoneNumber = phoneNumber;
            BaseStation = baseStation;
            CallType = callType;
            CallTime = DateTime.Now;
        }
    }

    // 無線電接聽事件
    public class TetraCallAnsweredEvent : PubSubEvent<TetraCallAnsweredEventArgs>
    {
    }

    public class TetraCallAnsweredEventArgs
    {
        public string CallId { get; set; }
        public string PhoneNumber { get; set; }
        public string BaseStation { get; set; }
        public string CallType { get; set; }
        public DateTime AnsweredTime { get; set; }

        public TetraCallAnsweredEventArgs(string callId, string phoneNumber, string baseStation, string callType)
        {
            CallId = callId;
            PhoneNumber = phoneNumber;
            BaseStation = baseStation;
            CallType = callType;
            AnsweredTime = DateTime.Now;
        }
    }
}
