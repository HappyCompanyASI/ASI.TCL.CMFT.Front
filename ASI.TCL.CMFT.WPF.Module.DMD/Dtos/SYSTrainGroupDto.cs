using System;
using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Dtos
{
    public class SYSTrainGroupDto 
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public IList<SYSTrainDto> Trains { get; set; }
    }
}