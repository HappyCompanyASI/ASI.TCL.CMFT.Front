using System;
using ASI.TCL.CMFT.WPF.Module.DMD.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Dtos
{
    public class DMDATSMessageDto 
    {
        public Guid Id { get; set; }
        public eATSType ATSType { get; set; }
        public string Header { get; set; }
        public string CDU { get; set; }
        public string PDU { get; set; }
    }
}