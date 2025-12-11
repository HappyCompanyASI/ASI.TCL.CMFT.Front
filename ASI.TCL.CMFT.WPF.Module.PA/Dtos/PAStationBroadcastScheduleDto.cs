using System;
using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    /// <summary>
    /// 某一車站的廣播時段設定
    /// </summary>
    public class PAStationBroadcastScheduleDto
    {
        public Guid Id { get; set; }
        public Guid StationId { get; set; }          // FK → SYSStationDto.StationID
        public SYSStationDto Station { get; set; }
        public IList<PADailyBroadcastScheduleDto> DailySchedules { get; set; }
    }
}