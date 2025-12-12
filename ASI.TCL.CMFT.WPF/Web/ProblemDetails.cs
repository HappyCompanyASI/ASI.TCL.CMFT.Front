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