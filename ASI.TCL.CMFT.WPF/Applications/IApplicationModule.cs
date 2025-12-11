using System.Threading.Tasks;

namespace ASI.TCL.CMFT.WPF.Applications
{
    public interface IModuleApplication
    {
        Task<AppInitResult> InitAsync();
        Task<string> UninitAsync();
        void Update();
    }
}