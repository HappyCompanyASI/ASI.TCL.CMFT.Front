using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.DMD.DataTypes;
using ASI.TCL.CMFT.WPF.Module.DMD.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Datas
{
    internal class DesignTimeDatas
    {

        public static IEnumerable<int> Numbers { get; private set; } = GenerateNumbers();
        public static IEnumerable<eDayOfWeek> DayOfWeeks { get; private set; } = GenerateDayOfWeeks();
        public static IEnumerable<eDaypartType> DaypartTypes { get; private set; } = GenerateDaypartTypes();
        public static IEnumerable<eDayType> DayTypes { get; private set; } = GenerateDayTypes();
        public static IEnumerable<eSystemType> SystemTypes { get; private set; } = GenerateSystemTypes();
        public static IEnumerable<eDisplayPosition> DisplayPositions { get; private set; } = GenerateDisplayPositions();
        public static IEnumerable<eMoveMode> MoveModes { get; private set; } = GenerateMoveModes();
        public static IEnumerable<eFontType> FontTypes { get; private set; } = GenerateFontTypes();
        public static IEnumerable<SYSStationGroupDto> SYSStationGroups { get; private set; } = GenerateSYSStationGroups();
        public static IEnumerable<SYSStationDto> SYSStations { get; private set; } = GenerateSYSStations();

        public static IEnumerable<SYSTrainGroupDto> SYSTrainGroups { get; private set; } = GenerateSYSTrainGroups();
        public static IEnumerable<SYSTrainDto> SYSTrains { get; private set; } = GenerateSYSTrains();

        public static IEnumerable<DMDMessageGroupDto> DMDMessageGroups { get; private set; } = GenerateDMDMessageGroups();
        public static IEnumerable<DMDPreRecordMessageDto> DMDPreRecordMessages { get; private set; } = GenerateDMDPreRecordMessages();
        public static IEnumerable<DMDATSMessageDto> DMDATSMessages { get; private set; } = GenerateDMDATSMessages();
        public static IEnumerable<DMDScheduleTemplateDto> DMDScheduleTemplates { get; private set; } = GenerateDMDScheduleTemplates();
        public static IEnumerable<DMDDisplayMode> DMDDisplayModes { get; private set; } = GenerateDMDDisplayModes();
        public static IEnumerable<string> DMDBlockList { get; private set; } = GenerateDMDBlockList();
        public static IEnumerable<DMDDayScheduleDto> DMDDaySchedules { get; private set; } = GenerateDMDDaySchedules();
        public static DMDATSIntervalDto DMDATSInterval { get; private set; } = GenerateDMDATSInterval();

        private static IEnumerable<int> GenerateNumbers()
        {
            var temp = new List<int>();
            for (int i = 0; i < 120; i++)
            {
                temp.Add(i);
            }
            return temp;
        }
        private static IEnumerable<eDisplayPosition> GenerateDisplayPositions()
        {
            return Enum.GetValues(typeof(eDisplayPosition)).Cast<eDisplayPosition>();
        }
        private static IEnumerable<eMoveMode> GenerateMoveModes()
        {
            return Enum.GetValues(typeof(eMoveMode)).Cast<eMoveMode>();
        }
        private static IEnumerable<eFontType> GenerateFontTypes()
        {
            return Enum.GetValues(typeof(eFontType)).Cast<eFontType>();
        }
        private static IEnumerable<eDayType> GenerateDayTypes()
        {
            return Enum.GetValues(typeof(eDayType)).Cast<eDayType>();
        }
        private static IEnumerable<eDayOfWeek> GenerateDayOfWeeks()
        {
            return Enum.GetValues(typeof(eDayOfWeek)).Cast<eDayOfWeek>();
        }
        private static IEnumerable<eDaypartType> GenerateDaypartTypes()
        {
            return Enum.GetValues(typeof(eDaypartType)).Cast<eDaypartType>();
        }
        private static IEnumerable<eSystemType> GenerateSystemTypes()
        {
            return Enum.GetValues(typeof(eSystemType)).Cast<eSystemType>();
        }
        private static IEnumerable<SYSStationGroupDto> GenerateSYSStationGroups()
        {
            var temp = new List<SYSStationGroupDto>()
            {
                new SYSStationGroupDto() { Id = Guid.NewGuid(), GroupName = "全部車站"},
                new SYSStationGroupDto() { Id = Guid.NewGuid(), GroupName = "一期車站"},
                new SYSStationGroupDto() { Id = Guid.NewGuid(), GroupName = "二期車站"},
                new SYSStationGroupDto() { Id = Guid.NewGuid(), GroupName = "三期車站"},
                new SYSStationGroupDto() { Id = Guid.NewGuid(), GroupName = "單數車站"},
                new SYSStationGroupDto() { Id = Guid.NewGuid(), GroupName = "偶數車站"},
                new SYSStationGroupDto() { Id = Guid.NewGuid(), GroupName = "轉運車站"},
            };
            return temp;
        }
        private static IEnumerable<SYSStationDto> GenerateSYSStations()
        {
            var temp = new List<SYSStationDto>()
            {
               //南環段
               new SYSStationDto() { StationID = eStationID.Y01, StationName = "Y01"},
               new SYSStationDto() { StationID = eStationID.Y01A, StationName = "Y01A"},
               new SYSStationDto() { StationID = eStationID.Y02A, StationName = "Y02A"},
               new SYSStationDto() { StationID = eStationID.Y03, StationName = "Y03"},
               new SYSStationDto() { StationID = eStationID.Y04, StationName = "Y04"},
               new SYSStationDto() { StationID = eStationID.Y05, StationName = "Y05"},

               //新北環狀線
               new SYSStationDto() { StationID = eStationID.Y06, StationName = "Y06"},
               new SYSStationDto() { StationID = eStationID.Y07, StationName = "Y07"},
               new SYSStationDto() { StationID = eStationID.Y08, StationName = "Y08"},
               new SYSStationDto() { StationID = eStationID.Y09, StationName = "Y09"},
               new SYSStationDto() { StationID = eStationID.Y10, StationName = "Y10"},
               new SYSStationDto() { StationID = eStationID.Y11, StationName = "Y11"},
               new SYSStationDto() { StationID = eStationID.Y12, StationName = "Y12"},
               new SYSStationDto() { StationID = eStationID.Y13, StationName = "Y13"},
               new SYSStationDto() { StationID = eStationID.Y14, StationName = "Y14"},
               new SYSStationDto() { StationID = eStationID.Y15, StationName = "Y15"},
               new SYSStationDto() { StationID = eStationID.Y16, StationName = "Y16"},
               new SYSStationDto() { StationID = eStationID.Y17, StationName = "Y17"},
               new SYSStationDto() { StationID = eStationID.Y18, StationName = "Y18"},
               new SYSStationDto() { StationID = eStationID.Y19, StationName = "Y19"},

               //北環段
               new SYSStationDto() { StationID = eStationID.Y19A, StationName = "Y19A"},
               new SYSStationDto() { StationID = eStationID.Y19B, StationName = "Y19B"},
               new SYSStationDto() { StationID = eStationID.Y20, StationName = "Y20"},
               new SYSStationDto() { StationID = eStationID.Y21, StationName = "Y21"},
               new SYSStationDto() { StationID = eStationID.Y22, StationName = "Y22"},
               new SYSStationDto() { StationID = eStationID.Y23, StationName = "Y23"},
               new SYSStationDto() { StationID = eStationID.Y24, StationName = "Y24"},
               new SYSStationDto() { StationID = eStationID.Y25, StationName = "Y25"},
               new SYSStationDto() { StationID = eStationID.Y26, StationName = "Y26"},
               new SYSStationDto() { StationID = eStationID.Y27, StationName = "Y27"},
               new SYSStationDto() { StationID = eStationID.Y28, StationName = "Y28"},
               new SYSStationDto() { StationID = eStationID.Y29, StationName = "Y29"},

                //東環段
                new SYSStationDto() { StationID = eStationID.Y30, StationName = "Y30"},
                new SYSStationDto() { StationID = eStationID.Y31, StationName = "Y31"},
                new SYSStationDto() { StationID = eStationID.Y32, StationName = "Y32"},
                new SYSStationDto() { StationID = eStationID.Y33, StationName = "Y33"},
                new SYSStationDto() { StationID = eStationID.Y34, StationName = "Y34"},
                new SYSStationDto() { StationID = eStationID.Y35, StationName = "Y35"},
                new SYSStationDto() { StationID = eStationID.Y36, StationName = "Y36"},
                new SYSStationDto() { StationID = eStationID.Y37, StationName = "Y37"},
                new SYSStationDto() { StationID = eStationID.Y38, StationName = "Y38"},
                new SYSStationDto() { StationID = eStationID.Y39, StationName = "Y39"},
            };

            var groups = SYSStationGroups.ToList();

            //全部車站
            groups[0].Stations = new List<SYSStationDto>(temp);
            //一期車站
            groups[1].Stations = new List<SYSStationDto>() { temp[0], temp[1], temp[2] };
            //二期車站
            groups[2].Stations = new List<SYSStationDto>() { temp[2], temp[3], temp[4] };
            //三期車站
            groups[3].Stations = new List<SYSStationDto>() { temp[6], temp[8], temp[10] };
            //單數車站
            groups[4].Stations = new List<SYSStationDto>() { temp[14], temp[22], temp[23] };
            //偶數車站
            groups[5].Stations = new List<SYSStationDto>() { temp[25], temp[26], temp[27] };
            //轉運車站
            groups[6].Stations = new List<SYSStationDto>() { temp[30], temp[31], temp[33] };

            return temp;
        }
        private static IEnumerable<SYSTrainGroupDto> GenerateSYSTrainGroups()
        {
            var temp = new List<SYSTrainGroupDto>()
            {
               new SYSTrainGroupDto() { Id = Guid.NewGuid(), GroupName = "全部列車"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid(), GroupName = "一期列車"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid(), GroupName = "上行群組"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid(), GroupName = "下行群組"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid(), GroupName = "營運列車"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid(), GroupName = "非營運列車"},
            };
            return temp;
        }
        private static IEnumerable<SYSTrainDto> GenerateSYSTrains()
        {
            var temp = new List<SYSTrainDto>()
            {
               new SYSTrainDto() { TrainNumber = "V01"  },
               new SYSTrainDto() { TrainNumber = "V02"  },
               new SYSTrainDto() { TrainNumber = "V03"  },
               new SYSTrainDto() { TrainNumber = "V04"  },
               new SYSTrainDto() { TrainNumber = "V05"  },
               new SYSTrainDto() { TrainNumber = "V06"  },
               new SYSTrainDto() { TrainNumber = "V07"  },
               new SYSTrainDto() { TrainNumber = "V08"  },
               new SYSTrainDto() { TrainNumber = "V09"  },
               new SYSTrainDto() { TrainNumber = "V10"  },
               new SYSTrainDto() { TrainNumber = "V11"  },
               new SYSTrainDto() { TrainNumber = "V12"  },
               new SYSTrainDto() { TrainNumber = "V13"  },
               new SYSTrainDto() { TrainNumber = "V14"  },
               new SYSTrainDto() { TrainNumber = "V15"  },
               new SYSTrainDto() { TrainNumber = "V16"  },
               new SYSTrainDto() { TrainNumber = "V17"  },
               new SYSTrainDto() { TrainNumber = "V18"  },
               new SYSTrainDto() { TrainNumber = "V19"  },
               new SYSTrainDto() { TrainNumber = "V20"  },
               new SYSTrainDto() { TrainNumber = "V21"  },
               new SYSTrainDto() { TrainNumber = "V22"  },
               new SYSTrainDto() { TrainNumber = "V23"  },
               new SYSTrainDto() { TrainNumber = "V24"  },
               new SYSTrainDto() { TrainNumber = "V25"  },
            };

            var groups = SYSTrainGroups.ToList();


            groups[0].Trains = new List<SYSTrainDto>(temp);

            groups[1].Trains = new List<SYSTrainDto>() { temp[0], temp[1], temp[2] };
            //temp[0].BelongGroup = groups[1];
            //temp[1].BelongGroup = groups[1];
            //temp[2].BelongGroup = groups[1];

            groups[2].Trains = new List<SYSTrainDto>() { temp[3], temp[4] };
            //temp[3].BelongGroup = groups[2];
            //temp[4].BelongGroup = groups[2];

            groups[3].Trains = new List<SYSTrainDto>() { temp[5], temp[6], temp[7], temp[8] };
            //temp[5].BelongGroup = groups[3];
            //temp[6].BelongGroup = groups[3];
            //temp[7].BelongGroup = groups[3];
            //temp[8].BelongGroup = groups[3];

            groups[4].Trains = new List<SYSTrainDto>() { temp[9], temp[10], temp[11] };
            //temp[9].BelongGroup = groups[4];
            //temp[10].BelongGroup = groups[4];
            //temp[11].BelongGroup = groups[4];

            groups[5].Trains = new List<SYSTrainDto>() { temp[12], temp[13], };
            //temp[12].BelongGroup = groups[5];
            //temp[13].BelongGroup = groups[5];
            return temp;
        }
        private static IEnumerable<DMDScheduleTemplateDto> GenerateDMDScheduleTemplates()
        {
            var contents = DMDPreRecordMessages.ToList();
            var targets = SYSStations.ToList();

            var templates = new List<DMDScheduleTemplateDto>()
            {
                new DMDScheduleTemplateDto()
                {
                    Id = Guid.NewGuid(),
                    ScheduleName = "不要飲食",
                    StartDayType = eDayType.當日,
                    StartTime = new DateTime().Add(new TimeSpan(10,30,0)),
                    EndDayType = eDayType.當日,
                    EndTime=  new DateTime().Add(new TimeSpan(15,30,0)),
                    Content = contents[0],
                    Targets = new List<SYSStationDto>(targets.GetRange(0,15))
                },

                new DMDScheduleTemplateDto()
                {
                    Id = Guid.NewGuid(),
                    ScheduleName = "單程票使用",
                    StartDayType = eDayType.當日,
                    StartTime = new DateTime().Add(new TimeSpan(12,30,0)),
                    EndDayType = eDayType.當日,
                    EndTime=  new DateTime().Add(new TimeSpan(20,30,0)),
                    Content = contents[1],
                    Targets = new List<SYSStationDto>(targets.GetRange(16,15))
                },

                new DMDScheduleTemplateDto()
                {
                    Id = Guid.NewGuid(),
                    ScheduleName = "事故後續辦理",
                    StartDayType = eDayType.當日,
                    StartTime = new DateTime().Add(new TimeSpan(23,30,0)),
                    EndDayType = eDayType.次日,
                    EndTime=  new DateTime().Add(new TimeSpan(4,30,0)),
                    Content = contents[2],
                    Targets = new List<SYSStationDto>(targets.GetRange(10,10))
                },
            };
            return templates;
        }
        private static IEnumerable<DMDMessageGroupDto> GenerateDMDMessageGroups()
        {
            var groups = new List<DMDMessageGroupDto>()
            {
                new DMDMessageGroupDto() { Id = Guid.NewGuid(), GroupName = "全部", },
                new DMDMessageGroupDto() { Id = Guid.NewGuid(), GroupName = "政令宣導", },
                new DMDMessageGroupDto() { Id = Guid.NewGuid(), GroupName = "安全宣導", },
                new DMDMessageGroupDto() { Id = Guid.NewGuid(), GroupName = "衛生宣導",  },
                new DMDMessageGroupDto() { Id = Guid.NewGuid(), GroupName = "活動訊息", },
                new DMDMessageGroupDto() { Id = Guid.NewGuid(), GroupName = "測試",    },
            };
            return groups;
        }
        private static IEnumerable<DMDPreRecordMessageDto> GenerateDMDPreRecordMessages()
        {
            var temp = new List<DMDPreRecordMessageDto>()
            {
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="不要飲食",
                    MessageContent ="各位旅客您好，為維護環境清潔，請不要在車站及列車上吃東西、喝飲料、嚼口香糖或檳榔。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="單程票使用",
                    MessageContent ="各位旅客您好，使用單程票時請將車票輕觸感應區後進站，出站時，車票會自動回收，謝謝您的支持與配合。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="事故後續辦理",
                    MessageContent ="各位旅客請注意，因本次事故而離開車站的旅客，您所持用的悠遊卡可繼續使用，不扣除本次旅程費用；持用單程票的旅客，請在一週內向車站辦理退票。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="不要飲食",
                    MessageContent ="各位旅客您好，為維護環境清潔，請不要在車站及列車上吃東西、喝飲料、嚼口香糖或檳榔。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="行動不便改搭電梯",
                    MessageContent ="各位旅客您好，由於電扶梯移動速度較快，年長及行動不便的旅客請儘量改搭電梯，以維護您的安全。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="電扶梯靠右",
                    MessageContent ="各位旅客請注意，搭乘電扶梯時，請靠右站立保持左側淨空並緊握扶手，謝謝您的支持與配合。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="照顧好小孩",
                    MessageContent ="各位旅客您好，為了維持秩序與安全，請照顧好您的小孩，不要讓他們在車站及列車上奔跑，以免發生危險，謝謝您的支持與配合。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="候車線排隊",
                    MessageContent ="您好，候車時，請於白色候車線後方依序排隊，謝謝。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="緊急事故",
                    MessageContent ="各位旅客請注意，各位旅客請注意，由於車站內發生緊急事故，為了您的安全，請迅速離開車站。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="下車再上車",
                    MessageContent ="各位旅客您好，搭車時請禮讓車上旅客下車再依序上車，謝謝您的支持與配合。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="人潮眾多分散候車",
                    MessageContent ="各位旅客您好，因為現在人潮眾多，請各位旅客分散到各車廂門候車，以避免擁擠，謝謝您的支持與配合。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="單程票使用",
                    MessageContent ="各位旅客您好，使用單程票時請將車票輕觸感應區後進站，出站時，車票會自動回收，謝謝您的支持與配合。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="分散候車",
                    MessageContent ="您好，月台候車時，請分散候車位置，以節省您的上下車時間，謝謝。",
                },
                new DMDPreRecordMessageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageName ="因故暫停服務",
                    MessageContent ="各位旅客請注意，本站因故將暫停服務，請遵照服務人員的引導，迅速離開車站，謝謝您的支持與配合。",
                },
            };


            var groups = DMDMessageGroups.ToList();


            groups[0].Messages = new List<DMDPreRecordMessageDto>(temp);

            groups[1].Messages = new List<DMDPreRecordMessageDto>() { temp[0], temp[1], temp[2] };
            temp[0].BelongGroup = groups[1];
            temp[1].BelongGroup = groups[1];
            temp[2].BelongGroup = groups[1];

            groups[2].Messages = new List<DMDPreRecordMessageDto>() { temp[3], temp[4] };
            temp[3].BelongGroup = groups[2];
            temp[4].BelongGroup = groups[2];

            groups[3].Messages = new List<DMDPreRecordMessageDto>() { temp[5], temp[6], temp[7], temp[8] };
            temp[5].BelongGroup = groups[3];
            temp[6].BelongGroup = groups[3];
            temp[7].BelongGroup = groups[3];
            temp[8].BelongGroup = groups[3];

            groups[4].Messages = new List<DMDPreRecordMessageDto>() { temp[9], temp[10], temp[11] };
            temp[9].BelongGroup = groups[4];
            temp[10].BelongGroup = groups[4];
            temp[11].BelongGroup = groups[4];

            groups[5].Messages = new List<DMDPreRecordMessageDto>() { temp[12], temp[13], };
            temp[12].BelongGroup = groups[5];
            temp[13].BelongGroup = groups[5];
            return temp;
        }
        private static IEnumerable<string> GenerateDMDBlockList()
        {
            var temp = new List<string>()
            {
               "超營養雞排",
               "趕羚羊",
            };
            return temp;
        }
        private static IEnumerable<DMDATSMessageDto> GenerateDMDATSMessages()
        {
            var groups = new List<DMDATSMessageDto>()
            {
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_一般車_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_末班車_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不載客_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.雙線雙向_不停靠不載客_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_一般車_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_末班車_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不載客_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_中間站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_中間站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_中間站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_中間站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_中間站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_終點站,Header = "進站倒數超過60秒 間格顯示設定60秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_終點站,Header = "進站倒數介於60秒~20秒 間格顯示設定10秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_終點站,Header = "進站倒數低於20秒 間格顯示設定2秒",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_終點站,Header = "列車進站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.單線雙向_不停靠不載客_終點站,Header = "列車離站中",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.列車已停靠月台_起始站_二輛列車已停靠月台,Header = "先發側顯示文字",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.列車已停靠月台_起始站_二輛列車已停靠月台,Header = "先發側顯示文字",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.列車已停靠月台_起始站_一輛列車已停靠月台,Header = "顯示文字",CDU = "大廳顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務",PDU = "月台顯示器:目前第[月台號碼]月台終點站,列車不靠站且不提供載客服務" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.列車已停靠月台非起始站,Header = "顯示文字",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.月台未開放,Header = "嘗試重停",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.月台未開放,Header = "不嘗試重停",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.列車停靠異常,Header = "顯示文字",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.月台狀態異常,Header = "顯示文字",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車進站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
                new DMDATSMessageDto() { Id = Guid.NewGuid(), ATSType = eATSType.列車狀態異常,Header = "顯示文字",CDU = "大廳顯示器:目前第[月台號碼]月台中間站,列車離站中",PDU = "月台顯示器:目前第[月台號碼]月台中間站,一般列車到站時間:約[倒數時間]後到站" },
            };
            return groups;
        }
        private static IEnumerable<DMDDisplayMode> GenerateDMDDisplayModes()
        {
            var stations = SYSStations.ToList();
            var temp = new List<DMDDisplayMode>();
            int index = 0;
            foreach (var station in stations)
            {
                if (index % 2 == 0)
                {
                    temp.Add(new DMDDisplayMode
                    {
                        Station = station,
                        CDUSetting = new DMDDisplaySetting()
                        {
                            CurrentPosition = eDisplayPosition.固定前端,
                            CurrentColor = "Red",
                            MoveMode = eMoveMode.向左移,
                            MoveSpeed = 5,
                            FontType = eFontType.標楷體
                        },
                        PDUSetting = new DMDDisplaySetting()
                        {
                            CurrentPosition = eDisplayPosition.固定前端,
                            CurrentColor = "Red",
                            MoveMode = eMoveMode.向下移,
                            MoveSpeed = 5,
                            FontType = eFontType.微軟正黑體
                        },
                    });
                }
                else
                {
                    temp.Add(new DMDDisplayMode
                    {
                        Station = station,
                        CDUSetting = new DMDDisplaySetting()
                        {
                            CurrentPosition = eDisplayPosition.隨文字,
                            CurrentColor = "Orange",
                            MoveMode = eMoveMode.向左移,
                            MoveSpeed = 5,
                            FontType = eFontType.標楷體
                        },
                        PDUSetting = new DMDDisplaySetting()
                        {
                            CurrentPosition = eDisplayPosition.固定前端,
                            CurrentColor = "Green",
                            MoveMode = eMoveMode.向左移,
                            MoveSpeed = 5,
                            FontType = eFontType.標楷體
                        },
                    });
                }

                ++index;
            }
            return temp;
        }
        private static IEnumerable<DMDDayScheduleDto> GenerateDMDDaySchedules()
        {
            var templates = DMDScheduleTemplates.ToList();

            var weekDays = new[]
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            var daySchedules = new List<DMDDayScheduleDto>();

            // 將範本依 index % 7 分配到不同的「天」
            for (int i = 0; i < weekDays.Length; i++)
            {
                var day = weekDays[i];

                // 只把 index % 7 == i 的那些範本，當作這一天的內容
                var listForThisDay = templates
                    .Select((tpl, idx) => new { tpl, idx })
                    .Where(x => x.idx % 7 == i)
                    .Select(x => x.tpl)
                    .ToList();

                daySchedules.Add(new DMDDayScheduleDto
                {
                    Day = day,
                    Templates = listForThisDay
                });
            }

            return daySchedules;

        }
        private static DMDATSIntervalDto GenerateDMDATSInterval()
        {
            var intervalDto = new DMDATSIntervalDto
            {
                StationArrival1 = 60,
                StationArrival2 = 20,
                IntervalDisplay1 = 60,
                IntervalDisplay2 = 10,
                IntervalDisplay3 = 2
            };

            return intervalDto;
        }
    }
}
