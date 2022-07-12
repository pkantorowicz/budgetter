using System;
using System.Threading.Tasks;
using Budgetter.Application.Commons;
using Budgetter.BuildingBlocks.Application.Contacts.Notifications;
using Budgetter.BuildingBlocks.Application.Notifications;
using Budgetter.BuildingBlocks.Application.Projections;
using Budgetter.BuildingBlocks.Domain.DomainEvents;

namespace Budgetter.Application.Configuration;

public abstract class ProjectorBase
{
    private readonly INotificationsHub _notificationsHub;

    protected ProjectorBase(INotificationsHub notificationsHub)
    {
        _notificationsHub = notificationsHub;
    }

    protected static Task When(IDomainEvent @event)
    {
        return Task.CompletedTask;
    }

    protected async Task NotifyAboutSuccess<TReadModel, TDomainEvent>(
        TReadModel readModel,
        TDomainEvent @event)
        where TReadModel : class, IReadModel
        where TDomainEvent : class, IDomainEvent
    {
        await _notificationsHub.NotifyAboutReadModelChanges(
            UiNotification<TReadModel>.Success(
                readModel,
                @event
                    .GetType()
                    .GetOperationName()));
    }

    protected async Task NotifyAboutFailed<TReadModel, TDomainEvent>(
        Exception exception,
        TDomainEvent @event)
        where TReadModel : class, IReadModel
        where TDomainEvent : class, IDomainEvent
    {
        await _notificationsHub.NotifyAboutReadModelChanges(
            UiNotification<TReadModel>
                .Failed(@event.GetType().GetOperationName())
                .AddError(
                    exception.GetType().Name,
                    exception.Message));
    }
}