using System.Collections.Generic;
using ASI.TCL.CMFT.WPF.Module.DLTS.Datas;

namespace ASI.TCL.CMFT.WPF.Module.DLTS.Dtos
{
    public class DLTPhoneGroupDto
    {
        public SYSStation Station { get; set; }
        public IEnumerable<DLTPhoneDto> Phones { get; set; }
    }
}
