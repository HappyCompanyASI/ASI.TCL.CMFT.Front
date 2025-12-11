using System;
using Prism.Events;

namespace ASI.TCL.CMFT.WPF.Events
{
    // PI 接聽事件
    public class PICallAnsweredEvent : PubSubEvent<PICallAnsweredEventArgs>
    {
    }

    public class PICallAnsweredEventArgs
    {
        public string CallId { get; set; }
        public string TrainNumber { get; set; }
        public string Car { get; set; }
        public string PINumber { get; set; }
        public string BaseStation { get; set; }
        public DateTime AnsweredTime { get; set; }

        public PICallAnsweredEventArgs(string callId, string trainNumber, string car, string piNumber, string baseStation)
        {
            CallId = callId;
            TrainNumber = trainNumber;
            Car = car;
            PINumber = piNumber;
            BaseStation = baseStation;
            AnsweredTime = DateTime.Now;
        }
    }
}
