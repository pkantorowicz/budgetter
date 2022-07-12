using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using MediatR;

namespace Budgetter.BuildingBlocks.Infrastructure.Processing;

public class UnitOfWorkCommandHandlerDecorator<T> : IRequestHandler<T, ICommandResult>
    where T : ICommand
{
    private readonly IRequestHandler<T, ICommandResult> _decorated;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        IRequestHandler<T, ICommandResult> decorated,
        IUnitOfWork unitOfWork)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICommandResult> Handle(T command, CancellationToken cancellationToken)
    {
        var commandResult = await _decorated.Handle(command, cancellationToken);

        await _unitOfWork.CommitTransactionAsync();

        return commandResult;
    }
}

public class UnitOfWorkCommandHandlerDecorator<T, TResult> : IRequestHandler<T, ICommandResult<TResult>>
    where T : ICommand<TResult>
{
    private readonly IRequestHandler<T, ICommandResult<TResult>> _decorated;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        IRequestHandler<T, ICommandResult<TResult>> decorated,
        IUnitOfWork unitOfWork)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICommandResult<TResult>> Handle(T command, CancellationToken cancellationToken)
    {
        var commandResult = await _decorated.Handle(command, cancellationToken);

        await _unitOfWork.CommitTransactionAsync();

        return commandResult;
    }
}