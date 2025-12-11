using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.PA.DataTypes;
using ASI.TCL.CMFT.WPF.Module.PA.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.PA.Datas
{
    internal class DesignTimeDatas
    {
        public static IEnumerable<eDayType> DayTypes { get; private set; } = GenerateDayTypes();
        public static IEnumerable<eDayOfWeek> DayOfWeeks { get; private set; } = GenerateDayOfWeeks();
        public static IEnumerable<eDaypartType> DaypartTypes { get; private set; } = GenerateDaypartTypes();

        public static IEnumerable<SYSStationGroupDto> SYSStationGroups { get; private set; } = GenerateSYSStationGroups();
        public static IEnumerable<SYSStationDto> SYSStations { get; private set; } = GenerateSYSStations();

        public static IEnumerable<SYSTrainGroupDto> SYSTrainGroups { get; private set; } = GenerateSYSTrainGroups();
        public static IEnumerable<SYSTrainDto> SYSTrains { get; private set; } = GenerateSYSTrains();

        public static IEnumerable<PAVoiceGroupDto> PAVoiceGroups { get; private set; } = GeneratePAPreRecordGroups();
        public static IEnumerable<PAPreRecordVoiceDto> PAPreRecordVoices { get; private set; } = GeneratePAPreRecordVoices();
        public static IEnumerable<PAPreRecordVoiceDto> PACurrentTrainPlaying { get; private set; } = GeneratePACurrentTrainPlaying();
        public static IEnumerable<PATimeSlotDto> PADaypartTimes { get; private set; } = GeneratePADaypartTimes(0);
        public static IList<PADailyBroadcastScheduleDto> PADayOfWeeks { get; private set; } = GeneratePADayOfWeeks(0, 1, 2, 3, 4, 5, 6);
        public static IEnumerable<PAStationBroadcastScheduleDto> PAStationBroadcastSchedules { get; private set; } = GeneratePAStationBroadcastSchedule();
        public static IEnumerable<PAScheduleTemplateDto> PAScheduleTemplates { get; private set; } = GeneratePAScheduleTemplates();
        public static IEnumerable<PADayScheduleDto> PADaySchedules { get; private set; } = GeneratePADaySchedules();

        public static IEnumerable<PABroadcastEquipmentDto> PABroadcastEquipments { get; private set; } = GeneratePABroadcastEquipments();

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
               new SYSStationDto() { StationID = eStationID.Y01 },
               new SYSStationDto() { StationID = eStationID.Y01A},
               new SYSStationDto() { StationID = eStationID.Y02A},
               new SYSStationDto() { StationID = eStationID.Y03 },
               new SYSStationDto() { StationID = eStationID.Y04 },
               new SYSStationDto() { StationID = eStationID.Y05 },

               //新北環狀線
               new SYSStationDto() { StationID = eStationID.Y06 },
               new SYSStationDto() { StationID = eStationID.Y07 },
               new SYSStationDto() { StationID = eStationID.Y08 },
               new SYSStationDto() { StationID = eStationID.Y09 },
               new SYSStationDto() { StationID = eStationID.Y10 },
               new SYSStationDto() { StationID = eStationID.Y11 },
               new SYSStationDto() { StationID = eStationID.Y12 },
               new SYSStationDto() { StationID = eStationID.Y13 },
               new SYSStationDto() { StationID = eStationID.Y14 },
               new SYSStationDto() { StationID = eStationID.Y15 },
               new SYSStationDto() { StationID = eStationID.Y16 },
               new SYSStationDto() { StationID = eStationID.Y17 },
               new SYSStationDto() { StationID = eStationID.Y18 },
               new SYSStationDto() { StationID = eStationID.Y19 },

               //北環段
               new SYSStationDto() { StationID = eStationID.Y19A},
               new SYSStationDto() { StationID = eStationID.Y19B},
               new SYSStationDto() { StationID = eStationID.Y20 },
               new SYSStationDto() { StationID = eStationID.Y21 },
               new SYSStationDto() { StationID = eStationID.Y22 },
               new SYSStationDto() { StationID = eStationID.Y23 },
               new SYSStationDto() { StationID = eStationID.Y24 },
               new SYSStationDto() { StationID = eStationID.Y25 },
               new SYSStationDto() { StationID = eStationID.Y26 },
               new SYSStationDto() { StationID = eStationID.Y27 },
               new SYSStationDto() { StationID = eStationID.Y28 },
               new SYSStationDto() { StationID = eStationID.Y29 },

                //東環段
                new SYSStationDto() { StationID = eStationID.Y30 },
                new SYSStationDto() { StationID = eStationID.Y31 },
                new SYSStationDto() { StationID = eStationID.Y32 },
                new SYSStationDto() { StationID = eStationID.Y33 },
                new SYSStationDto() { StationID = eStationID.Y34 },
                new SYSStationDto() { StationID = eStationID.Y35 },
                new SYSStationDto() { StationID = eStationID.Y36 },
                new SYSStationDto() { StationID = eStationID.Y37 },
                new SYSStationDto() { StationID = eStationID.Y38 },
                new SYSStationDto() { StationID = eStationID.Y39 },
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
               new SYSTrainGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "全部列車"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "一期列車"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "上行群組"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "下行群組"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "營運列車"  },
               new SYSTrainGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "非營運列車"},
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
        private static IEnumerable<PAVoiceGroupDto> GeneratePAPreRecordGroups()
        {
            var groups = new List<PAVoiceGroupDto>()
            {
                new PAVoiceGroupDto() { Id = Guid.NewGuid(), GroupName = "全部", },
                new PAVoiceGroupDto() { Id = Guid.NewGuid(), GroupName = "政令宣導", },
                new PAVoiceGroupDto() { Id = Guid.NewGuid(), GroupName = "安全宣導", },
                new PAVoiceGroupDto() { Id = Guid.NewGuid(), GroupName = "衛生宣導",  },
                new PAVoiceGroupDto() { Id = Guid.NewGuid(), GroupName = "活動訊息", },
                new PAVoiceGroupDto() { Id = Guid.NewGuid(), GroupName = "測試",    },
            };
            return groups;
        }
        private static IEnumerable<PAPreRecordVoiceDto> GeneratePAPreRecordVoices()
        {
            var temp = new List<PAPreRecordVoiceDto>()
            {
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="不要飲食",
                    VoiceContent ="各位旅客您好，為維護環境清潔，請不要在車站及列車上吃東西、喝飲料、嚼口香糖或檳榔。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="單程票使用",
                    VoiceContent ="各位旅客您好，使用單程票時請將車票輕觸感應區後進站，出站時，車票會自動回收，謝謝您的支持與配合。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = false,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="事故後續辦理",
                    VoiceContent ="各位旅客請注意，因本次事故而離開車站的旅客，您所持用的悠遊卡可繼續使用，不扣除本次旅程費用；持用單程票的旅客，請在一週內向車站辦理退票。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = false,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="不要飲食",
                    VoiceContent ="各位旅客您好，為維護環境清潔，請不要在車站及列車上吃東西、喝飲料、嚼口香糖或檳榔。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="行動不便改搭電梯",
                    VoiceContent ="各位旅客您好，由於電扶梯移動速度較快，年長及行動不便的旅客請儘量改搭電梯，以維護您的安全。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = false,  IsIncludeHAKKA = false
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="電扶梯靠右",
                    VoiceContent ="各位旅客請注意，搭乘電扶梯時，請靠右站立保持左側淨空並緊握扶手，謝謝您的支持與配合。",
                    IsIncludeCHN = false, IsIncludeENG = true, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="照顧好小孩",
                    VoiceContent ="各位旅客您好，為了維持秩序與安全，請照顧好您的小孩，不要讓他們在車站及列車上奔跑，以免發生危險，謝謝您的支持與配合。",
                    IsIncludeCHN = true, IsIncludeENG = false, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="候車線排隊",
                    VoiceContent ="您好，候車時，請於白色候車線後方依序排隊，謝謝。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = false,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="緊急事故",
                    VoiceContent ="各位旅客請注意，各位旅客請注意，由於車站內發生緊急事故，為了您的安全，請迅速離開車站。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = false,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="下車再上車",
                    VoiceContent ="各位旅客您好，搭車時請禮讓車上旅客下車再依序上車，謝謝您的支持與配合。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="人潮眾多分散候車",
                    VoiceContent ="各位旅客您好，因為現在人潮眾多，請各位旅客分散到各車廂門候車，以避免擁擠，謝謝您的支持與配合。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="單程票使用",
                    VoiceContent ="各位旅客您好，使用單程票時請將車票輕觸感應區後進站，出站時，車票會自動回收，謝謝您的支持與配合。",
                    IsIncludeCHN = true, IsIncludeENG = false, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="分散候車",
                    VoiceContent ="您好，月台候車時，請分散候車位置，以節省您的上下車時間，謝謝。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
                new PAPreRecordVoiceDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    VoiceName ="因故暫停服務",
                    VoiceContent ="各位旅客請注意，本站因故將暫停服務，請遵照服務人員的引導，迅速離開車站，謝謝您的支持與配合。",
                    IsIncludeCHN = true, IsIncludeENG = true, IsIncludeTWN = true,  IsIncludeHAKKA = true
                },
            };


            var groups = PAVoiceGroups.ToList();


            groups[0].Voices = new List<PAPreRecordVoiceDto>(temp);

            groups[1].Voices = new List<PAPreRecordVoiceDto>() { temp[0], temp[1], temp[2] };
            temp[0].BelongGroup = groups[1];
            temp[1].BelongGroup = groups[1];
            temp[2].BelongGroup = groups[1];

            groups[2].Voices = new List<PAPreRecordVoiceDto>() { temp[3], temp[4] };
            temp[3].BelongGroup = groups[2];
            temp[4].BelongGroup = groups[2];

            groups[3].Voices = new List<PAPreRecordVoiceDto>() { temp[5], temp[6], temp[7], temp[8] };
            temp[5].BelongGroup = groups[3];
            temp[6].BelongGroup = groups[3];
            temp[7].BelongGroup = groups[3];
            temp[8].BelongGroup = groups[3];

            groups[4].Voices = new List<PAPreRecordVoiceDto>() { temp[9], temp[10], temp[11] };
            temp[9].BelongGroup = groups[4];
            temp[10].BelongGroup = groups[4];
            temp[11].BelongGroup = groups[4];

            groups[5].Voices = new List<PAPreRecordVoiceDto>() { temp[12], temp[13], };
            temp[12].BelongGroup = groups[5];
            temp[13].BelongGroup = groups[5];
            return temp;
        }
        private static IEnumerable<PAPreRecordVoiceDto> GeneratePACurrentTrainPlaying()
        {
            var temp0 = PAPreRecordVoices.ToList();
            var temp = new List<PAPreRecordVoiceDto> { temp0[0], temp0[2], temp0[3], temp0[8], temp0[10] };
            return temp;
        }
        private static IEnumerable<PATimeSlotDto> GeneratePADaypartTimes(int index)
        {
            switch (index)
            {
                case 0:
                    return new List<PATimeSlotDto>()
                    {
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.尖峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(06, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(08, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.離峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(10, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(11, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.尖峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(12, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(13, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.尖峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(18, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(19, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.夜間時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(23, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(24, 00, 0))},
                    };

                case 1:
                    return new List<PATimeSlotDto>()
                    {
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.離峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(05, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(06, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.尖峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(06, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(07, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.離峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(10, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(11, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.尖峰時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(16, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(17, 00, 0))},
                        new PATimeSlotDto(){ Id = Guid.NewGuid(), DayPartType = eDaypartType.夜間時段, StartDayType = eDayType.當日, StartTime = new DateTime().Add(new TimeSpan(22, 00, 0)), EndDayType = eDayType.當日, EndTime = new DateTime().Add(new TimeSpan(23, 00, 0))},
                    };
                default: break;
            }
            return null;
        }
        private static IList<PADailyBroadcastScheduleDto> GeneratePADayOfWeeks(int index0, int index1, int index2, int index3, int index4, int index5, int index6)
        {
            var tempList = new List<List<PATimeSlotDto>>(){
                GeneratePADaypartTimes(0).ToList(),
                GeneratePADaypartTimes(1).ToList(),
                GeneratePADaypartTimes(0).ToList(),
                GeneratePADaypartTimes(1).ToList(),
                GeneratePADaypartTimes(0).ToList(),
                GeneratePADaypartTimes(1).ToList(),
                GeneratePADaypartTimes(0).ToList(),
            }.ToList();
            return new List<PADailyBroadcastScheduleDto>()
            {
                new PADailyBroadcastScheduleDto(){ Day = DayOfWeek.Monday, DaypartTimes = tempList[index0] },
                new PADailyBroadcastScheduleDto(){ Day = DayOfWeek.Tuesday, DaypartTimes = tempList[index1] },
                new PADailyBroadcastScheduleDto(){ Day = DayOfWeek.Wednesday, DaypartTimes = tempList[index2] },
                new PADailyBroadcastScheduleDto(){ Day = DayOfWeek.Thursday, DaypartTimes = tempList[index3] },
                new PADailyBroadcastScheduleDto(){ Day = DayOfWeek.Friday, DaypartTimes = tempList[index4] },
                new PADailyBroadcastScheduleDto(){ Day = DayOfWeek.Saturday, DaypartTimes = tempList[index5] },
                new PADailyBroadcastScheduleDto(){ Day = DayOfWeek.Sunday, DaypartTimes = tempList[index6] },
            };
        }
        private static IEnumerable<PAStationBroadcastScheduleDto> GeneratePAStationBroadcastSchedule()
        {
            var dayOfWeeks = GeneratePADayOfWeeks(0, 1, 2, 3, 4, 5, 6);

            var dayPartVolume = new List<PAStationBroadcastScheduleDto>();
            foreach (var station in SYSStations)
            {
                dayPartVolume.Add(new PAStationBroadcastScheduleDto() { Station = station, DailySchedules = dayOfWeeks });
            }
            return dayPartVolume;
        }
        private static IEnumerable<PAScheduleTemplateDto> GeneratePAScheduleTemplates()
        {
            var contents = PAPreRecordVoices.ToList();
            var targets = SYSStations.ToList();

            var templates = new List<PAScheduleTemplateDto>()
            {
                new PAScheduleTemplateDto()
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

                new PAScheduleTemplateDto()
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

                new PAScheduleTemplateDto()
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
        private static IEnumerable<PADayScheduleDto> GeneratePADaySchedules()
        {
            var templates = PAScheduleTemplates.ToList();

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

            var daySchedules = new List<PADayScheduleDto>();

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

                daySchedules.Add(new PADayScheduleDto
                {
                    Day = day,
                    Templates = listForThisDay
                });
            }

            return daySchedules;
        }
        private static IEnumerable<PABroadcastEquipmentDto> GeneratePABroadcastEquipments()
        {
            return new List<PABroadcastEquipmentDto>
            {
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台A", Location = "車站A-1F", SeatName = "A1", IsStationBroadcastEnabled = true },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台B", Location = "車站B-2F", SeatName = "B2", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台C", Location = "車站C-1F", SeatName = "C1", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台D", Location = "車站D-B1", SeatName = "D1", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台E", Location = "車站E-2F", SeatName = "E2", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台F", Location = "車站F-1F", SeatName = "F1", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台G", Location = "車站G-3F", SeatName = "G3", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台H", Location = "車站H-1F", SeatName = "H1", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台I", Location = "車站I-B2", SeatName = "I2", IsStationBroadcastEnabled = false },
                new PABroadcastEquipmentDto { Id = Guid.NewGuid(), ConsoleName = "主控台J", Location = "車站J-2F", SeatName = "J2", IsStationBroadcastEnabled = false },
            };
        }

    }
}
