using System;

namespace ASI.TCL.CMFT.WPF.Module.Alarm.Dtos
{
    public class EquipAlarmDto 
    {
        public string Id { get; set; }
        public DateTime AlarmTime { get; set; }
        public DateTime? ReleaseTime { get; set; }
       
        public string EquipDescription { get; set; }
        public string Location { get; set; }
        public string SystemType { get; set; }
        public eAlarmLevel AlarmLevel { get; set; }

        public string AlarmDescription { get; set; }
        public string ConfirmedUserName { get; set; }
        public bool? IsConfirmed { get; set; }
        public DateTime? ConfirmedTime { get; set; }
    }
}