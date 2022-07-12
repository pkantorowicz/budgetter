using System;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Notifications;
using Budgetter.BuildingBlocks.Application.Notifications;

namespace Budgetter.Infrastructure.Configuration.Notifications;

public class NotificationsHub : INotificationsHub
{
    public void Subscribe(EventHandler<object> eventHandler)
    {
        ReadModelUpdated += eventHandler;
    }

    public void Unsubscribe(EventHandler<object> eventHandler)
    {
        ReadModelUpdated -= eventHandler;
    }

    public async Task NotifyAboutReadModelChanges<T>(T message) where T : class, IUiNotification
    {
        ReadModelUpdated?.Invoke(this, message);

        await Task.CompletedTask;
    }

    private event EventHandler<object> ReadModelUpdated;
}