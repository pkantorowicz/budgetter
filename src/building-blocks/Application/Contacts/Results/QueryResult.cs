using System;
using System.Collections.Generic;

namespace Budgetter.BuildingBlocks.Application.Contacts.Results;

public class QueryResult<T> : Result, IQueryResult<T>
{
    public T Data { get; private init; }

    public static QueryResult<T> Success(T data)
    {
        return new QueryResult<T> { Ok = true, Data = data };
    }

    public static QueryResult<T> Failed(string error)
    {
        return new QueryResult<T> { Ok = false, Message = error };
    }

    public static QueryResult<T> Failed(IDictionary<string, string> errors)
    {
        return new QueryResult<T> { Ok = false, Errors = errors };
    }

    public static QueryResult<T> Failed(string message, Exception exception)
    {
        return new QueryResult<T> { Ok = false, Message = message, Exception = exception };
    }

    public static QueryResult<T> Failed(IDictionary<string, string> errors, Exception exception)
    {
        return new QueryResult<T> { Ok = false, Errors = errors, Exception = exception };
    }
}