using System;

namespace Budgetter.BuildingBlocks.Application.Exceptions;

public class ApplicationException : Exception
{
    protected ApplicationException()
    {
    }

    protected ApplicationException(string message) : base(message)
    {
    }
}