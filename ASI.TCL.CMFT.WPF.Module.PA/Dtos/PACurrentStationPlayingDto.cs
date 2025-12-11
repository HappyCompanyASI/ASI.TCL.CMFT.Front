using System;
using System.Collections.Generic;
using ASI.TCL.CMFT.WPF.Module.PA.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class PACurrentStationPlayingDto 
    {
        public PAPreRecordVoiceDto VoiceContent { get; set; }
        public DateTime VoiceFrom { get; set; }
        public DateTime VoiceTo { get; set; }
        public IEnumerable<eStationID> VoiceArea { get; set; }
        public string Id { get; set; }
    }
}