using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.OTCS.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.OTCS.Datas
{
    internal class DesignTimeDatas
    {
        public static IEnumerable<SYSTrainGroupDto> SYSTrainGroups { get; private set; } = GenerateSYSTrainGroups();
        public static IEnumerable<SYSTrainDto> SYSTrains { get; private set; } = GenerateSYSTrains();

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
    }
}
