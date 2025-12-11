using System;
using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Dtos
{
    public class DMDMessageGroupDto 
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public IList<DMDPreRecordMessageDto> Messages { get; set; }
    }
}
