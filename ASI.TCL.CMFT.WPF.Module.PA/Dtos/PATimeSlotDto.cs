using System;
using ASI.TCL.CMFT.WPF.Module.PA.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class PATimeSlotDto 
    {
        public Guid Id { get; set; }
        public Guid DailyScheduleId { get; set; }        // FK → DailyBroadcastScheduleDto.Id
        public eDayType StartDayType { get; set; }
        public DateTime StartTime { get; set; }
        public eDayType EndDayType { get; set; }
        public DateTime EndTime { get; set; }
        public eDaypartType DayPartType { get; set; }
    }
}