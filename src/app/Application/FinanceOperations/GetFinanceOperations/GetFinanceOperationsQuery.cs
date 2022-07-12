using System;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.BuildingBlocks.Application.Contacts.Queries.Pagination;
using Budgetter.BuildingBlocks.Application.Contacts.Results;

namespace Budgetter.Application.FinanceOperations.GetFinanceOperations;

public record GetFinanceOperationsQuery : PagedQueryBase<PagedResult<FinanceOperationDto>>
{
    public GetFinanceOperationsQuery(
        Guid budgetPlanId,
        int page,
        int results,
        string orderBy,
        SortDirection sortDirection)
        : base(
            page,
            results,
            orderBy,
            sortDirection)
    {
        BudgetPlanId = budgetPlanId;
    }

    public Guid BudgetPlanId { get; }
}