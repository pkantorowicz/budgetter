using System;
using System.Collections.Generic;

namespace Budgetter.BuildingBlocks.Application.Contacts.Results;

public class CommandResult : Result, ICommandResult
{
    public static CommandResult Success()
    {
        return new CommandResult { Ok = true };
    }

    public static CommandResult SuccessWithDetails(string message, IDictionary<string, string> errors)
    {
        return new CommandResult { Ok = true, Errors = errors, Message = message };
    }

    public static CommandResult Failed(string error)
    {
        return new CommandResult { Ok = false, Message = error };
    }

    public static CommandResult Failed(IDictionary<string, string> errors)
    {
        return new CommandResult { Ok = false, Errors = errors };
    }

    public static CommandResult Failed(string error, Exception exception)
    {
        return new CommandResult { Ok = false, Message = error, Exception = exception };
    }

    public static CommandResult Failed(IDictionary<string, string> errors, Exception exception)
    {
        return new CommandResult { Ok = false, Errors = errors, Exception = exception };
    }
}

public class CommandResult<T> : Result, ICommandResult<T>
{
    public T Data { get; private init; }

    public static CommandResult<T> Success(T data)
    {
        return new CommandResult<T> { Ok = true, Data = data };
    }

    public static CommandResult<T> Failed(string error)
    {
        return new CommandResult<T> { Ok = false, Message = error };
    }

    public static CommandResult<T> Failed(IDictionary<string, string> errors)
    {
        return new CommandResult<T> { Ok = false, Errors = errors };
    }

    public static CommandResult<T> Failed(string message, Exception exception)
    {
        return new CommandResult<T> { Ok = false, Message = message, Exception = exception };
    }

    public static CommandResult<T> Failed(IDictionary<string, string> errors, Exception exception)
    {
        return new CommandResult<T> { Ok = false, Errors = errors, Exception = exception };
    }
}