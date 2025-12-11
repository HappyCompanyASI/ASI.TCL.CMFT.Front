using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class DLTPhoneGroupDto
    {
        public SYSStationDto Station { get; set; }
        public IEnumerable<DLTPhoneDto> Phones { get; set; }
    }
}
