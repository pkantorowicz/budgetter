using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Budgetter.BuildingBlocks.Infrastructure.Processing;

public class ValidationCommandHandlerDecorator<T> : IRequestHandler<T, ICommandResult>
    where T : ICommand
{
    private readonly IRequestHandler<T, ICommandResult> _decorated;
    private readonly IList<IValidator<T>> _validators;

    public ValidationCommandHandlerDecorator(
        IList<IValidator<T>> validators,
        IRequestHandler<T, ICommandResult> decorated)
    {
        _validators = validators;
        _decorated = decorated;
    }

    public async Task<ICommandResult> Handle(T command, CancellationToken cancellationToken)
    {
        var errors = _validators
            .Select(v => v.Validate(command))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Any())
            throw new InvalidCommandException(errors.ToDictionary(x => x.ErrorCode, x => x.ErrorMessage));

        return await _decorated.Handle(command, cancellationToken);
    }
}

public class ValidationCommandHandlerDecorator<T, TResult> : IRequestHandler<T, ICommandResult<TResult>>
    where T : ICommand<TResult>
{
    private readonly IRequestHandler<T, ICommandResult<TResult>> _decorated;
    private readonly IList<IValidator<T>> _validators;

    public ValidationCommandHandlerDecorator(
        IList<IValidator<T>> validators,
        IRequestHandler<T, ICommandResult<TResult>> decorated)
    {
        _validators = validators;
        _decorated = decorated;
    }

    public async Task<ICommandResult<TResult>> Handle(T command, CancellationToken cancellationToken)
    {
        var errors = _validators
            .Select(v => v.Validate(command))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Any())
            throw new InvalidCommandException(errors.ToDictionary(x => x.ErrorCode, x => x.ErrorMessage));

        return await _decorated.Handle(command, cancellationToken);
    }
}