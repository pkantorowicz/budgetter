using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.Targets.AllocateExpense;
using Budgetter.Application.Targets.ChangeTargetAttributes;
using Budgetter.Application.Targets.ClearTarget;
using Budgetter.Application.Targets.CreateTarget;
using Budgetter.Application.Targets.DeallocateExpense;
using Budgetter.Application.Targets.Dtos;
using Budgetter.Application.Targets.GetTargets;
using Budgetter.Application.Targets.RemoveTarget;
using Budgetter.Wpf.Controllers.Base;
using Budgetter.Wpf.Controllers.Interfaces;

namespace Budgetter.Wpf.Controllers;

internal class TargetController : ControllerBase, ITargetsController
{
    public async Task<IEnumerable<TargetDto>> GetTargets(
        Guid budgetPlanId,
        DateTime validFrom,
        DateTime validTo)
    {
        return await QueryAsync(
            new GetTargetsQuery(
                budgetPlanId,
                validFrom,
                validTo));
    }

    public async Task<Guid> CreateTargetAsync(
        Guid budgetPlanId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        DateTime budgetPlanValidFrom,
        DateTime budgetPlanValidTo,
        decimal maxAmount,
        string currency)
    {
        return await SendAsync(
            new CreateTargetCommand(
                budgetPlanId,
                title,
                description,
                validFrom,
                validTo,
                budgetPlanValidFrom,
                budgetPlanValidTo,
                maxAmount,
                currency));
    }

    public async Task ChangeTargetAttributesAsync(
        Guid targetId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        DateTime budgetPlanValidFrom,
        DateTime budgetPlanValidTo,
        decimal maxAmount,
        string currency)
    {
        await SendAsync(
            new ChangeTargetAttributesCommand(
                targetId,
                title,
                description,
                validFrom,
                validTo,
                budgetPlanValidFrom,
                budgetPlanValidTo,
                maxAmount,
                currency));
    }

    public async Task AllocateExpenseAsync(
        Guid targetId,
        FinanceOperationDto financeOperation)
    {
        await SendAsync(
            new AllocateExpenseCommand(
                targetId,
                financeOperation));
    }

    public async Task DeallocateExpenseAsync(
        Guid targetId,
        Guid expenseId)
    {
        await SendAsync(
            new DeallocateExpenseCommand(
                targetId,
                expenseId));
    }

    public async Task ClearTargetAsync(
        Guid targetId)
    {
        await SendAsync(
            new ClearTargetCommand(
                targetId));
    }

    public async Task RemoveTargetAsync(Guid targetId)
    {
        await SendAsync(new RemoveTargetCommand(targetId));
    }
}