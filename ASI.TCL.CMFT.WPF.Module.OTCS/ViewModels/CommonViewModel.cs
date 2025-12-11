using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.OTCS.ViewModels
{
    public class CommonViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        public CommonViewModel()
        {
        }


        public ObservableCollection<object> SelectedTargets { get; set; } = new ObservableCollection<object>();

        private DelegateCommand<object> _deleteCommand;
        public DelegateCommand<object> DeleteCommand => _deleteCommand ??= new DelegateCommand<object>(ExcuteDeleteCommand);
        private void ExcuteDeleteCommand(object domaonModel) => SelectedTargets.Remove(domaonModel);
    }
}