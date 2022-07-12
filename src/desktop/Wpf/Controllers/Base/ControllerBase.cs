using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Execution;
using Budgetter.Infrastructure.Configuration;

namespace Budgetter.Wpf.Controllers.Base;

internal class ControllerBase
{
    private readonly IExecutor _executor;

    protected ControllerBase()
    {
        _executor = BudgetterCompositionRoot.Resolve<IExecutor>();
    }

    protected async Task SendAsync(ICommand command)
    {
        var commandResult = await _executor.ExecuteCommandAsync(command);

        if (!commandResult.Ok)
            ValidateResult(commandResult);
    }

    protected async Task<TResult> SendAsync<TResult>(ICommand<TResult> command)
    {
        var commandResult = await _executor.ExecuteCommandAsync(command);

        if (!commandResult.Ok)
            ValidateResult(commandResult);

        return commandResult.Data;
    }

    protected async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        var queryResult = await _executor.ExecuteQueryAsync(query);

        if (!queryResult.Ok)
            ValidateResult(queryResult);

        return queryResult.Data;
    }

    private static void ValidateResult(IResult result)
    {
        throw new RequestFailedException(
            result.Message ?? string.Join(",\n", result.Errors),
            result.FormatDetails(),
            result.Exception);
    }
}