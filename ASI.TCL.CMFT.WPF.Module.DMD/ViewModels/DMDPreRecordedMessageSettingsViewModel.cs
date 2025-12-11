using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ASI.TCL.CMFT.Messages.DMD;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.DMD.Dtos;
using ASI.TCL.CMFT.WPF.Module.DMD.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.DMD.ViewModels
{
    public class DMDPreRecordedMessageSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public DMDPreRecordedMessageSettingsViewModel()
        {
        }
        public DMDPreRecordedMessageSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            
            _dialogService = dialogService;
            
            _eventAggregator = eventAggregator;
   
            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
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

        private List<DMDPreRecordMessageDto> _originalMessages;
        // 群組控制控制項
        public bool IsGroupOperation => SelectedMessageGroup != null && SelectedMessage == null;
        // 單一訊息控制項
        public bool IsItemOperation => SelectedMessage != null;
        // 單一訊息在編輯時隱藏新增刪除按鈕
        public bool CanItemAddDelete => SelectedMessage != null && !IsEditMode;
        // 新增群組後可以直接加項目
        public bool CanAddItem => SelectedMessageGroup != null && !IsEditMode;


        private DMDMessageGroupDto _selectedMessageGroup;
        public DMDMessageGroupDto SelectedMessageGroup
        {
            get => _selectedMessageGroup;
            set
            {
                SetProperty(ref _selectedMessageGroup, value);
                RaisePropertyChanged(nameof(IsGroupOperation));
                RaisePropertyChanged(nameof(IsItemOperation));
                RaisePropertyChanged(nameof(CanAddItem));
            }
        }

        private DMDPreRecordMessageDto _selectedMessage;
        public DMDPreRecordMessageDto SelectedMessage
        {
            get => _selectedMessage;
            set
            {
                SetProperty(ref _selectedMessage, value);
                RaisePropertyChanged(nameof(IsGroupOperation));
                RaisePropertyChanged(nameof(IsItemOperation));
                RaisePropertyChanged(nameof(CanItemAddDelete));
            }
        }
        // 預錄訊息 - 設定
        private ObservableCollection<DMDMessageGroupDto> _dmdMessageGroups = new();
        public ObservableCollection<DMDMessageGroupDto> DMDMessageGroups
        {
            get => _dmdMessageGroups;
            set => SetProperty(ref _dmdMessageGroups, value);
        }
        // 預錄群組 - 選項來源
        public ObservableCollection<DMDMessageGroupDto> EditableGroups =>
            new ObservableCollection<DMDMessageGroupDto>(DMDMessageGroups.Where(g => g.GroupName != "全部"));
        // CollectionView的訊息來源
        private ObservableCollection<DMDPreRecordMessageDto> _dmdPreRecordMessages = new();
        public ObservableCollection<DMDPreRecordMessageDto> DMDPreRecordMessages
        {
            get => _dmdPreRecordMessages;
            set => SetProperty(ref _dmdPreRecordMessages, value);
        }
        public ICollectionView DMDPreRecordMessagesView { get; private set; }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addGroupCommand;
        public DelegateCommand AddGroupCommand => _addGroupCommand ??= new DelegateCommand(ExcuteAddGroupCommand);
        private void ExcuteAddGroupCommand()
        {
            _dialogService.ShowDialogHost(nameof(DMDMessageGroupSettingsDialogView), null, async result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey("GroupName"))
                {
                    string groupName = result.Parameters.GetValue<string>("GroupName");

                    Guid guid = Guid.NewGuid();

                    //畫面
                    var newGroup = new DMDMessageGroupDto
                    {
                        Id = guid,
                        GroupName = groupName
                    };

                    //資料庫
                    //await _requestHandler.HandleCommand(new Application.DMD.Commands.CreateGroup
                    //{
                    //    Id = guid,
                    //    Name = groupName
                    //}, _dmdApplicationService.Handle);

                    DMDMessageGroups.Add(newGroup);
                    UpdateSnapshot();
                    RaisePropertyChanged(nameof(EditableGroups));
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _deleteGroupCommand;
        public DelegateCommand DeleteGroupCommand => _deleteGroupCommand ??= new DelegateCommand(ExcuteDeleteGroupCommand);
        private void ExcuteDeleteGroupCommand()
        {
            var groupToRemove = SelectedMessageGroup;

            if (SelectedMessageGroup == null || SelectedMessageGroup.GroupName == "全部")
                return;

            _dialogService.IsDelete($"確定要刪除\"{groupToRemove.GroupName}\"這個群組嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //LOG 刪除群組
                    var groupToRemove = SelectedMessageGroup;

                    //await _requestHandler.HandleCommand(new Application.DMD.Commands.DeleteGroupAndDetachMessages
                    //{
                    //    Id = groupToRemove.Id,
                    //}, _dmdApplicationService.Handle);

                    // 清除屬於該群組的項目的群組關聯
                    foreach (var message in DMDPreRecordMessages.Where(m => m.BelongGroup?.Id == groupToRemove.Id).ToList())
                    {
                        message.BelongGroup = null;
                    }

                    // 從群組集合中移除該群組
                    DMDMessageGroups.Remove(groupToRemove);

                    // 更新快照
                    UpdateSnapshot();

                    // 刷新 ListView 顯示
                    DMDPreRecordMessagesView.Refresh();

                    RaisePropertyChanged(nameof(EditableGroups));
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private async void ExcuteAddItemCommand()
        {
            //LOG 新增訊息
            if (SelectedMessageGroup != null)
            {
                Guid guid = Guid.NewGuid();
                //畫面
                var newItem = new DMDPreRecordMessageDto
                {
                    Id = guid.ToString(),
                    MessageName = "新訊息",
                    MessageContent = "新訊息內容",
                    BelongGroup = SelectedMessageGroup.GroupName == "全部"
                                ? null
                                : SelectedMessageGroup,
                    IsUseDU = false
                };
                //資料庫
                //await _requestHandler.HandleCommand(new Commands.CreateMessage
                //{
                //    Id = guid,
                //    Name = "新訊息",
                //    Content = "新訊息內容",
                //    IsUseDu = false,
                //    BelongGroupId = SelectedMessageGroup.GroupName == "全部"
                //                ? null
                //                : SelectedMessageGroup.Id
                //}, _dmdApplicationService.Handle);

                DMDPreRecordMessages.Add(newItem);
                UpdateSnapshot();
                DMDPreRecordMessagesView.Refresh(); // 刷新篩選後的訊息顯示
            }
        }

        private DelegateCommand _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(ExcuteDeleteItemCommand);
        private void ExcuteDeleteItemCommand()
        {

            var itemToDelete = SelectedMessage;

            if (itemToDelete == null)
                return;

            _dialogService.IsDelete($"確定要刪除\"{itemToDelete.MessageName}\"這筆訊息嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //await _requestHandler.HandleCommand(new Commands.DeleteMessage { Id = Guid.Parse(itemToDelete.Id) }, _dmdApplicationService.Handle);

                    SelectedMessage = null; // 清除選取

                    DMDPreRecordMessages.Remove(itemToDelete);

                    UpdateSnapshot();

                    DMDPreRecordMessagesView.Refresh(); // 刷新篩選
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private async void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedMessage;

            //await _requestHandler.HandleCommand(new Commands.UpdateMessageDetails()
            //{
            //    Id = Guid.Parse(itemToUpdate.Id),
            //    NewName = itemToUpdate.MessageName,
            //    NewContent = itemToUpdate.MessageContent,
            //    NewDUState = itemToUpdate.IsUseDU,
            //    NewBelongGroupId = itemToUpdate.BelongGroup?.Id
            //}, _dmdApplicationService.Handle);

            // 更新快照
            UpdateSnapshot();
            DMDPreRecordMessagesView.Refresh();

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
            // LOG 取消編輯

            // 判斷是否資料有變
            if (!HasChanges())
            {
                IsEditMode = false;
                return;
            }

            var allGroups = DMDMessageGroups.Where(g => g.GroupName != "全部").ToList();
            var groupDict = allGroups.ToDictionary(g => g.Id);

            // 還原群組訊息
            foreach (var group in allGroups)
            {
                group.Messages = _originalMessages
                    .Where(m => m.BelongGroup?.Id == group.Id)
                    .Select(m => new DMDPreRecordMessageDto
                    {
                        Id = m.Id,
                        MessageName = m.MessageName,
                        MessageContent = m.MessageContent,
                        BelongGroup = group,
                        IsUseDU = m.IsUseDU
                    })
                    .ToList();
            }

            // 額外找出未歸屬任何群組的訊息
            var ungroupedMessages = _originalMessages
                .Where(m => m.BelongGroup == null)
                .Select(m => new DMDPreRecordMessageDto
                {
                    Id = m.Id,
                    MessageName = m.MessageName,
                    MessageContent = m.MessageContent,
                    BelongGroup = null,
                    IsUseDU = m.IsUseDU
                });

            // 組合所有訊息
            var flatList = allGroups.SelectMany(g => g.Messages).Concat(ungroupedMessages).ToList();

            DMDPreRecordMessages = new ObservableCollection<DMDPreRecordMessageDto>(flatList);
            RaisePropertyChanged(nameof(DMDPreRecordMessages));

            DMDPreRecordMessagesView = CollectionViewSource.GetDefaultView(DMDPreRecordMessages);
            DMDPreRecordMessagesView.Filter = CollectionViewFilter;
            RaisePropertyChanged(nameof(DMDPreRecordMessagesView));

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
            DMDPreRecordMessagesView?.Refresh();
        }

        private DelegateCommand _clearSelectedMessageCommand;
        public DelegateCommand ClearSelectedMessageCommand => _clearSelectedMessageCommand ??= new DelegateCommand(ExecuteClearSelectedMessageCommand);
        private void ExecuteClearSelectedMessageCommand()
        {
            SelectedMessage = null;
        }
        #endregion

        #region Private Methods
        private bool CollectionViewFilter(object item)
        {
            if (item is DMDPreRecordMessageDto message)
            {
                if (SelectedMessageGroup == null || SelectedMessageGroup.GroupName == "全部")
                    return true;

                return message.BelongGroup?.Id == SelectedMessageGroup.Id;
            }
            return false;
        }
        private bool HasChanges()
        {
            if (_originalMessages.Count != DMDPreRecordMessages.Count)
                return true;

            foreach (var current in DMDPreRecordMessages)
            {
                var original = _originalMessages.FirstOrDefault(m => m.Id == current.Id);
                if (original == null)
                    return true;

                if (original.MessageName != current.MessageName ||
                    original.MessageContent != current.MessageContent ||
                    original.BelongGroup?.Id != current.BelongGroup?.Id ||
                    original.IsUseDU != current.IsUseDU) 
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalMessages = DMDPreRecordMessages 
                .Select(m => new DMDPreRecordMessageDto
                {
                    Id = m.Id,
                    MessageName = m.MessageName,
                    MessageContent = m.MessageContent,
                    BelongGroup = DMDMessageGroups.FirstOrDefault(g => g.Id == m.BelongGroup?.Id),
                    IsUseDU = m.IsUseDU
                }).ToList();
        }
        private async Task InitDataAsync()
        {
            //// 讀取所有群組
            //var groups = await _requestHandler.HandleQuery(() =>
            //        _queryService.Query(new QueryModels.GetAllPreRecordMessageGroup()));

            //// 讀取所有訊息
            //var messages = await _requestHandler.HandleQuery(() =>
            //        _queryService.Query(new QueryModels.GetAllPreRecordMessage()));

            //// 建立「全部」群組（不對應資料庫）
            //var allGroup = new DMDMessageGroupDto
            //{
            //    Id = Guid.Empty,      // 或用 "ALL" 之類的固定字串
            //    GroupName = "全部",
            //    Messages = new ObservableCollection<DMDPreRecordMessageDto>()
            //};

            //// 映射到 UI DTO
            //var groupDict = groups
            //    .Select(g => new DMDMessageGroupDto
            //    {
            //        Id = g.Id,
            //        GroupName = g.GroupName,
            //        Messages = new ObservableCollection<DMDPreRecordMessageDto>()
            //    })
            //    .ToDictionary(g => g.Id, g => g);

            //var msgDtos = messages
            //    .Select(m => new DMDPreRecordMessageDto
            //    {
            //        Id = m.Id.ToString(),
            //        MessageName = m.MessageName,
            //        MessageContent = m.MessageContent,
            //        BelongGroup = groupDict.TryGetValue(m.BelongGroupId, out var grp) ? grp : null,
            //        IsUseDU = m.IsUseDU
            //    })
            //    .ToList();

            //// 把訊息放回群組的 Messages 集合
            //foreach (var msg in msgDtos)
            //{
            //    if (msg.BelongGroup != null)
            //    {
            //        msg.BelongGroup.Messages.Add(msg);
            //    }
            //}

            //// 同時把所有訊息也放到「全部」群組
            //foreach (var msg in msgDtos)
            //    allGroup.Messages.Add(msg);

            //// 構造群組清單：先「全部」，再真正的群組
            //var uiGroups = new List<DMDMessageGroupDto> { allGroup };
            //uiGroups.AddRange(groupDict.Values);

            //// 更新 ViewModel 屬性
            //DMDMessageGroups = new ObservableCollection<DMDMessageGroupDto>(uiGroups);
            //DMDPreRecordMessages = new ObservableCollection<DMDPreRecordMessageDto>(msgDtos);

            //// 主動指定選擇第一筆群組
            //SelectedMessageGroup = DMDMessageGroups.FirstOrDefault();

            //// 設定 CollectionView 筆刷
            //DMDPreRecordMessagesView = CollectionViewSource.GetDefaultView(DMDPreRecordMessages);
            //DMDPreRecordMessagesView.Filter = CollectionViewFilter;

            //// 通知畫面更新
            //RaisePropertyChanged(nameof(DMDMessageGroups));
            //RaisePropertyChanged(nameof(DMDPreRecordMessages));
            //RaisePropertyChanged(nameof(DMDPreRecordMessagesView));

            ////快照初始化
            //UpdateSnapshot();
        }
        #endregion
    }
}
