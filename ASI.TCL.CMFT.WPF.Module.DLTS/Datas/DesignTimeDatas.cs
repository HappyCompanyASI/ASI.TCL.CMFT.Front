using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.DLTS.DataTypes;
using ASI.TCL.CMFT.WPF.Module.DLTS.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.DLTS.Datas
{
    internal class DesignTimeDatas
    {
        public static IEnumerable<SYSStationGroupDto> SYSStationGroups { get; private set; } = GenerateSYSStationGroups();
        public static IEnumerable<SYSStation> SYSStations { get; private set; } = GenerateSYSStations();
        public static IEnumerable<DLTPhoneGroupDto> DLTPhoneGroups { get; private set; } = GenerateDLTPhoneGroups();
        public static IEnumerable<DLTPhoneDto> DLTPhones { get; private set; } = GenerateDLTPhones();
        
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
        private static IEnumerable<SYSStation> GenerateSYSStations()
        {
            var temp = new List<SYSStation>()
            {
               //南環段
               new SYSStation() { StationID = eStationID.Y01 },
               new SYSStation() { StationID = eStationID.Y01A},
               new SYSStation() { StationID = eStationID.Y02A},
               new SYSStation() { StationID = eStationID.Y03 },
               new SYSStation() { StationID = eStationID.Y04 },
               new SYSStation() { StationID = eStationID.Y05 },

               //新北環狀線
               new SYSStation() { StationID = eStationID.Y06 },
               new SYSStation() { StationID = eStationID.Y07 },
               new SYSStation() { StationID = eStationID.Y08 },
               new SYSStation() { StationID = eStationID.Y09 },
               new SYSStation() { StationID = eStationID.Y10 },
               new SYSStation() { StationID = eStationID.Y11 },
               new SYSStation() { StationID = eStationID.Y12 },
               new SYSStation() { StationID = eStationID.Y13 },
               new SYSStation() { StationID = eStationID.Y14 },
               new SYSStation() { StationID = eStationID.Y15 },
               new SYSStation() { StationID = eStationID.Y16 },
               new SYSStation() { StationID = eStationID.Y17 },
               new SYSStation() { StationID = eStationID.Y18 },
               new SYSStation() { StationID = eStationID.Y19 },

               //北環段
               new SYSStation() { StationID = eStationID.Y19A},
               new SYSStation() { StationID = eStationID.Y19B},
               new SYSStation() { StationID = eStationID.Y20 },
               new SYSStation() { StationID = eStationID.Y21 },
               new SYSStation() { StationID = eStationID.Y22 },
               new SYSStation() { StationID = eStationID.Y23 },
               new SYSStation() { StationID = eStationID.Y24 },
               new SYSStation() { StationID = eStationID.Y25 },
               new SYSStation() { StationID = eStationID.Y26 },
               new SYSStation() { StationID = eStationID.Y27 },
               new SYSStation() { StationID = eStationID.Y28 },
               new SYSStation() { StationID = eStationID.Y29 },

                //東環段
                new SYSStation() { StationID = eStationID.Y30 },
                new SYSStation() { StationID = eStationID.Y31 },
                new SYSStation() { StationID = eStationID.Y32 },
                new SYSStation() { StationID = eStationID.Y33 },
                new SYSStation() { StationID = eStationID.Y34 },
                new SYSStation() { StationID = eStationID.Y35 },
                new SYSStation() { StationID = eStationID.Y36 },
                new SYSStation() { StationID = eStationID.Y37 },
                new SYSStation() { StationID = eStationID.Y38 },
                new SYSStation() { StationID = eStationID.Y39 },
            };

            var groups = SYSStationGroups.ToList();

            //全部車站
            groups[0].Stations = new List<SYSStation>(temp);
            //一期車站
            groups[1].Stations = new List<SYSStation>() { temp[0], temp[1], temp[2] };
            //二期車站
            groups[2].Stations = new List<SYSStation>() { temp[2], temp[3], temp[4] };
            //三期車站
            groups[3].Stations = new List<SYSStation>() { temp[6], temp[8], temp[10] };
            //單數車站
            groups[4].Stations = new List<SYSStation>() { temp[14], temp[22], temp[23] };
            //偶數車站
            groups[5].Stations = new List<SYSStation>() { temp[25], temp[26], temp[27] };
            //轉運車站
            groups[6].Stations = new List<SYSStation>() { temp[30], temp[31], temp[33] };

            return temp;
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
    }
}
