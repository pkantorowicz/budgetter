using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;

namespace Budgetter.BuildingBlocks.Application.Execution;

public interface IExecutor
{
    Task<ICommandResult> ExecuteCommandAsync(ICommand command);
    Task<ICommandResult<TResult>> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
    Task<IQueryResult<TResult>> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}