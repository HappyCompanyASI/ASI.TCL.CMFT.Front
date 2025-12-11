using System;
using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Dtos
{
    public class DMDDayScheduleDto
    {
        public DayOfWeek Day { get; set; }                         // 用 .NET 內建的 enum：Sunday–Saturday
        public IList<DMDScheduleTemplateDto> Templates { get; set; }

        // 顯示用，例如「星期一」「星期二」…
        public string DayDisplay => Day switch
        {
            DayOfWeek.Monday => "星期一",
            DayOfWeek.Tuesday => "星期二",
            DayOfWeek.Wednesday => "星期三",
            DayOfWeek.Thursday => "星期四",
            DayOfWeek.Friday => "星期五",
            DayOfWeek.Saturday => "星期六",
            DayOfWeek.Sunday => "星期日",
            _ => ""
        };
    }
}
