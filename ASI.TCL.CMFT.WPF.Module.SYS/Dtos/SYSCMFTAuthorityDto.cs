namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class SYSCMFTAuthorityDto
    {
        public SYSConsoleDto Console { get; set; }
        public bool IsDMDEnable { get; set; }
        public bool IsPAEnable { get; set; }
        public bool IsTetraEnable { get; set; }
        public bool IsOTCSEnable { get; set; }
        public bool IsSystemAlarmEnable { get; set; }
        public bool IsLogSearchEnable { get; set; }
    }
}