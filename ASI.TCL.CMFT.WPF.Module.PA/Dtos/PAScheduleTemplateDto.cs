using System;
using System.Collections.Generic;
using ASI.TCL.CMFT.WPF.Module.PA.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class PAScheduleTemplateDto 
    {
        public Guid Id { get; set; }
        public string ScheduleName { get; set; }

        public eDayType StartDayType { get; set; }
        public DateTime StartTime { get; set; }

        public eDayType EndDayType { get; set; }
        public DateTime EndTime { get; set; }

        public IList<SYSStationDto> Targets { get; set; }
        public PAPreRecordVoiceDto Content { get; set; }
    }
}