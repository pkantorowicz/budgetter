using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.Targets.Dtos;

namespace Budgetter.Wpf.Controllers.Interfaces;

internal interface ITargetsController
{
    Task<IEnumerable<TargetDto>> GetTargets(
        Guid budgetPlanId,
        DateTime validFrom,
        DateTime validTo);

    Task<Guid> CreateTargetAsync(
        Guid budgetPlanId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        DateTime budgetPlanValidFrom,
        DateTime budgetPlanValidTo,
        decimal maxAmount,
        string currency);

    Task ChangeTargetAttributesAsync(
        Guid targetId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        DateTime budgetPlanValidFrom,
        DateTime budgetPlanValidTo,
        decimal maxAmount,
        string currency);

    Task AllocateExpenseAsync(
        Guid targetId,
        FinanceOperationDto financeOperation);

    Task DeallocateExpenseAsync(
        Guid targetId,
        Guid expenseId);

    Task ClearTargetAsync(
        Guid targetId);

    Task RemoveTargetAsync(Guid targetId);
}