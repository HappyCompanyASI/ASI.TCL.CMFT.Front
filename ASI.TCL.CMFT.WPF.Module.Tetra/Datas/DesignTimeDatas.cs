using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.Tetra.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.Datas
{
    public class DesignTimeDatas
    {
        public static IEnumerable<RadioGroupDto> RadioGroups { get; private set; } = GenerateRadioGroups();
        public static IEnumerable<RadioDto> Radios { get; private set; } = GenerateRadios();
        public static IEnumerable<RadioContentDto> RadioContent { get; private set; } = GenerateRadioContent();
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
        private static IEnumerable<RadioContentDto> GenerateRadioContent()
        {
            var temp = new List<RadioContentDto>()
            {
                new RadioContentDto()
                {
                    ChannelName = "個別呼叫",
                    ChannelNumber = "61021",
                     Contents = new List<RadioContentItemDto>()
                    {
                        new RadioContentItemDto() { NumberOrTrain = "61133", Detail= ""}
                    }
                },

                new RadioContentDto()
                {
                    ChannelName = "測試群組",
                    ChannelNumber = "65002",
                    Contents = new List<RadioContentItemDto>()
                    {
                        new RadioContentItemDto() { NumberOrTrain = "65101", Detail= "通話中"}
                    }
                },

                new RadioContentDto()
                {
                    ChannelName = "列車V05",
                    ChannelNumber = "69005",
                    Contents = new List<RadioContentItemDto>()
                    {
                        new RadioContentItemDto() { NumberOrTrain = "V05", Detail= "C車,9號PI" },
                        new RadioContentItemDto() { NumberOrTrain = "V05", Detail= "C車,7號PI" },
                        new RadioContentItemDto() { NumberOrTrain = "V05", Detail= "A車,3號PI" }
                    }
                },

                new RadioContentDto()
                {
                    ChannelName = "收音V02",
                    ChannelNumber = "65002",
                    Contents = new List<RadioContentItemDto>()
                    {
                        new RadioContentItemDto() { NumberOrTrain = "V02", Detail= "A車,2號PI"}
                    }
                }
            };
            return temp;
        }
    }
}
