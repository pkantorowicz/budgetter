using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;

namespace Budgetter.Application.BudgetPlans.GetBudgetPlans;

internal class GetBudgetPlansQueryHandler : QueryHandlerBase<GetBudgetPlansQuery, IEnumerable<BudgetPlanDto>>
{
    private readonly IBudgetPlanReadModelAccessor _budgetPlanReadModelAccessor;

    public GetBudgetPlansQueryHandler(
        IMapper mapper,
        IBudgetPlanReadModelAccessor budgetPlanReadModelAccessor) : base(mapper)
    {
        _budgetPlanReadModelAccessor = budgetPlanReadModelAccessor;
    }

    public override async Task<IQueryResult<IEnumerable<BudgetPlanDto>>> Handle(GetBudgetPlansQuery query,
        CancellationToken cancellationToken)
    {
        var budgetPlanReadModels = await _budgetPlanReadModelAccessor.GetAllAsync();
        var budgetPlanDtos =
            Mapper.Map<IEnumerable<BudgetPlanReadModel>, IEnumerable<BudgetPlanDto>>(budgetPlanReadModels);

        return QueryResult<IEnumerable<BudgetPlanDto>>.Success(budgetPlanDtos);
    }
}