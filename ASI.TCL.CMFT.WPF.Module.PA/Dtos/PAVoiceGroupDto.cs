using System;
using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class PAVoiceGroupDto
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public IList<PAPreRecordVoiceDto> Voices { get; set; }
    }
}