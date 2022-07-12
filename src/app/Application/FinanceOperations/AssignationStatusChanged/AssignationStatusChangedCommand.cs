using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.FinanceOperations.AssignationStatusChanged;

public record AssignationStatusChangedCommand : CommandBase
{
    public AssignationStatusChangedCommand(
        Guid financeOperationId,
        Guid targetId,
        bool assignationStatus)
    {
        FinanceOperationId = financeOperationId;
        TargetId = targetId;
        AssignationStatus = assignationStatus;
    }

    public Guid FinanceOperationId { get; }
    public Guid TargetId { get; }
    public bool AssignationStatus { get; }
}