using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using ASI.TCL.CMFT.WPF.Logger;
using ASI.TCL.CMFT.WPF.Web;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly IAuthService _auth;
    private readonly ILogService _log;
    private readonly JsonSerializerOptions _jsonOpt;

    public ApiClient(HttpClient http, IAuthService auth, ILogService log)
    {
        if (http == null) throw new ArgumentNullException(nameof(http));
        if (auth == null) throw new ArgumentNullException(nameof(auth));
        if (log == null) throw new ArgumentNullException(nameof(log));
        if (http.BaseAddress == null) throw new InvalidOperationException("HttpClient.BaseAddress is required.");

        _http = http;
        _auth = auth;
        _log = log;

        _baseUrl = http.BaseAddress.ToString().TrimEnd('/');

        _jsonOpt = new JsonSerializerOptions();
        _jsonOpt.PropertyNameCaseInsensitive = true;
    }

    // ===========================================================
    // Query（讀取）
    // ===========================================================
    public Task<T> GetAsync<T>(string endpoint)
    {
        var url = BuildUrl(endpoint, null);
        return SendReadAsync<T>("GET", url);
    }

    public Task<T> GetAsync<T, TQuery>(string endpoint, TQuery query)
    {
        var url = BuildUrl(endpoint, query);
        return SendReadAsync<T>("GET", url);
    }

    // ===========================================================
    // Command（寫入，不回傳）
    // ===========================================================
    public async Task PostAsync<TRequest>(string endpoint, TRequest body)
    {
        var url = BuildUrl(endpoint, null);
        await SendWriteAsync("POST", url, body).ConfigureAwait(false);
    }

    public async Task PutAsync<TRequest>(string endpoint, TRequest body)
    {
        var url = BuildUrl(endpoint, null);
        await SendWriteAsync("PUT", url, body).ConfigureAwait(false);
    }

    public async Task DeleteAsync(string endpoint)
    {
        var url = BuildUrl(endpoint, null);
        await SendWriteAsync("DELETE", url, null).ConfigureAwait(false);
    }

    // ===========================================================
    // Command（寫入，有回傳）— login / create 常用
    // ===========================================================
    public Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
    {
        var url = BuildUrl(endpoint, null);
        return SendReadAsync<TResponse>("POST", url, body);
    }

    public Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body)
    {
        var url = BuildUrl(endpoint, null);
        return SendReadAsync<TResponse>("PUT", url, body);
    }

    // ===========================================================
    // Core: Read (parse JSON)
    // ===========================================================
    private async Task<T> SendReadAsync<T>(string method, string url)
    {
        return await SendReadAsync<T>(method, url, null).ConfigureAwait(false);
    }

    private async Task<T> SendReadAsync<T>(string method, string url, object body)
    {
        var req = CreateRequest(method, url, body);

        if (body != null)
            _log.Info(ApiLogMessage("API " + method + " " + url, body));
        else
            _log.Info("API " + method + " " + url);

        var res = await _http.SendAsync(req).ConfigureAwait(false);
        var raw = await ReadContentSafely(res).ConfigureAwait(false);

        if (!res.IsSuccessStatusCode)
        {
            var problem = ToProblem(raw, res);
            _log.Error(ApiLogMessage("API Error " + method + " " + url, problem));
            throw new ApiException(problem);
        }

        if (string.IsNullOrEmpty(raw))
            return default(T);

        return JsonSerializer.Deserialize<T>(raw, _jsonOpt);
    }

    // ===========================================================
    // Core: Write (no JSON parsing)
    // ===========================================================
    private async Task SendWriteAsync(string method, string url, object body)
    {
        var req = CreateRequest(method, url, body);

        if (body != null)
            _log.Info(ApiLogMessage("API " + method + " " + url, body));
        else
            _log.Info("API " + method + " " + url);

        var res = await _http.SendAsync(req).ConfigureAwait(false);
        var raw = await ReadContentSafely(res).ConfigureAwait(false);

        if (!res.IsSuccessStatusCode)
        {
            var problem = ToProblem(raw, res);
            _log.Error(ApiLogMessage("API Error " + method + " " + url, problem));
            throw new ApiException(problem);
        }
    }

    // ===========================================================
    // Request Builder
    // ===========================================================
    private HttpRequestMessage CreateRequest(string method, string url, object body)
    {
        var req = new HttpRequestMessage(new HttpMethod(method), url);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var token = _auth.GetToken();
        if (!string.IsNullOrEmpty(token))
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (body != null && method != "GET" && method != "DELETE")
        {
            var json = JsonSerializer.Serialize(body, _jsonOpt);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return req;
    }

    private async Task<string> ReadContentSafely(HttpResponseMessage res)
    {
        if (res == null) return string.Empty;
        if (res.Content == null) return string.Empty;
        return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    // ===========================================================
    // Helpers
    // ===========================================================
    private string BuildUrl(string endpoint, object query)
    {
        if (string.IsNullOrEmpty(endpoint))
            throw new ArgumentException("endpoint is required.", nameof(endpoint));

        string url;

        // endpoint 若已是完整 URL，就不要拼 baseUrl
        if (endpoint.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            endpoint.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            url = endpoint.TrimEnd('/');
        }
        else
        {
            url = _baseUrl + "/" + endpoint.TrimStart('/');
        }

        if (query == null) return url;

        var parts = new List<string>();
        var props = query.GetType().GetProperties();

        for (int i = 0; i < props.Length; i++)
        {
            var p = props[i];
            var val = p.GetValue(query, null);
            if (val == null) continue;

            var key = Uri.EscapeDataString(p.Name);
            var v = Uri.EscapeDataString(Convert.ToString(val));

            parts.Add(key + "=" + v);
        }

        if (parts.Count == 0) return url;

        return url + "?" + string.Join("&", parts);
    }

    private string ApiLogMessage(string message, object data)
    {
        if (data == null) return message;

        try
        {
            return message + " " + JsonSerializer.Serialize(data, _jsonOpt);
        }
        catch
        {
            return message + " " + data.ToString();
        }
    }

    private ProblemDetails ToProblem(string raw, HttpResponseMessage res)
    {
        try
        {
            var pd = JsonSerializer.Deserialize<ProblemDetails>(raw, _jsonOpt);
            if (pd == null) pd = new ProblemDetails();

            if (pd.Status == 0) pd.Status = (int)res.StatusCode;
            if (string.IsNullOrEmpty(pd.Title)) pd.Title = res.ReasonPhrase;

            return pd;
        }
        catch
        {
            return new ProblemDetails
            {
                Status = (int)res.StatusCode,
                Title = res.ReasonPhrase,
                Detail = raw
            };
        }
    }
}
