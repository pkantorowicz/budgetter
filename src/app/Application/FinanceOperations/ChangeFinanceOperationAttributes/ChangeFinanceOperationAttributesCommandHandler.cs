using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Exceptions;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.FinanceOperations;

namespace Budgetter.Application.FinanceOperations.ChangeFinanceOperationAttributes;

internal class
    ChangeFinanceOperationAttributesCommandHandler : CommandHandlerBase<ChangeFinanceOperationAttributesCommand>
{
    public ChangeFinanceOperationAttributesCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult> Handle(ChangeFinanceOperationAttributesCommand command,
        CancellationToken cancellationToken)
    {
        var financeOperation = await AggregateStore.Load(
            FinanceOperationId.New(
                command.FinanceOperationId));

        if (financeOperation is null)
            throw new RecordNotFoundException(
                $"Finance operation with provided id: {command.FinanceOperationId} doesn't exists.");

        financeOperation.ChangeAttributes(
            Title.Entitle(command.Title),
            command.Price);

        AggregateStore.AppendChanges(financeOperation);

        return CommandResult.Success();
    }
}