using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.Dtos
{
    public class RadioContentDto
    {
        public string ChannelName { get; set; }
        public string ChannelNumber { get; set; }
        public List<RadioContentItemDto> Contents { get; set; }
    }
}
