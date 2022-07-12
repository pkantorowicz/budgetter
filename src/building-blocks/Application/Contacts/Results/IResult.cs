using System;
using System.Collections.Generic;

namespace Budgetter.BuildingBlocks.Application.Contacts.Results;

public interface IResult
{
    bool Ok { get; }
    string Message { get; }
    IDictionary<string, string> Errors { get; }
    Exception Exception { get; }
    string FormatDetails();
}