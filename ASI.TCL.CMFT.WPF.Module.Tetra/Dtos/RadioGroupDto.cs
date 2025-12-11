using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.Dtos
{
    public class RadioGroupDto 
    {
        public string GroupName { get; set; }
        public IEnumerable<RadioDto> Radios { get; set; }
        public string Id { get; set; }
    }
}