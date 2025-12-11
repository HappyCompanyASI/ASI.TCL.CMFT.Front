using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.OTCS.Dtos
{
    public class SYSTrainGroupDto 
    {
        public string GroupName { get; set; }
        public IEnumerable<SYSTrainDto> Trains { get; set; }
        public string Id { get; set; }
    }
}