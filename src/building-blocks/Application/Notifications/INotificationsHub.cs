using System;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Notifications;

namespace Budgetter.BuildingBlocks.Application.Notifications;

public interface INotificationsHub
{
    void Subscribe(EventHandler<object> eventHandler);
    void Unsubscribe(EventHandler<object> eventHandler);


    Task NotifyAboutReadModelChanges<T>(T message) where T : class, IUiNotification;
}