using System;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Exceptions;
using MediatR;
using Serilog;

namespace Budgetter.BuildingBlocks.Infrastructure.Processing;

public class LoggingCommandHandlerDecorator<T> : IRequestHandler<T, ICommandResult>
    where T : ICommand
{
    private readonly IRequestHandler<T, ICommandResult> _decorated;
    private readonly ILogger _logger;

    public LoggingCommandHandlerDecorator(
        IRequestHandler<T, ICommandResult> decorated,
        ILogger logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task<ICommandResult> Handle(T command, CancellationToken cancellationToken)
    {
        ICommandResult commandResult;

        try
        {
            _logger.Information($"Handling command: {command.GetType().Name} ({command}).");

            commandResult = await _decorated.Handle(command, cancellationToken);

            _logger.Information($"Command {command.GetType().Name} successfully handled.");
        }
        catch (InvalidCommandException invalidCommandException)
        {
            _logger.Error(
                $"Error occurred when processing command: {command.GetType().Name}, Ex: {invalidCommandException}");

            commandResult = CommandResult.Failed(
                invalidCommandException.Errors,
                invalidCommandException);
        }
        catch (Exception exception)
        {
            _logger.Error(
                $"Error occurred when processing command: {command.GetType().Name}, Ex: {exception}");

            commandResult = CommandResult.Failed(
                "Error occurred. Please see log for more details.",
                exception);
        }

        return commandResult;
    }
}

public class LoggingCommandHandlerDecorator<T, TResult> : IRequestHandler<T, ICommandResult<TResult>>
    where T : ICommand<TResult>
{
    private readonly IRequestHandler<T, ICommandResult<TResult>> _decorated;
    private readonly ILogger _logger;

    public LoggingCommandHandlerDecorator(
        IRequestHandler<T, ICommandResult<TResult>> decorated,
        ILogger logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task<ICommandResult<TResult>> Handle(T command, CancellationToken cancellationToken)
    {
        ICommandResult<TResult> commandResult;

        try
        {
            _logger.Information($"Handling command: {command.GetType().Name} ({command}).");

            commandResult = await _decorated.Handle(command, cancellationToken);

            _logger.Information($"Command {command.GetType().Name} successfully handled.");
        }
        catch (InvalidCommandException invalidCommandException)
        {
            commandResult = CommandResult<TResult>.Failed(
                invalidCommandException.Errors,
                invalidCommandException);

            _logger.Error(
                $"Error occurred when processing command: {command.GetType().Name}," +
                $" Details: {commandResult.FormatDetails()}");
        }
        catch (Exception exception)
        {
            commandResult = CommandResult<TResult>.Failed(
                "Error occurred. Please see log for more details.",
                exception);

            _logger.Error(
                $"Error occurred when processing command: {command.GetType().Name}," +
                $" Details: {commandResult.FormatDetails()}");
        }

        return commandResult;
    }
}