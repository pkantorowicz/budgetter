using System;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using MediatR;
using Serilog;

namespace Budgetter.BuildingBlocks.Infrastructure.Processing;

public class LoggingQueryHandlerDecorator<TQuery, TResult> : IRequestHandler<TQuery, IQueryResult<TResult>>
    where TQuery : IQuery<TResult>
{
    private readonly IRequestHandler<TQuery, IQueryResult<TResult>> _decorated;
    private readonly ILogger _logger;

    public LoggingQueryHandlerDecorator(
        IRequestHandler<TQuery, IQueryResult<TResult>> decorated,
        ILogger logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task<IQueryResult<TResult>> Handle(TQuery query, CancellationToken cancellationToken)
    {
        IQueryResult<TResult> queryResult;

        try
        {
            _logger.Information($"Handling query: {query.GetType().Name} ({query}).");

            queryResult = await _decorated.Handle(query, cancellationToken);

            _logger.Information($"Query {query.GetType().Name} successfully handled.");
        }
        catch (Exception exception)
        {
            queryResult = QueryResult<TResult>.Failed(
                "Error occurred. Please see log for more details.",
                exception);

            _logger.Error(
                $"Error occurred when processing query: {query.GetType().Name}, " +
                $"Details: {queryResult.FormatDetails()}");
        }

        return queryResult;
    }
}