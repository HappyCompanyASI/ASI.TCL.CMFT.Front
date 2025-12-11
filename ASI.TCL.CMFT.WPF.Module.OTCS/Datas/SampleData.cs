using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.OTCS.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.OTCS.Datas
{
    public static class SampleData
    {
        //列車
        public static IEnumerable<OTCSTarinDto> OTCSTarins { get; private set; } = GenerateOTCSTarins();
        //司機員通訊來電顯示
        public static IEnumerable<SICallerDisplayDto> SICallerDisplay { get; private set; } = GenerateSICallerDisplay();
        //旅客緊急通訊來電顯示
        public static IEnumerable<PICallerDisplayDto> PICallerDisplay { get; private set; } = GeneratePICallerDisplay();

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

        
    }
}