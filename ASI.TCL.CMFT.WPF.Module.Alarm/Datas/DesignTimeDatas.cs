using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.Alarm.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.Alarm.Datas
{
    internal class DesignTimeDatas
    {
        // 基礎數據 - 沒有依賴關係
        public static IEnumerable<eSystemType> SystemTypes { get; private set; } = GenerateSystemTypes();
        public static IEnumerable<SYSEquipTypeDto> SYSEquipTypes { get; set; } = GenerateSYSEquipTypes();
        public static IEnumerable<SYSConsoleDto> SYSConsoles { get; private set; } = GenerateSYSConsoles();
        public static IEnumerable<AlarmLevelDefineDto> AlarmLevelDefines { get; private set; } = GenerateAlarmLevelDefines();
        public static IEnumerable<SYSTrainDto> SYSTrains { get; private set; } = GenerateSYSTrains();

        // 依賴基礎數據的項目
        public static IEnumerable<SYSEquipDto> SYSEquips { get; set; } = GenerateSYSEquips();
        public static IEnumerable<SYSOperationLogDto> SYSOperationLogs { get; private set; } = GenerateSYSOperationLogs();
        public static IEnumerable<String> AlarmLevelColors { get; private set; } = GenerateAlarmLevelColors();
        public static IEnumerable<EquipAlarmTypeDefineDto> EquipAlarmTypeDefines { get; private set; } = GenerateEquipAlarmTypeDefines();
        public static IEnumerable<EventAlarmTypeDefineDto> EventAlarmTypeDefines { get; private set; } = GenerateEventAlarmTypeDefines();

        // 依賴多個其他項目的複雜項目
        public static IEnumerable<EquipAlarmDto> EquipAlarmInfos { get; set; } = GenerateEquipAlarmInfos();
        public static IEnumerable<EventAlarmDto> EventAlarmInfos { get; set; } = GenerateEventAlarmInfos();


        private static IEnumerable<eSystemType> GenerateSystemTypes()
        {
            return Enum.GetValues(typeof(eSystemType)).Cast<eSystemType>();
        }
        private static IEnumerable<SYSEquipTypeDto> GenerateSYSEquipTypes()
        {
            var temp = new List<SYSEquipTypeDto>()
            {
                new(){ SystemType = eSystemType.點矩陣,  EquipType = "伺服器" },//0
                new(){ SystemType = eSystemType.點矩陣,  EquipType = "工作站主機" },//1
                new(){ SystemType = eSystemType.點矩陣,  EquipType = "大廳層點矩陣顯示器" },//2
                new(){ SystemType = eSystemType.點矩陣,  EquipType = "單面點矩陣顯示器" },//3
                new(){ SystemType = eSystemType.點矩陣,  EquipType = "月臺層點矩陣顯示器" },//4
                new(){ SystemType = eSystemType.車站廣播,  EquipType = "伺服器" },//5
                new(){ SystemType = eSystemType.車站廣播,  EquipType = "消防主機" },//6
                new(){ SystemType = eSystemType.車站廣播,  EquipType = "廣播主機" },//7
                new(){ SystemType = eSystemType.車站廣播,  EquipType = "擴大機1" },//8
                new(){ SystemType = eSystemType.車站廣播,  EquipType = "PAO廣播控制面盤" },//9
            };
            return temp;
        }
        private static IEnumerable<SYSEquipDto> GenerateSYSEquips()
        {
            var sysEquipTypes = SYSEquipTypes.ToList();
            var temp = new List<SYSEquipDto>()
            {
                new(){ EquipID = "DMD-001", EquipTypes = sysEquipTypes[0],  RegionID = "Y06", RegionName = "大坪林", AreaName="頂版層", PlaceName="通訊維修員工室" },
                new(){ EquipID = "PA-001", EquipTypes = sysEquipTypes[7],  RegionID = "Y06", RegionName = "大坪林", AreaName="頂版層", PlaceName="通訊維修員工室"},
                new(){ EquipID = "PA-002", EquipTypes = sysEquipTypes[9],  RegionID = "Y06", RegionName = "大坪林", AreaName="頂版層", PlaceName="通訊維修員工室"},
                new(){ EquipID = "DMD-002", EquipTypes = sysEquipTypes[1],  RegionID = "Y06", RegionName = "大坪林", AreaName="大廳層", PlaceName="服務台" },
                new(){ EquipID = "PA-003", EquipTypes = sysEquipTypes[8],  RegionID = "Y06", RegionName = "大坪林", AreaName="月台層", PlaceName="廣播室" },
            };
            return temp;
        }
        private static IEnumerable<SYSConsoleDto> GenerateSYSConsoles()
        {
            var temp = new List<SYSConsoleDto>()
            {
                new() { Id = Guid.NewGuid().ToString(), SystemID = "Console1", ConsoleName = "通訊多功能操作台1", IPAddress = "10.7.0.51", DLTNumber ="2301", TetraNumber = "51001", SetupLocation = "行控中心", SeatName = "正線控制1" },
                new() { Id = Guid.NewGuid().ToString(), SystemID = "Console2", ConsoleName = "通訊多功能操作台2", IPAddress = "10.7.0.52", DLTNumber ="2302", TetraNumber = "51002", SetupLocation = "行控中心", SeatName = "正線控制2" },
                new() { Id = Guid.NewGuid().ToString(), SystemID = "Console3", ConsoleName = "通訊多功能操作台3", IPAddress = "10.7.0.53", DLTNumber ="2303", TetraNumber = "51003", SetupLocation = "行控中心", SeatName = "車站/電力控制" },
                new() { Id = Guid.NewGuid().ToString(), SystemID = "Console4", ConsoleName = "通訊多功能操作台4", IPAddress = "10.7.0.54", DLTNumber ="2304", TetraNumber = "51004", SetupLocation = "行控中心", SeatName = "南機廠控制" },
                new() { Id = Guid.NewGuid().ToString(), SystemID = "Console5", ConsoleName = "通訊多功能操作台5", IPAddress = "10.7.0.55", DLTNumber ="2305", TetraNumber = "51005", SetupLocation = "行控中心", SeatName = "主任控制" },
                new() { Id = Guid.NewGuid().ToString(), SystemID = "Console6", ConsoleName = "通訊多功能操作台6", IPAddress = "10.7.0.56", DLTNumber ="2306", TetraNumber = "51006", SetupLocation = "通訊維修室", SeatName = "通訊維修" },
                new() { Id = Guid.NewGuid().ToString(), SystemID = "Console7", ConsoleName = "通訊多功能操作台7", IPAddress = "10.7.0.57", DLTNumber ="2307", TetraNumber = "51007", SetupLocation = "測試軌控制室", SeatName = "測試軌控制" },
            };
            return temp;
        }
        private static IEnumerable<SYSOperationLogDto> GenerateSYSOperationLogs()
        {
            var consoles = SYSConsoles.ToList();
            
            var temp = new List<SYSOperationLogDto>()
            {
                new() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = "admin0" , UserName = "名稱0" ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送即時訊息" },
                new() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = "admin0" , UserName = "名稱0" ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送預路訊息" },
                new() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = "admin0" , UserName = "名稱0" ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送即時訊息" },
                new() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = "admin0" , UserName = "名稱0" ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="修改ATS訊息設定" },
                new() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = "admin0" , UserName = "名稱0" ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送預錄訊息" },
                new() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = "admin0" , UserName = "名稱0" ,Console = consoles[1], SystemType = eSystemType.車站廣播,  Content ="發送預錄廣播" },
                new() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = "admin0" , UserName = "名稱0" ,Console = consoles[2], SystemType = eSystemType.車站廣播,  Content ="發送預錄廣播" },
            };
            return temp;
        }
        private static IEnumerable<AlarmLevelDefineDto> GenerateAlarmLevelDefines()
        {
            var temp = new List<AlarmLevelDefineDto>()
            {
                new() { AlarmLevel = eAlarmLevel.重大, IsNeedConfirm = true, Color  = "#ff0000", AlarmSound = eAlarmSound.警示音1, IsFlashing = true, IsToSCADA = true },
                new() { AlarmLevel = eAlarmLevel.中度  , IsNeedConfirm = true, Color  = "#ffff00", AlarmSound = eAlarmSound.警示音2, IsFlashing = true, IsToSCADA = true },
                new() { AlarmLevel = eAlarmLevel.輕微  , IsNeedConfirm = true, Color  = "#ffa500", AlarmSound = eAlarmSound.警示音3, IsFlashing = true, IsToSCADA = true },
            };
            return temp;
        }
        private static IEnumerable<String> GenerateAlarmLevelColors()
        {
            //這個方法會回傳上面五種顏色的資料集合
            return AlarmLevelDefines.Select(x => x.Color).ToList();
        }
        private static IEnumerable<EquipAlarmTypeDefineDto> GenerateEquipAlarmTypeDefines()
        {
            var alarmLevelDefines = AlarmLevelDefines.ToList();
            var sysEquipTypes = SYSEquipTypes.ToList();

            var temp = new List<EquipAlarmTypeDefineDto>()
            {
                new() { EquipAlarmTypeID = "PA-001", EquipType = sysEquipTypes[7], Description = "廣播主機發生錯誤", DescriptionENG = "PA error", PossibleReason = "廣播主機故障。", PossibleReasonENG = "PA error", AlarmLevelDefine = alarmLevelDefines[0] },
                new() { EquipAlarmTypeID = "PA-002", EquipType = sysEquipTypes[6], Description = "消防主機", DescriptionENG = "消防主機錯誤", PossibleReason = "消防主機錯誤", PossibleReasonENG = "消防主機錯誤", AlarmLevelDefine = alarmLevelDefines[1] },
                new() { EquipAlarmTypeID = "PA-003", EquipType = sysEquipTypes[9], Description = "PAO廣播控制面盤錯誤", DescriptionENG = "PAO廣播控制面盤錯誤", PossibleReason = "廣播主機發生錯誤", PossibleReasonENG = "廣播主機發生錯誤", AlarmLevelDefine = alarmLevelDefines[1] },
                new() { EquipAlarmTypeID = "PA-004", EquipType = sysEquipTypes[0], Description = "伺服器控制面盤錯誤", DescriptionENG = "伺服器控制面盤錯誤", PossibleReason = "伺服器發生錯誤", PossibleReasonENG = "伺服器發生錯誤", AlarmLevelDefine = alarmLevelDefines[0] },
                new() { EquipAlarmTypeID = "PA-005", EquipType = sysEquipTypes[1], Description = "工作站控制面盤錯誤", DescriptionENG = "工作站控制面盤錯誤", PossibleReason = "工作站主機發生錯誤", PossibleReasonENG = "工作站主機發生錯誤", AlarmLevelDefine = alarmLevelDefines[2] }
            };

            return temp;
        }
        private static IEnumerable<EquipAlarmDto> GenerateEquipAlarmInfos()
        {
            // 準備容器
            var temp = new List<EquipAlarmDto>();
            var now = DateTime.Now;

            // 定義三種等級
            var levels = new[] { eAlarmLevel.重大, eAlarmLevel.中度, eAlarmLevel.輕微 };

            // 定義四種狀態組合 (解除狀態, 確認狀態)
            var states = new[]
            {
                new { IsReleased = false, IsConfirmed = false, Desc = "未解除/未確認 (閃爍)" },
                new { IsReleased = false, IsConfirmed = true,  Desc = "未解除/已確認 (靜態底色)" },
                new { IsReleased = true,  IsConfirmed = false, Desc = "已解除/未確認 (灰底分色字)" },
                new { IsReleased = true,  IsConfirmed = true,  Desc = "已解除/已確認 (正常結案)" }
            };

            int index = 1;
            foreach (var state in states)
            {
                foreach (var level in levels)
                {
                    temp.Add(new EquipAlarmDto()
                    {
                        Id = index.ToString(),
                        AlarmLevel = level,
                        // 處理解除時間：未解除則給 null，已解除則給現在時間
                        ReleaseTime = state.IsReleased ? now : (DateTime?)null,
                        // 處理確認狀態
                        IsConfirmed = state.IsConfirmed,
                        ConfirmedUserName = state.IsConfirmed ? "Admin" : string.Empty,

                        // 輔助資訊
                        AlarmTime = now.AddHours(-1),
                        SystemType = "OCC",
                        EquipDescription = $"設備-{index:00}",
                        AlarmDescription = $"[{level}] {state.Desc}",

                        // 如果你的 Style 有用到 ConfirmedTime，也要記得給值
                        ConfirmedTime = state.IsConfirmed ? now : (DateTime?)null
                    });
                    index++;
                }
            }

            // 最後將資料掛載到 UI 綁定的 Collection
            // this.EquipAlarmList = new ObservableCollection<EquipAlarmDto>(temp);

            return temp;
        }
        private static IEnumerable<EventAlarmTypeDefineDto> GenerateEventAlarmTypeDefines()
        {
            var alarmLevelDefines = AlarmLevelDefines.ToList();

            var temp = new List<EventAlarmTypeDefineDto>()
            {
                new()
                {
                    EventAlarmTypeID = "EVT-001",
                    Description = "車廂監視器異常",
                    DescriptionENG = "Car camera malfunction",
                    PossibleReason = "監視器硬體故障或連線中斷",
                    PossibleReasonENG = "Camera hardware failure or connection lost",
                    AlarmLevelDefine = alarmLevelDefines[0]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-002",
                    Description = "列車通訊中斷",
                    DescriptionENG = "Train communication lost",
                    PossibleReason = "通訊模組故障或訊號干擾",
                    PossibleReasonENG = "Communication module failure or signal interference",
                    AlarmLevelDefine = alarmLevelDefines[1]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-003",
                    Description = "車門無法正常開關",
                    DescriptionENG = "Door operation failure",
                    PossibleReason = "車門機構卡住或控制電路故障",
                    PossibleReasonENG = "Door mechanism stuck or control circuit failure",
                    AlarmLevelDefine = alarmLevelDefines[2]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-004",
                    Description = "煞車系統壓力異常",
                    DescriptionENG = "Brake system pressure abnormal",
                    PossibleReason = "煞車管路洩漏或壓縮機故障",
                    PossibleReasonENG = "Brake line leak or compressor failure",
                    AlarmLevelDefine = alarmLevelDefines[0]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-005",
                    Description = "空調溫度控制異常",
                    DescriptionENG = "Air conditioning temperature control abnormal",
                    PossibleReason = "溫控感測器故障或冷媒不足",
                    PossibleReasonENG = "Temperature sensor failure or refrigerant insufficient",
                    AlarmLevelDefine = alarmLevelDefines[1]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-006",
                    Description = "牽引馬達過熱",
                    DescriptionENG = "Traction motor overheating",
                    PossibleReason = "馬達負載過重或冷卻系統故障",
                    PossibleReasonENG = "Motor overload or cooling system failure",
                    AlarmLevelDefine = alarmLevelDefines[1]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-007",
                    Description = "車廂照明異常",
                    DescriptionENG = "Car lighting abnormal",
                    PossibleReason = "LED燈具故障或電源供應異常",
                    PossibleReasonENG = "LED fixture failure or power supply abnormal",
                    AlarmLevelDefine = alarmLevelDefines[1]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-008",
                    Description = "集電弓接觸不良",
                    DescriptionENG = "Pantograph contact poor",
                    PossibleReason = "集電弓磨耗或架空線接觸異常",
                    PossibleReasonENG = "Pantograph wear or overhead line contact abnormal",
                    AlarmLevelDefine = alarmLevelDefines[0]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-009",
                    Description = "蓄電池電壓不足",
                    DescriptionENG = "Battery voltage insufficient",
                    PossibleReason = "蓄電池老化或充電系統故障",
                    PossibleReasonENG = "Battery aging or charging system failure",
                    AlarmLevelDefine = alarmLevelDefines[2]
                },
                new()
                {
                    EventAlarmTypeID = "EVT-010",
                    Description = "緊急煞車系統啟動",
                    DescriptionENG = "Emergency brake system activated",
                    PossibleReason = "乘客緊急煞車或系統自動啟動",
                    PossibleReasonENG = "Passenger emergency brake or system auto activation",
                    AlarmLevelDefine = alarmLevelDefines[0]
                }
            };

            return temp;
        }
        private static IEnumerable<EventAlarmDto> GenerateEventAlarmInfos()
        {
            var sysTrain = SYSTrains.ToList();
            var alarmDefines = EventAlarmTypeDefines.ToList();

            var temp = new List<EventAlarmDto>()
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddHours(-2),
                    ReleaseTime = null,
                    ConfirmedTime = null,
                    ConfirmedUserID = null,
                    ConfirmedUserName = null,
                    IsConfirmed = false,
                    Train = sysTrain[0],
                    CarNumber = "1",
                    EquipID = "CAM-001",
                    EquipName = "車廂監視器1",
                    CameraIDs = new List<string> { "CAM-001", "CAM-002" },
                    Remark = "前車廂監視設備",
                    AlarmDefine = alarmDefines[0]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddHours(-4),
                    ReleaseTime = DateTime.Now.AddHours(-1),
                    ConfirmedTime = DateTime.Now.AddHours(-3),
                    ConfirmedUserID = "admin1",
                    ConfirmedUserName = "控制員王小明",
                    IsConfirmed = true,
                    Train = sysTrain[1],
                    CarNumber = "2",
                    EquipID = "COMM-001",
                    EquipName = "列車通訊設備",
                    CameraIDs = new List<string> { "CAM-003" },
                    Remark = "主要通訊設備",
                    AlarmDefine = alarmDefines[1]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddHours(-6),
                    ReleaseTime = null,
                    ConfirmedTime = DateTime.Now.AddHours(-5),
                    ConfirmedUserID = "admin2",
                    ConfirmedUserName = "技術員李小華",
                    IsConfirmed = true,
                    Train = sysTrain[2],
                    CarNumber = "3",
                    EquipID = "DOOR-001",
                    EquipName = "車門控制器",
                    CameraIDs = new List<string> { "CAM-004", "CAM-005" },
                    Remark = "左側車門",
                    AlarmDefine = alarmDefines[2]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddMinutes(-30),
                    ReleaseTime = null,
                    ConfirmedTime = null,
                    ConfirmedUserID = null,
                    ConfirmedUserName = null,
                    IsConfirmed = false,
                    Train = sysTrain[3],
                    CarNumber = "4",
                    EquipID = "BRAKE-001",
                    EquipName = "煞車系統",
                    CameraIDs = new List<string> { "CAM-006" },
                    Remark = "主煞車系統",
                    AlarmDefine = alarmDefines[3]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddHours(-8),
                    ReleaseTime = DateTime.Now.AddHours(-7),
                    ConfirmedTime = DateTime.Now.AddHours(-7).AddMinutes(-30),
                    ConfirmedUserID = "admin3",
                    ConfirmedUserName = "維修員張小強",
                    IsConfirmed = true,
                    Train = sysTrain[4],
                    CarNumber = "5",
                    EquipID = "AC-001",
                    EquipName = "空調系統",
                    CameraIDs = new List<string> { "CAM-007", "CAM-008" },
                    Remark = "車廂空調",
                    AlarmDefine = alarmDefines[4]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddHours(-1),
                    ReleaseTime = null,
                    ConfirmedTime = DateTime.Now.AddMinutes(-30),
                    ConfirmedUserID = "admin4",
                    ConfirmedUserName = "主控員陳小美",
                    IsConfirmed = true,
                    Train = sysTrain[5],
                    CarNumber = "6",
                    EquipID = "TRAC-001",
                    EquipName = "牽引馬達",
                    CameraIDs = new List<string> { "CAM-009" },
                    Remark = "主牽引馬達",
                    AlarmDefine = alarmDefines[5]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddDays(-1),
                    ReleaseTime = DateTime.Now.AddDays(-1).AddHours(2),
                    ConfirmedTime = DateTime.Now.AddDays(-1).AddMinutes(15),
                    ConfirmedUserID = "admin5",
                    ConfirmedUserName = "技師劉小偉",
                    IsConfirmed = true,
                    Train = sysTrain[6],
                    CarNumber = "7",
                    EquipID = "LIGHT-001",
                    EquipName = "照明系統",
                    CameraIDs = new List<string> { "CAM-010", "CAM-011" },
                    Remark = "車廂照明",
                    AlarmDefine = alarmDefines[6]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddMinutes(-15),
                    ReleaseTime = null,
                    ConfirmedTime = null,
                    ConfirmedUserID = null,
                    ConfirmedUserName = null,
                    IsConfirmed = false,
                    Train = sysTrain[7],
                    CarNumber = "8",
                    EquipID = "PANTOGRAPH-001",
                    EquipName = "集電弓",
                    CameraIDs = new List<string> { "CAM-012" },
                    Remark = "主集電弓",
                    AlarmDefine = alarmDefines[7]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddHours(-12),
                    ReleaseTime = DateTime.Now.AddHours(-10),
                    ConfirmedTime = DateTime.Now.AddHours(-11),
                    ConfirmedUserID = "admin6",
                    ConfirmedUserName = "電機員蘇小芳",
                    IsConfirmed = true,
                    Train = sysTrain[8],
                    CarNumber = "9",
                    EquipID = "BATTERY-001",
                    EquipName = "蓄電池系統",
                    CameraIDs = new List<string> { "CAM-013", "CAM-014" },
                    Remark = "備用電源",
                    AlarmDefine = alarmDefines[8]
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlarmTime = DateTime.Now.AddMinutes(-45),
                    ReleaseTime = null,
                    ConfirmedTime = DateTime.Now.AddMinutes(-40),
                    ConfirmedUserID = "admin7",
                    ConfirmedUserName = "安全員黃小龍",
                    IsConfirmed = true,
                    Train = sysTrain[9],
                    CarNumber = "10",
                    EquipID = "EMERGENCY-001",
                    EquipName = "緊急煞車",
                    CameraIDs = new List<string> { "CAM-015" },
                    Remark = "緊急制動系統",
                    AlarmDefine = alarmDefines[9]
                }
            };

            return temp;
        }
        private static IEnumerable<SYSTrainDto> GenerateSYSTrains()
        {
            var temp = new List<SYSTrainDto>()
            {
               new() { TrainNumber = "V01"  },
               new() { TrainNumber = "V02"  },
               new() { TrainNumber = "V03"  },
               new() { TrainNumber = "V04"  },
               new() { TrainNumber = "V05"  },
               new() { TrainNumber = "V06"  },
               new() { TrainNumber = "V07"  },
               new() { TrainNumber = "V08"  },
               new() { TrainNumber = "V09"  },
               new() { TrainNumber = "V10"  },
               new() { TrainNumber = "V11"  },
               new() { TrainNumber = "V12"  },
               new() { TrainNumber = "V13"  },
               new() { TrainNumber = "V14"  },
               new() { TrainNumber = "V15"  },
               new() { TrainNumber = "V16"  },
               new() { TrainNumber = "V17"  },
               new() { TrainNumber = "V18"  },
               new() { TrainNumber = "V19"  },
               new() { TrainNumber = "V20"  },
               new() { TrainNumber = "V21"  },
               new() { TrainNumber = "V22"  },
               new() { TrainNumber = "V23"  },
               new() { TrainNumber = "V24"  },
               new() { TrainNumber = "V25"  },
            };

            return temp;
        }
    }
}
