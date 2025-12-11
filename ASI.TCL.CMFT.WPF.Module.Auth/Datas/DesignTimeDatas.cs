using System;
using System.Collections.Generic;
using System.Linq;
using ASI.TCL.CMFT.WPF.Module.Auth.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.Auth.Datas
{
    internal class DesignTimeDatas
    {
        public static IEnumerable<RoleDto> UserRoles = GenerateUserRoles();
        public static IEnumerable<AccountDto> UserAccounts = GenerateUserAccounts();
        private static IEnumerable<RoleDto> GenerateUserRoles()
        {
            var temp = new List<RoleDto>
            {
               new RoleDto() { Id = Guid.NewGuid().ToString(), RoleName ="系統管理員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid().ToString(), RoleName ="主任控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid().ToString(), RoleName ="正線控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid().ToString(), RoleName ="車站控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid().ToString(), RoleName ="電力控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid().ToString(), RoleName ="機廠控制員",   IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
               new RoleDto() { Id = Guid.NewGuid().ToString(), RoleName ="維修人員"  , IsPAFunction = true, IsDMDFunction = true, IsTetraFunction = true, IsOTCSFunction = true, IsPASetting = true, IsDMDSetting = true, IsTetraSetting = true,  IsAlarmSetting = true, IsSystemSetting = true, IsUserSetting = true, },
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
    }
}
