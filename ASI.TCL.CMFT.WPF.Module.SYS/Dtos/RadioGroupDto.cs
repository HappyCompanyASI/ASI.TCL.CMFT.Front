using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class RadioGroupDto 
    {
        public string GroupName { get; set; }
        public IList<RadioDto> Radios { get; set; }
        public string Id { get; set; }
    }
}