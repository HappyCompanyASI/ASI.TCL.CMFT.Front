using System;
using System.Collections.Generic;
using ASI.TCL.CMFT.WPF.Module.DMD.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Dtos
{

   
    public class DMDScheduleTemplateDto
    {
        public Guid Id { get; set; }
        public string ScheduleName { get; set; }

        public eDayType StartDayType { get; set; }
        public DateTime StartTime { get; set; }

        public eDayType EndDayType { get; set; }
        public DateTime EndTime { get; set; }

        public IList<SYSStationDto> Targets { get; set; }
        public DMDPreRecordMessageDto Content { get; set; }
    }
    public class DMDDisplaySetting
    {
        public eDisplayPosition CurrentPosition { get; set; }
        public string CurrentColor { get; set; }
        public eMoveMode MoveMode { get; set; }
        public int MoveSpeed { get; set; }
        public eFontType FontType { get; set; }
    }
    public class DMDDisplayMode
    {
        public SYSStationDto Station { get; set; }
        public DMDDisplaySetting CDUSetting { get; set; }
        public DMDDisplaySetting PDUSetting { get; set; }
    }
}
