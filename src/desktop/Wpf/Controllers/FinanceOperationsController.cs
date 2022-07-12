using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.FinanceOperations.AssignationStatusChanged;
using Budgetter.Application.FinanceOperations.ChangeFinanceOperationAttributes;
using Budgetter.Application.FinanceOperations.CreateFinanceOperation;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.FinanceOperations.FilterFinanceOperations;
using Budgetter.Application.FinanceOperations.RemoveFinanceOperation;
using Budgetter.Wpf.Controllers.Base;
using Budgetter.Wpf.Controllers.Interfaces;

namespace Budgetter.Wpf.Controllers;

internal class FinanceOperationsController : ControllerBase, IFinanceOperationsController
{
    public async Task<IEnumerable<FinanceOperationDto>> FilterFinanceOperationsAsync(
        Guid budgetPlanId,
        string titleKeyword,
        decimal? minPrice,
        decimal? maxPrice,
        DateTime? dateStart,
        DateTime? dateEnd,
        bool? onlyAssigned)
    {
        var filteredFinanceOperations = await QueryAsync(
            new FilterFinanceOperationsQuery(
                budgetPlanId,
                titleKeyword,
                minPrice,
                maxPrice,
                dateStart,
                dateEnd,
                onlyAssigned));

        return filteredFinanceOperations;
    }

    public async Task<Guid> CreateFinanceOperation(
        Guid budgetPlanId,
        string description,
        decimal price,
        string currency,
        string planCurrency,
        DateTime? planValidFrom,
        DateTime? planValidTo,
        DateTime occurredAt)
    {
        var financeOperationId = await SendAsync(
            new CreateFinanceOperationCommand(
                budgetPlanId,
                description,
                price,
                currency,
                planCurrency,
                planValidFrom ?? DateTime.MinValue,
                planValidTo ?? DateTime.MaxValue,
                occurredAt));

        return financeOperationId;
    }

    public async Task ChangeFinanceOperationAttributes(
        Guid financeOperationId,
        string description,
        decimal price)
    {
        await SendAsync(
            new ChangeFinanceOperationAttributesCommand(
                financeOperationId,
                description,
                price));
    }

    public async Task ChangeFinanceOperationAssignationStatus(
        Guid financeOperationId,
        Guid targetId,
        bool assignationStatus)
    {
        await SendAsync(
            new AssignationStatusChangedCommand(
                financeOperationId,
                targetId,
                assignationStatus));
    }

    public async Task RemoveFinanceOperation(Guid financeOperationId)
    {
        await SendAsync(new RemoveFinanceOperationCommand(financeOperationId));
    }
}