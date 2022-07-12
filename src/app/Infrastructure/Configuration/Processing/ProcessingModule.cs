using Autofac;
using Budgetter.BuildingBlocks.Infrastructure.Processing;
using MediatR;

namespace Budgetter.Infrastructure.Configuration.Processing;

public class ProcessingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(IRequestHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<,>),
            typeof(IRequestHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(ValidationCommandHandlerDecorator<>),
            typeof(IRequestHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(ValidationCommandHandlerDecorator<,>),
            typeof(IRequestHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<>),
            typeof(IRequestHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<,>),
            typeof(IRequestHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(LoggingQueryHandlerDecorator<,>),
            typeof(IRequestHandler<,>));
    }
}