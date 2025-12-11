using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ASI.TCL.CMFT.Messages.PA;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.PA.Dtos;
using ASI.TCL.CMFT.WPF.Module.PA.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.PA.ViewModels
{
    public class PAPreRecordedAudioSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public PAPreRecordedAudioSettingsViewModel()
        {
        }
        public PAPreRecordedAudioSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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
            set => SetProperty(ref _isEditMode, value);
        }

        private List<PAPreRecordVoiceDto> _originalVoices;

        public bool IsGroupOperation => SelectedVoiceGroup != null && SelectedVoice == null;
        public bool IsItemOperation => SelectedVoice != null;

        private PAVoiceGroupDto _selectedVoiceGroup;
        public PAVoiceGroupDto SelectedVoiceGroup
        {
            get => _selectedVoiceGroup;
            set
            {
                SetProperty(ref _selectedVoiceGroup, value);
                RaisePropertyChanged(nameof(IsGroupOperation));
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        private PAPreRecordVoiceDto _selectedVoice;
        public PAPreRecordVoiceDto SelectedVoice
        {
            get => _selectedVoice;
            set
            {
                SetProperty(ref _selectedVoice, value);
                RaisePropertyChanged(nameof(IsGroupOperation));
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        // ListBox 顯示群組
        private ObservableCollection<PAVoiceGroupDto> _paVoiceGroupDtos = new();
        public ObservableCollection<PAVoiceGroupDto> PAVoiceGroupDtos
        {
            get => _paVoiceGroupDtos;
            set => SetProperty(ref _paVoiceGroupDtos, value);
        }
        // ComboBox 選項來源
        public ObservableCollection<PAVoiceGroupDto> EditableGroups =>
            new ObservableCollection<PAVoiceGroupDto>(PAVoiceGroupDtos.Where(g => g.GroupName != "全部"));
        // ListView 顯示訊息
        private ObservableCollection<PAPreRecordVoiceDto> _paPreRecordVoices = new();
        public ObservableCollection<PAPreRecordVoiceDto> PAPreRecordVoices
        {
            get => _paPreRecordVoices;
            set => SetProperty(ref _paPreRecordVoices, value);
        }
        // 用來做篩選後的訊息顯示
        public ICollectionView PAPreRecordVoicesView { get; private set; }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addGroupCommand;
        public DelegateCommand AddGroupCommand => _addGroupCommand ??= new DelegateCommand(ExcuteAddGroupCommand);
        private void ExcuteAddGroupCommand()
        {
            _dialogService.ShowDialogHost(nameof(PAVoiceGroupSettingsDialogView), null, async result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey("GroupName"))
                {
                    string groupName = result.Parameters.GetValue<string>("GroupName");

                    Guid guid = Guid.NewGuid();

                    //畫面
                    var newGroup = new PAVoiceGroupDto
                    {
                        Id = guid,
                        GroupName = groupName
                    };

                    //資料庫
                    //await _requestHandler.HandleCommand(new Commands.CreateGroup
                    //{
                    //    Id = guid,
                    //    Name = groupName
                    //}, _paApplicationService.Handle);

                    PAVoiceGroupDtos.Add(newGroup);
                    UpdateSnapshot();
                    RaisePropertyChanged(nameof(EditableGroups));
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _deleteGroupCommand;
        public DelegateCommand DeleteGroupCommand => _deleteGroupCommand ??= new DelegateCommand(ExcuteDeleteGroupCommand);
        private void ExcuteDeleteGroupCommand()
        {
            var groupToRemove = SelectedVoiceGroup;

            if (SelectedVoiceGroup == null || SelectedVoiceGroup.GroupName == "全部")
                return;

            _dialogService.IsDelete($"確定要刪除\"{groupToRemove.GroupName}\"這個群組嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //LOG 刪除群組
                    var groupToRemove = SelectedVoiceGroup;

                    //await _requestHandler.HandleCommand(new Commands.DeleteGroupAndDetachVoices
                    //{
                    //    Id = groupToRemove.Id,
                    //}, _paApplicationService.Handle);

                    // 清除屬於該群組的項目的群組關聯
                    foreach (var message in PAPreRecordVoices.Where(m => m.BelongGroup?.Id == groupToRemove.Id).ToList())
                    {
                        message.BelongGroup = null;
                    }

                    // 從群組集合中移除該群組
                    PAVoiceGroupDtos.Remove(groupToRemove);

                    // 更新快照
                    UpdateSnapshot();

                    // 刷新 ListView 顯示
                    PAPreRecordVoicesView.Refresh();

                    RaisePropertyChanged(nameof(EditableGroups));
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private async void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedVoice;

            //await _requestHandler.HandleCommand(new Commands.ChangeVoiceBelongGroup()
            //{
            //    Id = itemToUpdate.Id,
            //    NewBelongGroupId = itemToUpdate.BelongGroup.Id
            //}, _paApplicationService.Handle);

            // 更新快照
            UpdateSnapshot();
            PAPreRecordVoicesView.Refresh();

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
            // 判斷是否資料有變
            if (!HasChanges())
            {
                IsEditMode = false;
                return;
            }

            var allGroups = PAVoiceGroupDtos.Where(g => g.GroupName != "全部").ToList();
            var groupDict = allGroups.ToDictionary(g => g.Id);

            // 還原群組訊息
            foreach (var group in allGroups)
            {
                group.Voices = _originalVoices
                    .Where(m => m.BelongGroup?.Id == group.Id)
                    .Select(m => new PAPreRecordVoiceDto
                    {
                        Id = m.Id,
                        VoiceName = m.VoiceName,
                        VoiceContent = m.VoiceContent,
                        BelongGroup = group
                    })
                    .ToList();
            }

            // 額外找出未歸屬任何群組的訊息
            var ungroupedMessages = _originalVoices
                .Where(m => m.BelongGroup == null)
                .Select(m => new PAPreRecordVoiceDto
                {
                    Id = m.Id,
                    VoiceName = m.VoiceName,
                    VoiceContent = m.VoiceContent,
                    BelongGroup = null
                });

            // 組合所有訊息
            var flatList = allGroups.SelectMany(g => g.Voices).Concat(ungroupedMessages).ToList();

            PAPreRecordVoices = new ObservableCollection<PAPreRecordVoiceDto>(flatList);
            RaisePropertyChanged(nameof(PAPreRecordVoices));

            PAPreRecordVoicesView = CollectionViewSource.GetDefaultView(PAPreRecordVoices);
            PAPreRecordVoicesView.Filter = CollectionViewFilter;
            RaisePropertyChanged(nameof(PAPreRecordVoicesView));

            IsEditMode = false;
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
            PAPreRecordVoicesView?.Refresh();
        }

        private DelegateCommand _clearSelectedVoiceCommand;
        public DelegateCommand ClearSelectedVoiceCommand => _clearSelectedVoiceCommand ??= new DelegateCommand(ExecuteClearSelectedVoiceCommand);
        private void ExecuteClearSelectedVoiceCommand()
        {
            SelectedVoice = null;
        }
        #endregion

        #region Private method
        private bool CollectionViewFilter(object item)
        {
            if (item is PAPreRecordVoiceDto message)
            {
                // SelectedGroup == null ➔ 全部
                // SelectedGroup.GroupName == "全部" ➔ 也視為不篩選
                if (SelectedVoiceGroup == null || SelectedVoiceGroup.GroupName == "全部")
                    return true; // 顯示所有訊息

                return message.BelongGroup?.Id == SelectedVoiceGroup.Id;
            }
            return false;
        }
        private bool HasChanges()
        {
            if (_originalVoices.Count != PAPreRecordVoices.Count)
                return true;

            foreach (var current in PAPreRecordVoices)
            {
                var original = _originalVoices.FirstOrDefault(m => m.Id == current.Id);
                if (original == null)
                    return true;

                if (original.VoiceName != current.VoiceName ||
                    original.VoiceContent != current.VoiceContent ||
                    original.BelongGroup?.Id != current.BelongGroup?.Id)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalVoices = PAPreRecordVoices
                .Select(m => new PAPreRecordVoiceDto
                {
                    Id = m.Id,
                    VoiceName = m.VoiceName,
                    VoiceContent = m.VoiceContent,
                    BelongGroup = PAVoiceGroupDtos.FirstOrDefault(g => g.Id == m.BelongGroup?.Id),
                }).ToList();
        }
        private async Task InitDataAsync()
        {
            //// 讀取所有群組
            //var groups = await _requestHandler.HandleQuery(() =>
            //        _queryService.Query(new QueryModels.GetAllPreRecordVoiceGroup()));

            //// 讀取所有訊息
            //var voices = await _requestHandler.HandleQuery(() =>
            //        _queryService.Query(new QueryModels.GetAllPreRecordVoice()));

            //// 建立「全部」群組（不對應資料庫）
            //var allGroup = new PAVoiceGroupDto
            //{
            //    Id = Guid.Empty,      // 或用 "ALL" 之類的固定字串
            //    GroupName = "全部",
            //    Voices = new ObservableCollection<PAPreRecordVoiceDto>()
            //};

            //// 映射到 UI DTO
            //var groupDict = groups
            //    .Select(g => new PAVoiceGroupDto
            //    {
            //        Id = g.Id,
            //        GroupName = g.GroupName,
            //        Voices = new ObservableCollection<PAPreRecordVoiceDto>()
            //    })
            //    .ToDictionary(g => g.Id, g => g);

            //var msgDtos = voices
            //    .Select(m => new PAPreRecordVoiceDto
            //    {
            //        Id = m.Id.ToString(),
            //        VoiceName = m.VoiceName,
            //        VoiceContent = m.VoiceContent,
            //        IsIncludeCHN = m.IsIncludeCHN,
            //        IsIncludeTWN = m.IsIncludeTWN,
            //        IsIncludeHAKKA = m.IsIncludeHAKKA,
            //        IsIncludeENG = m.IsIncludeENG,
            //        BelongGroup = groupDict.TryGetValue(m.BelongGroupId, out var grp) ? grp : null
            //    })
            //    .ToList();

            //// 把訊息放回群組的 Messages 集合
            //foreach (var msg in msgDtos)
            //{
            //    if (msg.BelongGroup != null)
            //    {
            //        msg.BelongGroup.Voices.Add(msg);
            //    }
            //}

            //// 同時把所有訊息也放到「全部」群組
            //foreach (var msg in msgDtos)
            //    allGroup.Voices.Add(msg);

            //// 構造群組清單：先「全部」，再真正的群組
            //var uiGroups = new List<PAVoiceGroupDto> { allGroup };
            //uiGroups.AddRange(groupDict.Values);

            //// 更新 ViewModel 屬性
            //PAVoiceGroupDtos = new ObservableCollection<PAVoiceGroupDto>(uiGroups);
            //PAPreRecordVoices = new ObservableCollection<PAPreRecordVoiceDto>(msgDtos);

            //// 主動指定選擇第一筆群組
            //SelectedVoiceGroup = PAVoiceGroupDtos.FirstOrDefault();

            //// 設定 CollectionView 筆刷
            //PAPreRecordVoicesView = CollectionViewSource.GetDefaultView(PAPreRecordVoices);
            //PAPreRecordVoicesView.Filter = CollectionViewFilter;

            //// 通知畫面更新
            //RaisePropertyChanged(nameof(PAVoiceGroupDtos));
            //RaisePropertyChanged(nameof(PAPreRecordVoices));
            //RaisePropertyChanged(nameof(PAPreRecordVoicesView));

            ////快照初始化
            //UpdateSnapshot();
        }
        #endregion
    }
}
