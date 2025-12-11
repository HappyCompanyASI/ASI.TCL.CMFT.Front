using System;

namespace ASI.TCL.CMFT.WPF.Module.PA.Dtos
{
    public class PAPreRecordVoiceDto : IEquatable<PAPreRecordVoiceDto>
    {
        public string Id { get; set; }
        public string VoiceName { get; set; }
        public string VoiceContent { get; set; }
        public bool IsIncludeCHN { get; set; }
        public bool IsIncludeTWN { get; set; }
        public bool IsIncludeHAKKA { get; set; }
        public bool IsIncludeENG { get; set; }
        public PAVoiceGroupDto BelongGroup { get; set; }

        // 問題發生於 PAScheduleTemplateSettingsView
        // 這是為了解決XAML綁定時物件參考不同的問題 改用Id參考相同即相等
        public override bool Equals(object obj)
        {
            return Equals(obj as PAPreRecordVoiceDto);
        }

        public bool Equals(PAPreRecordVoiceDto other)
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