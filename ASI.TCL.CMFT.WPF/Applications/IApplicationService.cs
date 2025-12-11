using System.Threading.Tasks;

namespace ASI.TCL.CMFT.WPF.Applications
{
    public interface IApplication
    {
        Task<AppInitResult> InitApplicationAsync(string userID, string password);
        void UpdateApplication();
        Task<bool> UninitApplicationAsync();
    }
}