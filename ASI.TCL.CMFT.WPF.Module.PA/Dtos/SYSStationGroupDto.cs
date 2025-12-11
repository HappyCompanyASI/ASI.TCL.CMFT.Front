using System;
using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class SYSStationGroupDto 
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public IList<SYSStationDto> Stations { get; set; }
    }
}