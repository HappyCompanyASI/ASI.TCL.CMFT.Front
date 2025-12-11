using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class SYSOTCSAuthorityDto
    {
        public SYSConsoleDto Console { get; set; }
        public IList<SYSAuthorityItemDto> Authorities { get; set; }
    }
}