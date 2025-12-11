using System;

namespace ASI.TCL.CMFT.WPF.Module.Alarm.Dtos
{
    public class SYSOperationLogDto 
    {
        //EventTime  : 在什麼時候
        //Account    : 誰
        //Console    : 用了哪一台
        //SystemType : 對哪個系統
        //Content    : 做什麼事
        public DateTime EventTime { get; set; }
        //public AccountDto AccountDto { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public SYSConsoleDto Console { get; set; }
        public eSystemType SystemType { get; set; }
        public string Content { get; set; }
        public string Id { get; set; }
    }
}
