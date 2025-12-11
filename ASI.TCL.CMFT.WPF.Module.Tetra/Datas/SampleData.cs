using System;
using System.Collections.Generic;
using ASI.TCL.CMFT.WPF.Module.Tetra.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.Datas
{
    public static class SampleData
    {
        //無線電來電顯示
        public static IEnumerable<TetraCallerDisplayDto> TetraCallerDisplay { get; private set; } = GenerateTetraCallerDisplay();

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
        
    }
}