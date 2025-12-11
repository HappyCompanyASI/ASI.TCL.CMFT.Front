using System;
using ASI.TCL.CMFT.WPF.Module.DLTS.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.DLTS.Dtos
{
    public class DLTCallEventDto 
    {
        public DLTPhoneDto CallerPhone { get; set; }
        public DLTPhoneDto CalleePhone { get; set; }
        public eDLTCallState State { get; set; }
        public DateTime CallTime { get; set; }
        public string Id { get; set; }
    }

}
