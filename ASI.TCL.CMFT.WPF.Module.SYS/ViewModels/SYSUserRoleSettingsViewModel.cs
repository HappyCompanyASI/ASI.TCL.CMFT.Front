using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ASI.TCL.CMFT.Messages.Role;
using ASI.TCL.CMFT.Messages.User;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.SYS.Dtos;
using ASI.TCL.CMFT.WPF.Web;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSUserRoleSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IApiClient _apiClient;

        #region Constructors
        public SYSUserRoleSettingsViewModel()
        {

        }
        public SYSUserRoleSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator, IApiClient apiClient)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _apiClient = apiClient;

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        private List<RoleDto> _originalRoleDtos;

        public bool IsItemOperation => SelectedRole != null;

        private RoleDto _selectedRole;
        public RoleDto SelectedRole
        {
            get => _selectedRole;
            set
            {
                SetProperty(ref _selectedRole, value);
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        private ObservableCollection<RoleDto> _sysUserRoleDtos;
        public ObservableCollection<RoleDto> SYSUserRoleDtos
        {
            get => _sysUserRoleDtos;
            set => SetProperty(ref _sysUserRoleDtos, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private void ExcuteAddItemCommand()
        {
            var newRole = new RoleDto
            {
                Id = Guid.NewGuid(),
                RoleName = "新角色",
                IsPAFunction = false,
                IsDMDFunction = false,
                IsTetraFunction = false,
                IsOTCSFunction = false,
                IsPASetting = false,
                IsDMDSetting = false,
                IsTetraSetting = false,
                IsAlarmSetting = false,
                IsSystemSetting = false,
                IsUserSetting = false,
                Accounts = new List<AccountDto>()
            };

            SYSUserRoleDtos.Add(newRole);
            SelectedRole = newRole;
            UpdateSnapshot();
        }

        private DelegateCommand _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(ExcuteDeleteItemCommand);
        private void ExcuteDeleteItemCommand()
        {
            var roleToRemove = SelectedRole;

            if (SelectedRole == null)
                return;

            _dialogService.IsDelete($"確定要刪除\"{roleToRemove.RoleName}\"這個角色嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //資料庫
                    
                    // 從角色集合中移除該角色
                    SYSUserRoleDtos.Remove(SelectedRole);

                    // 更新快照
                    UpdateSnapshot();
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var roleToUpdate = SelectedRole;

            //資料庫

            UpdateSnapshot();

            IsEditMode = false;
        }

        private DelegateCommand _editCommand;
        public DelegateCommand EditCommand => _editCommand ??= new DelegateCommand(ExcuteEditCommand);
        private void ExcuteEditCommand()
        {
            IsEditMode = true;
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private void ExcuteCancelCommand()
        {
            if (!HasChanges())
            {
                IsEditMode = false;
                return;
            }

            // 回復角色資料到快照
            if (_originalRoleDtos != null)
            {
                SYSUserRoleDtos = new ObservableCollection<RoleDto>(
                    _originalRoleDtos.Select(r => new RoleDto
                    {
                        Id = r.Id,
                        RoleName = r.RoleName,
                        IsPAFunction = r.IsPAFunction,
                        IsDMDFunction = r.IsDMDFunction,
                        IsTetraFunction = r.IsTetraFunction,
                        IsOTCSFunction = r.IsOTCSFunction,
                        IsPASetting = r.IsPASetting,
                        IsDMDSetting = r.IsDMDSetting,
                        IsTetraSetting = r.IsTetraSetting,
                        IsAlarmSetting = r.IsAlarmSetting,
                        IsSystemSetting = r.IsSystemSetting,
                        IsUserSetting = r.IsUserSetting,
                        Accounts = r.Accounts != null ? new List<AccountDto>(r.Accounts.Select(a => new AccountDto
                        {
                            UserID = a.UserID,
                            UserName = a.UserName,
                            UserPassword = a.UserPassword,
                            Description = a.Description,
                            RoleDto = null // 避免循環參考
                        })) : new List<AccountDto>()
                    })
                );

                SelectedRole = SYSUserRoleDtos.FirstOrDefault();
                RaisePropertyChanged(nameof(SYSUserRoleDtos));
            }

            IsEditMode = false;
        }

        private DelegateCommand _cancelEditOnSelectionChangedCommand;
        public DelegateCommand CancelEditOnSelectionChangedCommand =>
            _cancelEditOnSelectionChangedCommand ??= new DelegateCommand(ExecuteCancelEditOnSelectionChangedCommand);
        private void ExecuteCancelEditOnSelectionChangedCommand()
        {
            if (IsEditMode)
            {
                CancelCommand.Execute();
            }
        }
        #endregion

        #region Private method
        private bool HasChanges()
        {
            if (_originalRoleDtos == null || SYSUserRoleDtos == null)
                return false;
            if (_originalRoleDtos.Count != SYSUserRoleDtos.Count)
                return true;
            foreach (var role in SYSUserRoleDtos)
            {
                var original = _originalRoleDtos.FirstOrDefault(r => r.Id == role.Id);
                if (original == null)
                    return true;
                if (role.RoleName != original.RoleName ||
                    role.IsPAFunction != original.IsPAFunction ||
                    role.IsDMDFunction != original.IsDMDFunction ||
                    role.IsTetraFunction != original.IsTetraFunction ||
                    role.IsOTCSFunction != original.IsOTCSFunction ||
                    role.IsPASetting != original.IsPASetting ||
                    role.IsDMDSetting != original.IsDMDSetting ||
                    role.IsTetraSetting != original.IsTetraSetting ||
                    role.IsAlarmSetting != original.IsAlarmSetting ||
                    role.IsSystemSetting != original.IsSystemSetting ||
                    role.IsUserSetting != original.IsUserSetting)
                    return true;
                // 比較帳號
                if ((role.Accounts?.Count ?? 0) != (original.Accounts?.Count ?? 0))
                    return true;
                if (role.Accounts != null && original.Accounts != null)
                {
                    foreach (var acct in role.Accounts)
                    {
                        var origAcct = original.Accounts.FirstOrDefault(a => a.UserID == acct.UserID);
                        if (origAcct == null ||
                            acct.UserName != origAcct.UserName ||
                            acct.Description != origAcct.Description)
                            return true;
                    }
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalRoleDtos = SYSUserRoleDtos
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                    IsPAFunction = r.IsPAFunction,
                    IsDMDFunction = r.IsDMDFunction,
                    IsTetraFunction = r.IsTetraFunction,
                    IsOTCSFunction = r.IsOTCSFunction,

                    IsPASetting = r.IsPASetting,
                    IsDMDSetting = r.IsDMDSetting,
                    IsTetraSetting = r.IsTetraSetting,
                    IsAlarmSetting = r.IsAlarmSetting,
                    IsSystemSetting = r.IsSystemSetting,
                    IsUserSetting = r.IsUserSetting,

                    Accounts = r.Accounts
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            // 取得所有角色和使用者資料，並排除超級管理員角色和帳號
            var superAdminRoleId = new Guid("00000000-0000-0000-0000-111111111111");

            //var roles = await _requestHandler.HandleQuery(() =>
            //     _queryService.Query(new QueryModels.GetAllRoles()));
            //var roleList = roles.Where(r => r.Id != superAdminRoleId).ToList();

            //var users = await _requestHandler.HandleQuery(() =>
            //    _queryService.Query(new QueryModels.GetAllUsers()));
            //var userList = users.Where(r => r.BelongRoleId != superAdminRoleId).ToList();

            var roles = await _apiClient.GetAsync<List<Messages.Role.ReadModels.Role>>("sys/role");
            var roleList = roles.Where(r => r.Id != superAdminRoleId).ToList();

            var users = await _apiClient.GetAsync<List<Messages.User.ReadModels.User>>("sys/user");
            var userList = users.Where(r => r.RoleId != superAdminRoleId).ToList();



            // 先把每個 AuthorityList 切割成 HashSet<string>
            var roleDtoDict = roleList
                .Select(r =>
                {
                    var flags = (r.Authorities ?? Enumerable.Empty<string>())
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    // 再映射到 RoleDto
                    return new RoleDto
                    {
                        Id = r.Id,
                        RoleName = r.Name,
                        IsPAFunction = flags.Contains("PAFunc"),
                        IsDMDFunction = flags.Contains("DMDFunc"),
                        IsTetraFunction = flags.Contains("TetraFunc"),
                        IsOTCSFunction = flags.Contains("OTCSFunc"),

                        IsPASetting = flags.Contains("PASetting"),
                        IsDMDSetting = flags.Contains("DMDSetting"),
                        IsTetraSetting = flags.Contains("TetraSetting"),
                        IsAlarmSetting = flags.Contains("AlarmSetting"),
                        IsSystemSetting = flags.Contains("SystemSetting"),
                        IsUserSetting = flags.Contains("UserSetting"),

                        Accounts = new List<AccountDto>()
                    };
                })
                .ToDictionary(dto => dto.Id, dto => dto);

            // 再把 User 映成 AccountDto，並加回對應的 RoleDto.Accounts
            var accountDtos = userList
                .Select(u =>
                {
                    roleDtoDict.TryGetValue(u.RoleId, out var role);

                    var acct = new AccountDto
                    {
                        UserID = u.Id.ToString(),
                        UserName = u.Name,
                        Description = String.Empty,
                        RoleDto = role
                    };

                    // 如果有對應的角色，就把帳號加進去
                    role?.Accounts.Add(acct);
                    return acct;
                })
                .ToList();

            // 最後把角色 DTO 丟到 ObservableCollection 給 UI 綁定
            SYSUserRoleDtos = new ObservableCollection<RoleDto>(roleDtoDict.Values);

            // 主動指定選擇第一筆群組
            SelectedRole = SYSUserRoleDtos.FirstOrDefault();

            RaisePropertyChanged(nameof(SYSUserRoleDtos));

            UpdateSnapshot();
        }
        #endregion
    }
}
