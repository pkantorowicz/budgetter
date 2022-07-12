using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Exceptions;

namespace Budgetter.Application.BudgetPlans.GetBudgetPlan;

internal class GetBudgetPlanQueryHandler : QueryHandlerBase<GetBudgetPlanQuery, BudgetPlanDto>
{
    private readonly IBudgetPlanReadModelAccessor _budgetPlanReadModelAccessor;

    public GetBudgetPlanQueryHandler(
        IMapper mapper, IBudgetPlanReadModelAccessor budgetPlanReadModelAccessor) : base(mapper)
    {
        _budgetPlanReadModelAccessor = budgetPlanReadModelAccessor;
    }

    public override async Task<IQueryResult<BudgetPlanDto>> Handle(GetBudgetPlanQuery query,
        CancellationToken cancellationToken)
    {
        var budgetPlanReadModel = await _budgetPlanReadModelAccessor
            .GetByIdAsync(
                query.BudgetPlanId);

        if (budgetPlanReadModel is null)
            throw new RecordNotFoundException(
                "Unable to update read model," +
                $" because budget plan with id: {query.BudgetPlanId} doesn't exists.");

        var budgetPlanDto = Mapper.Map<BudgetPlanReadModel, BudgetPlanDto>(budgetPlanReadModel);

        return QueryResult<BudgetPlanDto>.Success(budgetPlanDto);
    }
}