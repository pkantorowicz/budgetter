using System.Threading.Tasks;
using Autofac;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Execution;
using MediatR;

namespace Budgetter.Infrastructure.Configuration;

public class Executor : IExecutor
{
    private readonly ILifetimeScope _lifetimeScope;

    public Executor(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public async Task<ICommandResult> ExecuteCommandAsync(ICommand command)
    {
        await using var scope = _lifetimeScope.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        var result = await mediator.Send(command);

        return result;
    }

    public async Task<ICommandResult<TResult>> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        await using var scope = _lifetimeScope.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        var result = await mediator.Send(command);

        return result;
    }

    public async Task<IQueryResult<TResult>> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        await using var scope = _lifetimeScope.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        var result = await mediator.Send(query);

        return result;
    }
}