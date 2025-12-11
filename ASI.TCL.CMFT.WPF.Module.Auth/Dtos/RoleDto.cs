using System.Collections.Generic;

namespace ASI.TCL.CMFT.WPF.Module.Auth.Dtos
{
    public class RoleDto 
    {
        public string RoleName { get; set; }

        public bool IsPAFunction { get; set; }
        public bool IsDMDFunction { get; set; }
        public bool IsTetraFunction { get; set; }
        public bool IsOTCSFunction { get; set; }
        public bool IsPASetting { get; set; }
        public bool IsDMDSetting { get; set; }
        public bool IsTetraSetting { get; set; }
        public bool IsAlarmSetting { get; set; }
        public bool IsSystemSetting { get; set; }
        public bool IsUserSetting { get; set; }

        public IEnumerable<AccountDto> Accounts { get; set; }
        public string Id { get; set; }
    }
}
