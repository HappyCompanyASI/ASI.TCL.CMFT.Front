using System;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Dtos
{
    public class DMDPreRecordMessageDto : IEquatable<DMDPreRecordMessageDto>
    {
        public string Id { get; set; }
        public string MessageName { get; set; }
        public string MessageContent { get; set; }
        public DMDMessageGroupDto BelongGroup { get; set; }
        public bool IsUseDU { get; set; }

        // 問題發生於 DMDScheduleTemplateSettingsView
        // 這是為了解決XAML綁定時物件參考不同的問題 改用Id參考相同即相等
        public override bool Equals(object obj)
        {
            return Equals(obj as DMDPreRecordMessageDto);
        }

        public bool Equals(DMDPreRecordMessageDto other)
        {
            return other is not null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
