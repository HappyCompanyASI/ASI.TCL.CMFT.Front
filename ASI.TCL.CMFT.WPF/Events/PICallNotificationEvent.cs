using System;
using Prism.Events;

namespace ASI.TCL.CMFT.WPF.Events
{
    // PI 來電通知事件
    public class PICallNotificationEvent : PubSubEvent<PICallNotificationEventArgs>
    {
    }

    public class PICallNotificationEventArgs
    {
        public string TrainNumber { get; set; }
        public string Car { get; set; }
        public string PINumber { get; set; }
        public string BaseStation { get; set; }
        public DateTime CallTime { get; set; }
        
        public PICallNotificationEventArgs(string trainNumber, string car, string piNumber, string baseStation)
        {
            TrainNumber = trainNumber;
            Car = car;
            PINumber = piNumber;
            BaseStation = baseStation;
            CallTime = DateTime.Now;
        }
    }
}