using System;

public class ApiException : Exception
{
    public ProblemDetails Problem { get; private set; }

    public ApiException(ProblemDetails problem)
        : base(problem != null ? problem.Title : "API Error")
    {
        Problem = problem;
    }
}