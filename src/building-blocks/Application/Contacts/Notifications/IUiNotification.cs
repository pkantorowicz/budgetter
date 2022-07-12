using System.Collections.Generic;

namespace Budgetter.BuildingBlocks.Application.Contacts.Notifications;

public interface IUiNotification
{
    public bool IsSuccess { get; }
    IDictionary<string, string> Errors { get; }
}