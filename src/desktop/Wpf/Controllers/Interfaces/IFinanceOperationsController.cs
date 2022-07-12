using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.FinanceOperations.Dtos;

namespace Budgetter.Wpf.Controllers.Interfaces;

internal interface IFinanceOperationsController
{
    Task<IEnumerable<FinanceOperationDto>> FilterFinanceOperationsAsync(
        Guid budgetPlanId,
        string titleKeyword,
        decimal? minPrice,
        decimal? maxPrice,
        DateTime? dateStart,
        DateTime? dateEnd,
        bool? onlyAssigned);

    Task<Guid> CreateFinanceOperation(
        Guid budgetPlanId,
        string description,
        decimal price,
        string currency,
        string planCurrency,
        DateTime? planValidFrom,
        DateTime? planValidTo,
        DateTime occurredAt);

    Task ChangeFinanceOperationAttributes(
        Guid financeOperationId,
        string description,
        decimal price);

    Task ChangeFinanceOperationAssignationStatus(
        Guid financeOperationId,
        Guid targetId,
        bool assignationStatus);
    
    Task RemoveFinanceOperation(
        Guid financeOperationId);
}