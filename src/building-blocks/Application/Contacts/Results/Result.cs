using System;
using System.Collections.Generic;
using System.Text;

namespace Budgetter.BuildingBlocks.Application.Contacts.Results;

public class Result : IResult
{
    public bool Ok { get; protected init; }
    public IDictionary<string, string> Errors { get; protected init; }
    public string Message { get; protected init; }
    public Exception Exception { get; protected init; }

    public string FormatDetails()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(Message))
            sb.AppendLine(Message);

        if (Errors is { Count: > 0 })
            foreach (var (key, value) in Errors)
            {
                sb.AppendFormat("{0}: {1}", key, value);
                sb.AppendLine();
            }

        if (Exception != null)
            sb.AppendLine($"Ex: {Exception}");

        return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }
}