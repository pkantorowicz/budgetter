using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.FinanceOperations.RemoveFinanceOperation;

public record RemoveFinanceOperationCommand : CommandBase
{
    public RemoveFinanceOperationCommand(Guid financeOperationId)
    {
        FinanceOperationId = financeOperationId;
    }

    public Guid FinanceOperationId { get; }
}