using System;

namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class SYSConsoleDto 
    {
        public Guid Id { get; set; }
        public string SystemID { get; set; }
        public string ConsoleName { get; set; }

        public string IPAddress { get; set; }
        public string SetupLocation { get; set; }
        public string SeatName { get; set; }
        public string DLTNumber { get; set; }
        public string TetraNumber { get; set; }
    }
}