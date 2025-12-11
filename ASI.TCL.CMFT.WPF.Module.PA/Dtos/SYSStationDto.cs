using ASI.TCL.CMFT.WPF.Module.PA.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class SYSStationDto
    {
        public eStationID StationID { get; set; }
        public string StationName { get; set; }
        public SYSStationGroupDto BelongGroup { get; set; }
    }
}