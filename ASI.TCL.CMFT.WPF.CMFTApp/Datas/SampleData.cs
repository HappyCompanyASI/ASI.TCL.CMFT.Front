using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.Alarm.Dtos;
using ASI.TCL.CMFT.WPF.Module.OTCS.Dtos;
using ASI.TCL.CMFT.WPF.Module.Tetra.Dtos;

namespace ASI.TCL.CMFT.WPF.CMFTApp.Datas
{
    public static class SampleData
    {
        //無線電來電顯示
        public static IEnumerable<TetraCallerDisplayDto> TetraCallerDisplay { get; private set; } = GenerateTetraCallerDisplay();
        //列車
        public static IEnumerable<OTCSTarinDto> OTCSTarins { get; private set; } = GenerateOTCSTarins();
        //司機員通訊來電顯示
        public static IEnumerable<SICallerDisplayDto> SICallerDisplay { get; private set; } = GenerateSICallerDisplay();
        //旅客緊急通訊來電顯示
        public static IEnumerable<PICallerDisplayDto> PICallerDisplay { get; private set; } = GeneratePICallerDisplay();

        private static IEnumerable<TetraCallerDisplayDto> GenerateTetraCallerDisplay()
        {
            var temp = new List<TetraCallerDisplayDto>
            {
               new TetraCallerDisplayDto()
               {
                   Id = Guid.NewGuid().ToString(),
                   PhoneNumber = "50965",
                   BaseStation = "BS1",
                   TetraCallType = eTetraCallType.緊急
               },
               new TetraCallerDisplayDto()
               {
                   Id = Guid.NewGuid().ToString(),
                   PhoneNumber = "51593",
                   BaseStation = "BS5",
                   TetraCallType = eTetraCallType.緊急
               },
               new TetraCallerDisplayDto()
               {
                   Id = Guid.NewGuid().ToString(),
                   PhoneNumber = "54421",
                   BaseStation = "BS2",
                   TetraCallType = eTetraCallType.一般
               },
               new TetraCallerDisplayDto()
               {
                   Id = Guid.NewGuid().ToString(),
                   PhoneNumber = "55161",
                   BaseStation = "BS5",
                   TetraCallType = eTetraCallType.一般
               },
               new TetraCallerDisplayDto()
               {
                   Id = Guid.NewGuid().ToString(),
                   PhoneNumber = "54046",
                   BaseStation = "BS4",
                   TetraCallType = eTetraCallType.一般
               }
            };
            return temp;
        }
        private static IEnumerable<OTCSTarinDto> GenerateOTCSTarins()
        {
            var temp = new List<OTCSTarinDto>()
            {
                new OTCSTarinDto() { TrainID = "V01",  ChannelNumber ="65121", TrainNumber = "956次" },
                new OTCSTarinDto() { TrainID = "V02",  ChannelNumber ="65122", TrainNumber = "921次" },
                new OTCSTarinDto() { TrainID = "V03",  ChannelNumber ="65123", TrainNumber = "877次" },
                new OTCSTarinDto() { TrainID = "V04",  ChannelNumber ="65124", TrainNumber = "231次" },
                new OTCSTarinDto() { TrainID = "V05",  ChannelNumber ="65125", TrainNumber = "452次" },
            };
            return temp;
        }
        private static IEnumerable<SICallerDisplayDto> GenerateSICallerDisplay()
        {
            var trains = OTCSTarins.ToList();
            var temp = new List<SICallerDisplayDto>
            {
               new SICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[2], Car ="A車", BaseStation = "Y6/Y8" },
               new SICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[1], Car ="A車", BaseStation = "Y6/Y8" },
               new SICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[3], Car ="A車", BaseStation = "Y1/Y3" },
               new SICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[4], Car ="A車", BaseStation = "Y9/Y11"},
               new SICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[0], Car ="A車", BaseStation = "Y6/Y8" }
            };
            return temp;
        }
        private static IEnumerable<PICallerDisplayDto> GeneratePICallerDisplay()
        {
            var trains = OTCSTarins.ToList();
            var temp = new List<PICallerDisplayDto>
            {
               new PICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[1], Car ="A車", PINumber="41", Source = "Y6/Y8" },
               new PICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[3], Car ="B車", PINumber="31", Source = "Y6/Y8" },
               new PICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[2], Car ="C車", PINumber="13", Source = "Y1/Y3" },
               new PICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[2], Car ="B車", PINumber="17", Source = "Y9/Y11"},
               new PICallerDisplayDto() { Id = Guid.NewGuid().ToString(), Train = trains[4], Car ="D車", PINumber="28", Source = "Y6/Y8" }
            };
            return temp;
        }

        private static IEnumerable<EquipAlarmDto> GenerateSYSAlarmInfos()
        {
            //SYSAlarmInfos = new List<AlarmInfo>()
            //{
            //    new AlarmInfo(){  Id = Guid.NewGuid().ToString(), ConfirmedTime = DateTime.Now(), Equip = new SYSEquip() }

            return null;
            //}
        }
    }
}