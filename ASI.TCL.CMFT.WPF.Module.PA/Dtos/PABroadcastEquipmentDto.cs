using System;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class PABroadcastEquipmentDto
    {
        public Guid Id { get; set; }                         // 唯一識別碼
        public string ConsoleName { get; set; }              // 主控台名稱
        public string Location { get; set; }                 // 安裝地點
        public string SeatName { get; set; }                 // 座位名稱
        public bool IsStationBroadcastEnabled { get; set; }  // 車站廣播開關
    }
}
