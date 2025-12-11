using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class SYSTrainGroupDto 
    {
        public string GroupName { get; set; }
        public IEnumerable<SYSTrainDto> Trains { get; set; }
        public string Id { get; set; }
    }
}