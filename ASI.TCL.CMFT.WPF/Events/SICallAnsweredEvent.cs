using System;
using Prism.Events;

namespace ASI.TCL.CMFT.WPF.Events
{
    // SI 接聽事件  
    public class SICallAnsweredEvent : PubSubEvent<SICallAnsweredEventArgs>
    {
    }

    public class SICallAnsweredEventArgs
    {
        public string CallId { get; set; }
        public string TrainNumber { get; set; }
        public string Car { get; set; }
        public string BaseStation { get; set; }
        public DateTime AnsweredTime { get; set; }

        public SICallAnsweredEventArgs(string callId, string trainNumber, string car, string baseStation)
        {
            CallId = callId;
            TrainNumber = trainNumber;
            Car = car;
            BaseStation = baseStation;
            AnsweredTime = DateTime.Now;
        }
    }
}
