using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class SYSStationGroupDto 
    {
        public string GroupName { get; set; }
        public IList<SYSStationDto> Stations { get; set; }
        public string Id { get; set; }
    }
}