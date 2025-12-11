using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.SYS.DataTypes;
using ASI.TCL.CMFT.WPF.Module.SYS.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.SYS.Datas
{
    internal class DesignTimeDatas
    {
        public static IEnumerable<eSystemType> SystemTypes { get; private set; } = GenerateSystemTypes();
        public static IEnumerable<RoleDto> UserRoles = GenerateUserRoles();
        public static IEnumerable<AccountDto> UserAccounts = GenerateUserAccounts();
        public static IEnumerable<SYSStationGroupDto> SYSStationGroups { get; private set; } = GenerateSYSStationGroups();
        public static IEnumerable<SYSStationDto> SYSStations { get; private set; } = GenerateSYSStations();
        public static IEnumerable<SYSTrainGroupDto> SYSTrainGroups { get; private set; } = GenerateSYSTrainGroups();
        public static IEnumerable<SYSTrainDto> SYSTrains { get; private set; } = GenerateSYSTrains();
        public static IEnumerable<RadioGroupDto> RadioGroups { get; private set; } = GenerateRadioGroups();
        public static IEnumerable<RadioDto> Radios { get; private set; } = GenerateRadios();
        public static IEnumerable<DLTPhoneGroupDto> DLTPhoneGroups { get; private set; } = GenerateDLTPhoneGroups();
        public static IEnumerable<DLTPhoneDto> DLTPhones { get; private set; } = GenerateDLTPhones();
        public static IEnumerable<SYSConsoleDto> SYSConsoles { get; private set; } = GenerateSYSConsoles();
        public static IEnumerable<SYSCMFTAuthorityDto> SYSCMFTAuthoritys { get; private set; } = GenerateSYSCMFTAuthoritys();
        public static IEnumerable<SYSOTCSAuthorityDto> SYSOTCSAuthoritys { get; private set; } = GenerateSYSOTCSAuthoritys();
        public static IEnumerable<SYSPASAuthority> SYSPASAuthoritys { get; private set; } = GenerateSYSPAAuthoritys();
        public static IEnumerable<SYSOperationLogDto> SYSOperationLogs { get; private set; } = GenerateSYSOperationLogs();
        public static IEnumerable<SYSConsoleStateDto> SYSConsoleStates { get; private set; } = GenerateSYSConsoleStates();
        public static IEnumerable<string> DMDBlockList { get; private set; } = GenerateDMDBlockList();


        private static IEnumerable<eSystemType> GenerateSystemTypes()
        {
            return Enum.GetValues(typeof(eSystemType)).Cast<eSystemType>();
        }
        private static IEnumerable<RoleDto> GenerateUserRoles()
        {
            var temp = new List<RoleDto>
            {
               new RoleDto() { Id = Guid.NewGuid(), RoleName ="系統管理員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid(), RoleName ="主任控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid(), RoleName ="正線控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid(), RoleName ="車站控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid(), RoleName ="電力控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid(), RoleName ="機廠控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid(), RoleName ="維修人員"  , IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
            };
            return temp;
        }
        private static IEnumerable<AccountDto> GenerateUserAccounts()
        {
            var temp = new List<AccountDto>
            {
               new AccountDto() { UserID ="admin0", UserPassword="", UserName="名稱0", Description="admin" },
               new AccountDto() { UserID ="admin1", UserPassword="", UserName="名稱1", Description="Leader" },
               new AccountDto() { UserID ="admin2", UserPassword="", UserName="名稱2", Description="Chief" },
               new AccountDto() { UserID ="admin3", UserPassword="", UserName="名稱3", Description="Staff" },
               new AccountDto() { UserID ="admin4", UserPassword="", UserName="名稱4", Description="Staff" },
               new AccountDto() { UserID ="admin5", UserPassword="", UserName="名稱5", Description="Staff" },
               new AccountDto() { UserID ="admin6", UserPassword="", UserName="名稱6", Description="Staff" },
            };

            var roles = UserRoles.ToList();

            //系統管理員
            temp[0].RoleDto = roles[0];
            roles[0].Accounts = new List<AccountDto>() { temp[0] };

            //主任控制員
            temp[1].RoleDto = roles[1];
            temp[2].RoleDto = roles[1];
            roles[1].Accounts = new List<AccountDto>() { temp[1], temp[2] };

            //正線控制員
            temp[3].RoleDto = roles[2];
            temp[4].RoleDto = roles[2];
            roles[2].Accounts = new List<AccountDto>() { temp[3], temp[4] };

            //車站控制員
            temp[5].RoleDto = roles[4];
            temp[6].RoleDto = roles[3];
            roles[3].Accounts = new List<AccountDto>() { temp[5], temp[6] };
            roles[4].Accounts = new List<AccountDto>();
            roles[5].Accounts = new List<AccountDto>();
            roles[6].Accounts = new List<AccountDto>();

            return temp;
        }
        private static IEnumerable<SYSStationGroupDto> GenerateSYSStationGroups()
        {
            var temp = new List<SYSStationGroupDto>()
            {
               new SYSStationGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "全部車站"},
               new SYSStationGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "一期車站"},
               new SYSStationGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "二期車站"},
               new SYSStationGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "三期車站"},
               new SYSStationGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "單數車站"},
               new SYSStationGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "偶數車站"},
               new SYSStationGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "轉運車站"},
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
        private static IEnumerable<RadioGroupDto> GenerateRadioGroups()
        {
            var temp = new List<RadioGroupDto>()
            {
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "全部無線電" },
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "固-正線" },
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "固-警察" },
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "固-電力" },
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "固-維修" },
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "固-機廠" },
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "主任1"   },
               new RadioGroupDto() { Id = Guid.NewGuid().ToString(), GroupName = "控制員"  },
            };
            return temp;
        }
        private static IEnumerable<RadioDto> GenerateRadios()
        {
            var temp = new List<RadioDto>();

            for (int i = 61001; i < 61011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });
            for (int i = 62001; i < 62011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });
            for (int i = 63001; i < 63011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });
            for (int i = 64001; i < 64011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });
            for (int i = 65001; i < 65011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });
            for (int i = 66001; i < 66011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });
            for (int i = 67001; i < 67011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });
            for (int i = 68001; i < 68011; i++)
                temp.Add(new RadioDto { RadioNumber = i.ToString() });

            var groups = RadioGroups.ToList();

            //全部無線
            groups[0].Radios = new List<RadioDto>(temp);
            //固 - 正線
            groups[1].Radios = new List<RadioDto>(temp.GetRange(9, 10));
            //固 - 警察
            groups[2].Radios = new List<RadioDto>(temp.GetRange(19, 10));
            //固 - 電力
            groups[3].Radios = new List<RadioDto>(temp.GetRange(29, 10));
            //固 - 維修
            groups[4].Radios = new List<RadioDto>(temp.GetRange(39, 10));
            //固 - 機廠
            groups[5].Radios = new List<RadioDto>(temp.GetRange(49, 10));
            //主任1
            groups[6].Radios = new List<RadioDto>(temp.GetRange(59, 10));
            //控制員
            groups[7].Radios = new List<RadioDto>(temp.GetRange(69, 10));

            return temp;
        }
        private static IEnumerable<SYSConsoleDto> GenerateSYSConsoles()
        {
            var temp = new List<SYSConsoleDto>()
            {
               new SYSConsoleDto() { Id = Guid.NewGuid(), SystemID = "Console1", ConsoleName = "通訊多功能操作台1", IPAddress = "10.7.0.51", DLTNumber ="2301", TetraNumber = "51001", SetupLocation = "行控中心", SeatName = "正線控制1" },
               new SYSConsoleDto() { Id = Guid.NewGuid(), SystemID = "Console2", ConsoleName = "通訊多功能操作台2", IPAddress = "10.7.0.52", DLTNumber ="2302", TetraNumber = "51002", SetupLocation = "行控中心", SeatName = "正線控制2" },
               new SYSConsoleDto() { Id = Guid.NewGuid(), SystemID = "Console3", ConsoleName = "通訊多功能操作台3", IPAddress = "10.7.0.53", DLTNumber ="2303", TetraNumber = "51003", SetupLocation = "行控中心", SeatName = "車站/電力控制" },
               new SYSConsoleDto() { Id = Guid.NewGuid(), SystemID = "Console4", ConsoleName = "通訊多功能操作台4", IPAddress = "10.7.0.54", DLTNumber ="2304", TetraNumber = "51004", SetupLocation = "行控中心", SeatName = "南機廠控制" },
               new SYSConsoleDto() { Id = Guid.NewGuid(), SystemID = "Console5", ConsoleName = "通訊多功能操作台5", IPAddress = "10.7.0.55", DLTNumber ="2305", TetraNumber = "51005", SetupLocation = "行控中心", SeatName = "主任控制" },
               new SYSConsoleDto() { Id = Guid.NewGuid(), SystemID = "Console6", ConsoleName = "通訊多功能操作台6", IPAddress = "10.7.0.56", DLTNumber ="2306", TetraNumber = "51006", SetupLocation = "通訊維修室", SeatName = "通訊維修" },
               new SYSConsoleDto() { Id = Guid.NewGuid(), SystemID = "Console7", ConsoleName = "通訊多功能操作台7", IPAddress = "10.7.0.57", DLTNumber ="2307", TetraNumber = "51007", SetupLocation = "測試軌控制室", SeatName = "測試軌控制" },
            };
            return temp;
        }
        private static IEnumerable<SYSCMFTAuthorityDto> GenerateSYSCMFTAuthoritys()
        {
            var list = SYSConsoles.ToList();
            var temp = new List<SYSCMFTAuthorityDto>()
            {
               new SYSCMFTAuthorityDto() { Console = list[0], IsDMDEnable = true, IsPAEnable = true, IsTetraEnable = true, IsOTCSEnable = true, IsSystemAlarmEnable = true, IsLogSearchEnable = true },
               new SYSCMFTAuthorityDto() { Console = list[1], IsDMDEnable = true, IsPAEnable = true, IsTetraEnable = true, IsOTCSEnable = true, IsSystemAlarmEnable = true, IsLogSearchEnable = true },
               new SYSCMFTAuthorityDto() { Console = list[2], IsDMDEnable = true, IsPAEnable = true, IsTetraEnable = true, IsOTCSEnable = true, IsSystemAlarmEnable = true, IsLogSearchEnable = true },
               new SYSCMFTAuthorityDto() { Console = list[3], IsDMDEnable = true, IsPAEnable = true, IsTetraEnable = true, IsOTCSEnable = true, IsSystemAlarmEnable = true, IsLogSearchEnable = true },
               new SYSCMFTAuthorityDto() { Console = list[4], IsDMDEnable = true, IsPAEnable = true, IsTetraEnable = true, IsOTCSEnable = true, IsSystemAlarmEnable = true, IsLogSearchEnable = true },
               new SYSCMFTAuthorityDto() { Console = list[5], IsDMDEnable = false, IsPAEnable = false, IsTetraEnable = true, IsOTCSEnable = false, IsSystemAlarmEnable = true, IsLogSearchEnable = true },
               new SYSCMFTAuthorityDto() { Console = list[6], IsDMDEnable = false, IsPAEnable = false, IsTetraEnable = true, IsOTCSEnable = true, IsSystemAlarmEnable = false, IsLogSearchEnable = true },
            };
            return temp;
        }
        private static IEnumerable<SYSOTCSAuthorityDto> GenerateSYSOTCSAuthoritys()
        {
            var list = SYSConsoles.ToList();
            var temp = new List<SYSOTCSAuthorityDto>();

            var listAuthorities = new List<SYSAuthorityItemDto>();

            //南機廠
            listAuthorities.Add(new SYSAuthorityItemDto { IsChecked = true });
            //車站的量
            foreach (var station in SYSStations)
                listAuthorities.Add(new SYSAuthorityItemDto { IsChecked = true });

            foreach (var console in list)
            {
                // 產生新的 List 並複製每個 AuthorityItem
                var authoritiesCopy = listAuthorities
                    .Select(a => new SYSAuthorityItemDto { IsChecked = a.IsChecked })
                    .ToList();

                temp.Add(new SYSOTCSAuthorityDto { Console = console, Authorities = authoritiesCopy });
            }
            return temp;
        }
        private static IEnumerable<SYSPASAuthority> GenerateSYSPAAuthoritys()
        {
            var list = SYSConsoles.ToList();
            var temp = new List<SYSPASAuthority>()
            {
               new SYSPASAuthority() { Console = list[0], CanPlay = true },
               new SYSPASAuthority() { Console = list[1], CanPlay = true },
               new SYSPASAuthority() { Console = list[2], CanPlay = true },
               new SYSPASAuthority() { Console = list[3], CanPlay = true },
               new SYSPASAuthority() { Console = list[4], CanPlay = true },
               new SYSPASAuthority() { Console = list[5], CanPlay = true },
               new SYSPASAuthority() { Console = list[6], CanPlay = true },
            };
            return temp;
        }
     
       
        private static IEnumerable<SYSOperationLogDto> GenerateSYSOperationLogs()
        {
            var consoles = SYSConsoles.ToList();
            var accounts = UserAccounts.ToList();
            var temp = new List<SYSOperationLogDto>()
            {
                new SYSOperationLogDto() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = accounts[0].UserID , UserName =  accounts[0].UserName ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送即時訊息" },
                new SYSOperationLogDto() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = accounts[0].UserID , UserName =  accounts[0].UserName ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送預路訊息" },
                new SYSOperationLogDto() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = accounts[0].UserID , UserName =  accounts[0].UserName ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送即時訊息" },
                new SYSOperationLogDto() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = accounts[0].UserID , UserName =  accounts[0].UserName ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="修改ATS訊息設定" },
                new SYSOperationLogDto() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = accounts[0].UserID , UserName =  accounts[0].UserName ,Console = consoles[0], SystemType = eSystemType.點矩陣,  Content ="發送預錄訊息" },
                new SYSOperationLogDto() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = accounts[1].UserID , UserName =  accounts[1].UserName ,Console = consoles[1], SystemType = eSystemType.車站廣播,  Content ="發送預錄廣播" },
                new SYSOperationLogDto() { Id = Guid.NewGuid().ToString(), EventTime = DateTime.Today, UserID = accounts[2].UserID , UserName =  accounts[2].UserName ,Console = consoles[2], SystemType = eSystemType.車站廣播,  Content ="發送預錄廣播" },
            };
            return temp;



        }
        private static IEnumerable<SYSConsoleStateDto> GenerateSYSConsoleStates()
        {
            var consols = SYSConsoles.ToList();
            var users = UserAccounts.ToList();
            if (consols.Count > 6 && users.Count > 6)
            {
                var temp = new List<SYSConsoleStateDto>
                {
                    new SYSConsoleStateDto() { Console = consols[0], UserID = users[0].UserID, UserName = users[0].UserName  ,DLTState =""     , TetraState = "", OTCSState ="PI-V01", PAStation = ""    },
                    new SYSConsoleStateDto() { Console = consols[1], UserID = users[1].UserID, UserName = users[1].UserName  ,DLTState ="12344", TetraState = "", OTCSState =""      , PAStation = ""    },
                    new SYSConsoleStateDto() { Console = consols[2], UserID = users[2].UserID, UserName = users[2].UserName  ,DLTState =""     , TetraState = "", OTCSState ="PI-V02", PAStation = ""    },
                    new SYSConsoleStateDto() { Console = consols[3], UserID = users[3].UserID, UserName = users[3].UserName  ,DLTState =""     , TetraState = "", OTCSState =""      , PAStation = "Y06" },
                    new SYSConsoleStateDto() { Console = consols[4], UserID = users[4].UserID, UserName = users[4].UserName  ,DLTState =""     , TetraState = "", OTCSState ="SI-V01", PAStation = ""    },
                    new SYSConsoleStateDto() { Console = consols[5], UserID = users[5].UserID, UserName = users[5].UserName  ,DLTState =""     , TetraState = "", OTCSState =""      , PAStation = "Y06" },
                    new SYSConsoleStateDto() { Console = consols[6], UserID = users[6].UserID, UserName = users[6].UserName  ,DLTState ="23007", TetraState = "", OTCSState =""      , PAStation = ""    },
                };
                return temp;
            }
            return null;
        }
        private static IEnumerable<DLTPhoneGroupDto> GenerateDLTPhoneGroups()
        {
            var temp = new List<DLTPhoneGroupDto>();
            foreach (var station in SYSStations)
            {
                temp.Add(new DLTPhoneGroupDto() { Station = station });
            }
            return temp;
        }
        private static IEnumerable<DLTPhoneDto> GenerateDLTPhones()
        {
            var groups = DLTPhoneGroups.ToList();
            var temp = new List<DLTPhoneDto>()
            {
                new DLTPhoneDto() { PhoneNumber = "7601", BelongGroup = groups[0], PhoneLocation = "站長室" },
                new DLTPhoneDto() { PhoneNumber = "7602", BelongGroup = groups[0], PhoneLocation = "車站詢問處" },
                new DLTPhoneDto() { PhoneNumber = "7603", BelongGroup = groups[0], PhoneLocation = "上行月台終端" },
                new DLTPhoneDto() { PhoneNumber = "7604", BelongGroup = groups[0], PhoneLocation = "下行月台終端" },

                new DLTPhoneDto() { PhoneNumber = "5466", BelongGroup = groups[1], PhoneLocation = "站長室" },
                new DLTPhoneDto() { PhoneNumber = "5467", BelongGroup = groups[1], PhoneLocation = "車站詢問處" },
                new DLTPhoneDto() { PhoneNumber = "5468", BelongGroup = groups[1], PhoneLocation = "上行月台終端" },
                new DLTPhoneDto() { PhoneNumber = "5469", BelongGroup = groups[1], PhoneLocation = "下行月台終端" },
            };
            groups[0].Phones = new List<DLTPhoneDto>() { temp[0], temp[1], temp[2], temp[3] };
            groups[1].Phones = new List<DLTPhoneDto>() { temp[4], temp[5], temp[6], temp[7] };

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
    }
}
