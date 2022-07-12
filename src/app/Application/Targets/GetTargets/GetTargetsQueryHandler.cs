using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.Targets.Dtos;
using Budgetter.Application.Targets.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;

namespace Budgetter.Application.Targets.GetTargets;

internal class GetTargetsQueryHandler : QueryHandlerBase<GetTargetsQuery, IEnumerable<TargetDto>>
{
    private readonly ITargetReadModelAccessor _targetReadModelAccessor;

    public GetTargetsQueryHandler(
        IMapper mapper,
        ITargetReadModelAccessor targetReadModelAccessor) : base(mapper)
    {
        _targetReadModelAccessor = targetReadModelAccessor;
    }

    public override async Task<IQueryResult<IEnumerable<TargetDto>>> Handle(GetTargetsQuery query,
        CancellationToken cancellationToken)
    {
        var targets = await _targetReadModelAccessor
            .FindAsync(t =>
                t.BudgetPlanId == query.BudgetPlanId &&
                t.ValidFrom >= query.ValidFrom &&
                t.ValidTo <= query.ValidTo &&
                t.ValidTo <= query.ValidTo &&
                t.IsRemoved == false);

        var expenseDtos = Mapper.Map<IEnumerable<TargetReadModel>, IEnumerable<TargetDto>>(targets);

        return QueryResult<IEnumerable<TargetDto>>.Success(expenseDtos);
    }
}