using System.Collections.Generic;

namespace Budgetter.BuildingBlocks.Application.Exceptions;

public class InvalidCommandException : ApplicationException
{
    public InvalidCommandException(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    public IDictionary<string, string> Errors { get; }
}