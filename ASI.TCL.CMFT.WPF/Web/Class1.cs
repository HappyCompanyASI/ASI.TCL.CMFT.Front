using System;
using System.Collections.Generic;

public class ProblemDetails
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public string Instance { get; set; }

    public Dictionary<string, object> Extensions { get; private set; }

    public ProblemDetails()
    {
        Extensions = new Dictionary<string, object>();
    }
}


 

public class ApiException : Exception
{
    public ProblemDetails Problem { get; private set; }

    public ApiException(ProblemDetails problem)
        : base(problem != null ? problem.Title : "API Error")
    {
        Problem = problem;
    }
}
public interface IAuthService
{
    string GetToken();
}
public class MyAuthService : IAuthService
{
    private string _token;

    public string GetToken()
    {
        return _token;
    }

    public void SetToken(string token)
    {
        _token = token;
    }

    public void ClearToken()
    {
        _token = null;
    }
}

public interface IApiLogger
{
    void Debug(string message);
    void Debug(string message, object data);
    void Error(string message);
    void Error(string message, object data);
}

public class ApiClientOptions
{
    public string BaseUrl { get; set; }
    public IAuthService AuthService { get; set; }
    public IApiLogger Logger { get; set; }
}

public class DefaultApiLogger : IApiLogger
{
    public void Debug(string message)
    {
        //Debug.WriteLine("[DEBUG] " + message);
    }

    public void Debug(string message, object data)
    {
        //Debug.WriteLine("[DEBUG] " + message + " " + data);
    }

    public void Error(string message)
    {
        //Debug.WriteLine("[ERROR] " + message);
    }

    public void Error(string message, object data)
    {
        //Debug.WriteLine("[ERROR] " + message + " " + data);
    }
}
