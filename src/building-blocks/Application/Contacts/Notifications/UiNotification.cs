using System.Collections.Generic;
using System.Linq;
using Budgetter.BuildingBlocks.Application.Projections;

namespace Budgetter.BuildingBlocks.Application.Contacts.Notifications;

public class UiNotification<T> : IUiNotification
    where T : class, IReadModel
{
    private UiNotification(string operationName)
    {
        OperationName = operationName;
        Errors = new Dictionary<string, string>();
    }

    private UiNotification(
        T changedObject,
        string operationName)
    {
        OperationName = operationName;
        IsSuccess = true;
        ChangedObject = changedObject;
        Errors = new Dictionary<string, string>();
    }

    public string OperationName { get; }
    public T ChangedObject { get; }
    public bool IsSuccess { get; }
    public IDictionary<string, string> Errors { get; }

    public UiNotification<T> AddError(string errorType, string errorMessage)
    {
        Errors.Add(errorType, errorMessage);

        return this;
    }

    public UiNotification<T> AddErrorRange(Dictionary<string, string> errors)
    {
        foreach (var (key, value) in errors)
            Errors.Add(key, value);

        return this;
    }

    public string GetErrors()
    {
        var errorsList = Errors.Select(error => $"{error.Key}: {error.Value}").ToList();
        var errorMessage = string.Join("/n", errorsList);

        return errorMessage;
    }

    public static UiNotification<T> Success(T changedObject, string operationName)
    {
        return new UiNotification<T>(changedObject, operationName);
    }

    public static UiNotification<T> Failed(string operationName)
    {
        return new UiNotification<T>(operationName);
    }
}