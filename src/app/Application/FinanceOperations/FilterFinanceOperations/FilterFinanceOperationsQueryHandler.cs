using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Extensions;

namespace Budgetter.Application.FinanceOperations.FilterFinanceOperations;

internal class
    FilterFinanceOperationsQueryHandler : QueryHandlerBase<FilterFinanceOperationsQuery,
        IEnumerable<FinanceOperationDto>>
{
    private readonly IFinanceOperationReadModelAccessor _financeOperationReadModelAccessor;

    public FilterFinanceOperationsQueryHandler(
        IMapper mapper,
        IFinanceOperationReadModelAccessor financeOperationReadModelAccessor) : base(mapper)
    {
        _financeOperationReadModelAccessor = financeOperationReadModelAccessor;
    }

    public override async Task<IQueryResult<IEnumerable<FinanceOperationDto>>> Handle(
        FilterFinanceOperationsQuery query, CancellationToken cancellationToken)
    {
        var budgetPlanMustBeTheSame = BudgetPlanMustBeTheSame(query.BudgetPlanId);
        var cannotBeRemoved = CannotBeRemoved();
        var titleFilter = ContainsInTitle(query.Title);
        var pricesFilter = PriceBetweenValues(query.MinPrice, query.MaxPrice);
        var datesFilter = OccurredBetweenDates(query.StartDate, query.EndDate);
        var mustBeAssigned = MustBeAssigned(query.IsAssigned);

        var financeOperations = await _financeOperationReadModelAccessor
            .FilterAsync(budgetPlanMustBeTheSame
                .And(cannotBeRemoved)
                .And(titleFilter)
                .And(datesFilter)
                .And(pricesFilter)
                .And(mustBeAssigned));

        var financeOperationsAsList = financeOperations.ToList();

        var financeOperationsDto =
            Mapper.Map<IEnumerable<FinanceOperationReadModel>, IEnumerable<FinanceOperationDto>>(
                financeOperationsAsList);

        return QueryResult<IEnumerable<FinanceOperationDto>>.Success(financeOperationsDto);
    }

    private static Expression<Func<FinanceOperationReadModel, bool>> BudgetPlanMustBeTheSame(Guid budgetPlanId)
    {
        return fo => fo.BudgetPlanId == budgetPlanId;
    }

    private static Expression<Func<FinanceOperationReadModel, bool>> CannotBeRemoved()
    {
        return fo => fo.IsRemoved == false;
    }
    
    private static Expression<Func<FinanceOperationReadModel, bool>> MustBeAssigned(bool? mustBeAssigned)
    {
        if (mustBeAssigned is null)
            return fo => true;
        
        return fo => fo.IsAllocated == mustBeAssigned;
    }

    private static Expression<Func<FinanceOperationReadModel, bool>> ContainsInTitle(string keyword)
    {
        return !string.IsNullOrEmpty(keyword) && keyword.Length >= 3
            ? fo => fo.Title.Contains(keyword)
            : fo => true;
    }

    private static Expression<Func<FinanceOperationReadModel, bool>> PriceBetweenValues(
        decimal? minPrice,
        decimal? maxPrice)
    {
        var filterType = SpecifyFilterType(minPrice, maxPrice);

        return filterType switch
        {
            BetweenFilterType.AlwaysTrue => fo => true,
            BetweenFilterType.OnlyRight => fo => fo.Price.Value >= minPrice,
            BetweenFilterType.OnlyLeft => fo => fo.Price.Value <= maxPrice,
            BetweenFilterType.Both => fo => fo.Price.Value >= minPrice && fo.Price.Value <= maxPrice,
            _ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, "Unknown filter type.")
        };
    }

    private static Expression<Func<FinanceOperationReadModel, bool>> OccurredBetweenDates(
        DateTime? dateStart,
        DateTime? dateEnd)
    {
        var filterType = SpecifyFilterType(dateStart, dateEnd);

        return filterType switch
        {
            BetweenFilterType.AlwaysTrue => fo => true,
            BetweenFilterType.OnlyRight => fo => fo.OccurredAt >= dateStart,
            BetweenFilterType.OnlyLeft => fo => fo.OccurredAt <= dateEnd,
            BetweenFilterType.Both => fo => fo.OccurredAt >= dateStart && fo.OccurredAt <= dateEnd,
            _ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, "Unknown filter type.")
        };
    }

    private static BetweenFilterType SpecifyFilterType(object value1, object value2)
    {
        if (value1 != null && value2 != null)
            return BetweenFilterType.Both;
        if (value1 != null)
            return BetweenFilterType.OnlyRight;
        if (value2 != null)
            return BetweenFilterType.OnlyLeft;

        return BetweenFilterType.AlwaysTrue;
    }

    private enum BetweenFilterType
    {
        AlwaysTrue,
        OnlyRight,
        OnlyLeft,
        Both
    }
}