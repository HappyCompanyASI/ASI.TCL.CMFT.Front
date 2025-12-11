using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Applications;

namespace ASI.TCL.CMFT.WPF.Utilities
{
    public interface IAsyncLifecycle 
    {
        Task<AppInitResult> InitializeAsync();
        void Update();
        Task<bool> UninitializeAsync();
    }
}