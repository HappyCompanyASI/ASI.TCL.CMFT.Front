using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.SYS.Dtos;
using ASI.TCL.CMFT.WPF.Module.SYS.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSUserAccountSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => true;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSUserAccountSettingsViewModel()
        {
        }
        public SYSUserAccountSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                SetProperty(ref _isEditMode, value);
                RaisePropertyChanged(nameof(CanItemAddDelete));
                RaisePropertyChanged(nameof(CanAddItem));
            }
        }

        private List<AccountDto> _originalAccountDtos;

        public bool IsItemOperation => SelectedAccount != null;
        public bool CanItemAddDelete => SelectedAccount != null && !IsEditMode;
        public bool CanAddItem => SelectedRole != null && !IsEditMode;

        private AccountDto _selectedAccount;
        public AccountDto SelectedAccount
        {
            get => _selectedAccount;
            set  
            { 
                SetProperty(ref _selectedAccount, value);
                RaisePropertyChanged(nameof(IsItemOperation));
                RaisePropertyChanged(nameof(CanItemAddDelete));
            }
        }

        private RoleDto _selectedRole;
        public RoleDto SelectedRole
        {
            get => _selectedRole;
            set
            {
                SetProperty(ref _selectedRole, value);
                RaisePropertyChanged(nameof(CanAddItem));
            }
        }

        // ListBox 顯示群組
        private ObservableCollection<RoleDto> _sysUserRoleDtos;
        public ObservableCollection<RoleDto> SYSUserRoleDtos
        {
            get => _sysUserRoleDtos;
            set => SetProperty(ref _sysUserRoleDtos, value);
        }
        // ComboBox 選項來源
        public ObservableCollection<RoleDto> EditableGroups =>
            new ObservableCollection<RoleDto>(SYSUserRoleDtos);
        // ListView 顯示訊息
        private ObservableCollection<AccountDto> _sysUserAccountDtos;
        public ObservableCollection<AccountDto> SYSUserAccountDtos
        {
            get => _sysUserAccountDtos;
            set => SetProperty(ref _sysUserAccountDtos, value);
        }
        // 用來做篩選後的訊息顯示
        public ICollectionView SYSUserAccountDtosView { get; private set; }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private void ExcuteAddItemCommand()
        {
            _dialogService.ShowDialogHost(nameof(SYSUserAccountSettingsDialogView), null, async result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey("AccountID"))
                {
                    string newAccountID = result.Parameters.GetValue<string>("AccountID");

                    //畫面
                    var newAccount = new AccountDto
                    {
                        UserID = newAccountID,
                        UserName = "新使用者",
                        UserPassword = "",
                        Description = "新增敘述",
                        RoleDto = SelectedRole
                    };

                    ////資料庫
                    //await _requestHandler.HandleCommand(new Commands.CreateUser
                    //{
                    //    Id = newAccountID,
                    //    Name = "新使用者",
                    //    Description = "新增敘述",
                    //    BelongRoleId = SelectedRole.Id
                    //}, _sysApplicationService.Handle);

                    SYSUserAccountDtos.Add(newAccount);
                    UpdateSnapshot();
                    RaisePropertyChanged(nameof(EditableGroups));
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(ExcuteDeleteItemCommand);
        private void ExcuteDeleteItemCommand()
        {
            var itemToDelete = SelectedAccount;

            if (itemToDelete == null)
                return;

            _dialogService.IsDelete($"確定要刪除\"{itemToDelete.UserID}\"這筆帳號嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //await _requestHandler.HandleCommand(new Commands.DeleteUser { Id = itemToDelete.UserID }, _sysApplicationService.Handle);

                    SelectedAccount = null; // 清除選取

                    SYSUserAccountDtos.Remove(itemToDelete);

                    SYSUserAccountDtosView.Refresh(); // 刷新篩選

                    UpdateSnapshot();
                }
            }, "MainDialogHost");

        }

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private async void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedAccount;

            //await _requestHandler.HandleCommand(new Commands.UpdateUserDetails()
            //{
            //    Id = itemToUpdate.UserID,
            //    NewName = itemToUpdate.UserName,
            //    NewDescription = itemToUpdate.Description,
            //    BelongRoleId = itemToUpdate.RoleDto.Id
            //}, _sysApplicationService.Handle);

            // 更新快照
            UpdateSnapshot();
            SYSUserAccountDtosView.Refresh();

            IsEditMode = false;
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private void ExcuteCancelCommand()
        {
            // 判斷是否資料有變更
            if (!HasChanges())
            {
                IsEditMode = false;
                return;
            }

            // 還原帳號資料
            SYSUserAccountDtos = new ObservableCollection<AccountDto>(_originalAccountDtos
                .Select(a => new AccountDto
                {
                    UserID = a.UserID,
                    UserName = a.UserName,
                    UserPassword = a.UserPassword,
                    Description = a.Description,
                    RoleDto = SYSUserRoleDtos.FirstOrDefault(r => r.Id == a.RoleDto?.Id)
                })
                .ToList()
            );
            RaisePropertyChanged(nameof(SYSUserAccountDtos));

            // 重新設定 CollectionView
            SYSUserAccountDtosView = CollectionViewSource.GetDefaultView(SYSUserAccountDtos);
            SYSUserAccountDtosView.Filter = CollectionViewFilter;
            RaisePropertyChanged(nameof(SYSUserAccountDtosView));

            IsEditMode = false;
        }

        private DelegateCommand _editCommand;
        public DelegateCommand EditCommand => _editCommand ??= new DelegateCommand(ExcuteEditCommand);
        private void ExcuteEditCommand()
        {
            IsEditMode = true;
        }

        // 以下是跟畫面控制相關的Command

        // 編輯模式時選項變更或離開時視作取消編輯
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

        private DelegateCommand _groupSelectionChangedCommand;
        public DelegateCommand GroupSelectionChangedCommand => _groupSelectionChangedCommand ??= new DelegateCommand(ExecuteGroupSelectionChangedCommand);
        private void ExecuteGroupSelectionChangedCommand()
        {
            SYSUserAccountDtosView?.Refresh();
        }

        private DelegateCommand _clearSelectedMessageCommand;
        public DelegateCommand ClearSelectedMessageCommand => _clearSelectedMessageCommand ??= new DelegateCommand(ExecuteClearSelectedMessageCommand);
        private void ExecuteClearSelectedMessageCommand()
        {
            SelectedAccount = null;
        }
        #endregion

        #region Private method
        private bool CollectionViewFilter(object item)
        {
            if (item is AccountDto account)
            {
                if (SelectedRole == null)
                    return true;

                return account.RoleDto?.Id == SelectedRole.Id;
            }
            return false;
        }
        private bool HasChanges()
        {
            if (_originalAccountDtos.Count != SYSUserAccountDtos.Count)
                return true;

            foreach (var current in SYSUserAccountDtos)
            {
                var original = _originalAccountDtos.FirstOrDefault(a => a.UserID == current.UserID);
                if (original == null)
                    return true;

                if (original.UserName != current.UserName ||
                    original.Description != current.Description ||
                    original.RoleDto?.Id != current.RoleDto?.Id)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalAccountDtos = SYSUserAccountDtos
                .Select(a => new AccountDto
                {
                    UserID = a.UserID,
                    UserName = a.UserName,
                    UserPassword = a.UserPassword,
                    Description = a.Description,
                    RoleDto = a.RoleDto
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            //// 取得所有角色和使用者資料，並排除超級管理員角色和帳號
            //var superAdminRoleId = new Guid("00000000-0000-0000-0000-111111111111");

            //var roles = await _requestHandler.HandleQuery(() =>
            //    _queryService.Query(new QueryModels.GetAllRoles()));
            //var roleList = roles.Where(r => r.Id != superAdminRoleId).ToList();

            //var users = await _requestHandler.HandleQuery(() =>
            //    _queryService.Query(new QueryModels.GetAllUsers()));
            //var userList = users.Where(r => r.BelongRoleId != superAdminRoleId).ToList();

            //// 先把每個 AuthorityList 切割成 HashSet<string>
            //var roleDtoDict = roleList
            //    .Select(r =>
            //    {
            //        // 切成 ["PAFunc","PASetting",…]，去除空白
            //        var flags = (r.AuthorityList ?? "")
            //            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //            .Select(s => s.Trim())
            //            .ToHashSet(StringComparer.OrdinalIgnoreCase);

            //        // 再映射到 RoleDto
            //        return new RoleDto
            //        {
            //            Id = r.Id,
            //            RoleName = r.Name,
            //            IsPAFunction = flags.Contains("PAFunc"),
            //            IsDMDFunction = flags.Contains("DMDFunc"),
            //            IsTetraFunction = flags.Contains("TetraFunc"),
            //            IsOTCSFunction = flags.Contains("OTCSFunc"),

            //            IsPASetting = flags.Contains("PASetting"),
            //            IsDMDSetting = flags.Contains("DMDSetting"),
            //            IsTetraSetting = flags.Contains("TetraSetting"),
            //            IsAlarmSetting = flags.Contains("AlarmSetting"),
            //            IsSystemSetting = flags.Contains("SystemSetting"),
            //            IsUserSetting = flags.Contains("UserSetting"),

            //            Accounts = new List<AccountDto>()
            //        };
            //    })
            //    .ToDictionary(dto => dto.Id, dto => dto);

            //// 再把 User 映成 AccountDto，並加回對應的 RoleDto.Accounts
            //var accountDtos = userList
            //    .Select(u =>
            //    {
            //        roleDtoDict.TryGetValue(u.BelongRoleId, out var role);

            //        var acct = new AccountDto
            //        {
            //            UserID = u.Id,
            //            UserName = u.Name,
            //            Description = u.Description,
            //            RoleDto = role
            //        };

            //        // 如果有對應的角色，就把帳號加進去
            //        role?.Accounts.Add(acct);
            //        return acct;
            //    })
            //    .ToList();

            //// 最後把角色 DTO 丟到 ObservableCollection 給 UI 綁定
            //SYSUserRoleDtos = new ObservableCollection<RoleDto>(roleDtoDict.Values);
            //SYSUserAccountDtos = new ObservableCollection<AccountDto>(accountDtos);

            //// 主動指定選擇第一筆群組
            //SelectedRole = SYSUserRoleDtos.FirstOrDefault();

            //// 設定 CollectionView 筆刷
            //SYSUserAccountDtosView = CollectionViewSource.GetDefaultView(SYSUserAccountDtos);
            //SYSUserAccountDtosView.Filter = CollectionViewFilter;

            //// 通知畫面更新
            //RaisePropertyChanged(nameof(SYSUserRoleDtos));
            //RaisePropertyChanged(nameof(SYSUserAccountDtos));
            //RaisePropertyChanged(nameof(SYSUserAccountDtosView));

            ////快照初始化
            //UpdateSnapshot();
        }
        #endregion
    }
}
