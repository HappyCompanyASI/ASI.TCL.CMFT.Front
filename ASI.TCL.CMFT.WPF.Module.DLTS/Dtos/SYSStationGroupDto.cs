using System.Collections.Generic;
using ASI.TCL.CMFT.WPF.Module.DLTS.Datas;

namespace ASI.TCL.CMFT.WPF.Module.DLTS.Dtos
{
    public class SYSStationGroupDto 
    {
        public string GroupName { get; set; }
        public IEnumerable<SYSStation> Stations { get; set; }
        public string Id { get; set; }
    }
}