using System.Threading.Tasks;

namespace ASI.TCL.CMFT.WPF.Web
{
    public interface IApiClient
    {
        // ===========================================================
        // Query（讀取）
        // ===========================================================
        Task<T> GetAsync<T>(string endpoint);
        Task<T> GetAsync<T, TQuery>(string endpoint, TQuery query);

        // ===========================================================
        // Command（寫入，不回傳）
        // ===========================================================
        Task PostAsync<TRequest>(string endpoint, TRequest body);
        Task PutAsync<TRequest>(string endpoint, TRequest body);
        Task DeleteAsync(string endpoint);

        // ===========================================================
        // Command（寫入，有回傳）— 用於 login / create
        // ===========================================================
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body);
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body);
    }
}