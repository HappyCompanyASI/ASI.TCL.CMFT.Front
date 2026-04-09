using System;

namespace ASI.TCL.CMFT.WPF.Module.Alarm.Dtos
{
    public class EventAlarmDto
    {
        public DateTime AlarmTime { get; set; }
        public string Train { get; set; }
        public string CarNumber { get; set; }
        public string Description { get; set; }
        public eAlarmLevel AlarmLevel { get; set; }
    }
}