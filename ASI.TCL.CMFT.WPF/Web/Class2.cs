using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public interface IApiClient
{
    // GET: 回傳資料
    Task<T> Get<T>(string endpoint);
    Task<T> Get<T, Q>(string endpoint, Q query);

    // POST: 不回傳資料
    Task Post<TRequest>(string endpoint, TRequest body);

    // PUT: 不回傳資料
    Task Put<TRequest>(string endpoint, TRequest body);

    // DELETE: 不回傳資料
    Task Delete(string endpoint);
}



public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly IAuthService _auth;
    private readonly IApiLogger _log;
    private readonly JsonSerializerOptions _jsonOpt;

    public ApiClient(HttpClient http, ApiClientOptions opt)
    {
        if (http == null) throw new ArgumentNullException("http");
        if (opt == null) throw new ArgumentNullException("opt");

        _http = http;
        _baseUrl = opt.BaseUrl.TrimEnd('/');
        _auth = opt.AuthService;
        _log = opt.Logger ?? new DefaultApiLogger();

        _jsonOpt = new JsonSerializerOptions();
        _jsonOpt.PropertyNameCaseInsensitive = true;
    }

    //===========================================================
    // GET 回傳資料
    //===========================================================
    public Task<T> Get<T>(string endpoint)
    {
        string url = BuildUrl(endpoint, null);
        return Send<T>("GET", url, null);
    }

    public Task<T> Get<T, Q>(string endpoint, Q query)
    {
        string url = BuildUrl(endpoint, query);
        return Send<T>("GET", url, null);
    }

    //===========================================================
    // POST 不回資料
    //===========================================================
    public Task Post<TRequest>(string endpoint, TRequest body)
    {
        string url = BuildUrl(endpoint, null);
        return Send<object>("POST", url, body);
    }

    //===========================================================
    // PUT 不回資料
    //===========================================================
    public Task Put<TRequest>(string endpoint, TRequest body)
    {
        string url = BuildUrl(endpoint, null);
        return Send<object>("PUT", url, body);
    }

    //===========================================================
    // DELETE 不回資料
    //===========================================================
    public Task Delete(string endpoint)
    {
        string url = BuildUrl(endpoint, null);
        return Send<object>("DELETE", url, null);
    }

    //===========================================================
    // 核心
    //===========================================================
    private async Task<T> Send<T>(string method, string url, object body)
    {
        var req = new HttpRequestMessage(new HttpMethod(method), url);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        string token = _auth.GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (body != null && method != "GET" && method != "DELETE")
        {
            string json = JsonSerializer.Serialize(body, _jsonOpt);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            _log.Debug("API " + method + " " + url, body);
        }
        else
        {
            _log.Debug("API " + method + " " + url);
        }

        var res = await _http.SendAsync(req).ConfigureAwait(false);
        string raw = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!res.IsSuccessStatusCode)
        {
            ProblemDetails problem = ToProblem(raw, res);
            _log.Error("API Error", problem);
            throw new ApiException(problem);
        }

        if (typeof(T) == typeof(object))
        {
            return default(T);
        }

        if (string.IsNullOrEmpty(raw))
        {
            return default(T);
        }

        return JsonSerializer.Deserialize<T>(raw, _jsonOpt);
    }

    //===========================================================
    // Helpers
    //===========================================================
    private string BuildUrl(string endpoint, object query)
    {
        string url = _baseUrl + "/" + endpoint.TrimStart('/');

        if (query == null) return url;

        List<string> parts = new List<string>();
        var props = query.GetType().GetProperties();

        foreach (var p in props)
        {
            object val = p.GetValue(query, null);
            if (val == null) continue;

            string key = Uri.EscapeDataString(p.Name);
            string v = Uri.EscapeDataString(Convert.ToString(val));

            parts.Add(key + "=" + v);
        }

        if (parts.Count == 0) return url;

        return url + "?" + string.Join("&", parts);
    }

    private ProblemDetails ToProblem(string raw, HttpResponseMessage res)
    {
        try
        {
            ProblemDetails pd = JsonSerializer.Deserialize<ProblemDetails>(raw, _jsonOpt);
            if (pd == null) pd = new ProblemDetails();

            pd.Status = (pd.Status == 0 ? (int)res.StatusCode : pd.Status);
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

