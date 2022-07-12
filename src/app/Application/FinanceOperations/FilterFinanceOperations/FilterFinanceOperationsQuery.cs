using System;
using System.Collections.Generic;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;

namespace Budgetter.Application.FinanceOperations.FilterFinanceOperations;

public record FilterFinanceOperationsQuery : QueryBase<IEnumerable<FinanceOperationDto>>
{
    public FilterFinanceOperationsQuery(
        Guid budgetPlanId,
        string title,
        decimal? minPrice,
        decimal? maxPrice,
        DateTime? startDate,
        DateTime? endDate, 
        bool? isAssigned)
    {
        BudgetPlanId = budgetPlanId;
        Title = title;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        StartDate = startDate;
        EndDate = endDate;
        IsAssigned = isAssigned;
    }

    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public decimal? MinPrice { get; }
    public decimal? MaxPrice { get; }
    public DateTime? StartDate { get; }
    public DateTime? EndDate { get; }
    public bool? IsAssigned { get; }
}