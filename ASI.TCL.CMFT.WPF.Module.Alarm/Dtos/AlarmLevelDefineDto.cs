namespace ASI.TCL.CMFT.WPF.Module.Alarm.Dtos
{
    public enum eAlarmLevel
    {
        重大,
        中度,
        輕微
    }
    public enum eSystemType
    {
        閉路電視,
        直線電話, 
        點矩陣,
        列車通訊,
        車站廣播,
        無線電,
        通訊多功能
    }

    public enum eAlarmSound
    {
        警示音1,
        警示音2,
        警示音3,
        警示音4,
        警示音5,
        無警示音
    }

    public class SYSEquipTypeDto
    {
        // 設備所屬系統
        public eSystemType SystemType { get; set; }
        // 設備名稱
        public string EquipType { get; set; }
    }

    public class SYSEquipDto
    {
        // 設備ID
        public string EquipID { get; set; }
        // 設備類型
        public SYSEquipTypeDto EquipTypes { get; set; }
        // 設備所在位置
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string PlaceName { get; set; }
        public string AreaName { get; set; }
    }

    public class AlarmLevelDefineDto
    {
        public eAlarmLevel AlarmLevel { get; set; }
        public bool IsNeedConfirm { get; set; }
        public string Color { get; set; }
        public eAlarmSound AlarmSound { get; set; }
        public bool IsFlashing { get; set; }
        public bool IsToSCADA { get; set; }
    }

    public class EquipAlarmTypeDefineDto
    {
        public string EquipAlarmTypeID { get; set; }

        // 此條告警是哪個設備
        public SYSEquipTypeDto EquipType { get; set; }
        // 此設備出了什麼錯誤
        public string Description { get; set; }
        public string DescriptionENG { get; set; }
        public string PossibleReason { get; set; }
        public string PossibleReasonENG { get; set; }
        // 此錯誤是什麼等級的告警
        public AlarmLevelDefineDto AlarmLevelDefine { get; set; }
    }

    public class EventAlarmTypeDefineDto
    {
        public string EventAlarmTypeID { get; set; }

        // 只保留告警類型定義相關的資訊
        public string Description { get; set; }
        public string DescriptionENG { get; set; }
        public string PossibleReason { get; set; }
        public string PossibleReasonENG { get; set; }

        // 告警等級定義
        public AlarmLevelDefineDto AlarmLevelDefine { get; set; }
    }
}
