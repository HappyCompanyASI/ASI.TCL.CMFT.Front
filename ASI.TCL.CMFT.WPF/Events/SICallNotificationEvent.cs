using System;
using Prism.Events;

namespace ASI.TCL.CMFT.WPF.Events
{

    // SI 來電通知事件
    public class SICallNotificationEvent : PubSubEvent<SICallNotificationEventArgs>
    {
    }

    public class SICallNotificationEventArgs
    {
        public string TrainNumber { get; set; }
        public string Car { get; set; }
        public string BaseStation { get; set; }
        public DateTime CallTime { get; set; }

        public SICallNotificationEventArgs(string trainNumber, string car, string baseStation)
        {
            TrainNumber = trainNumber;
            Car = car;
            BaseStation = baseStation;
            CallTime = DateTime.Now;
        }
    }
}
