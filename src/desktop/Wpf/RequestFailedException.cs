using System;

namespace Budgetter.Wpf;

internal class RequestFailedException : Exception
{
    public RequestFailedException(
        string message,
        string details) : base(message)
    {
        Details = details;
    }

    public RequestFailedException(
        string message,
        string details,
        Exception innerException) : base(message, innerException)
    {
        Details = details;
    }

    public string Details { get; init; }
}