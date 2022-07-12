using Autofac;
using Budgetter.BuildingBlocks.Application.Notifications;

namespace Budgetter.Infrastructure.Configuration.Notifications;

public class NotificationsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<NotificationsHub>()
            .As<INotificationsHub>()
            .SingleInstance();
    }
}