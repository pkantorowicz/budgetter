using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Queries.Pagination;
using Budgetter.BuildingBlocks.Application.Contacts.Results;

namespace Budgetter.Application.FinanceOperations.GetFinanceOperations;

internal class
    GetFinanceOperationsQueryHandler : QueryHandlerBase<GetFinanceOperationsQuery, PagedResult<FinanceOperationDto>>
{
    private readonly IFinanceOperationReadModelAccessor _financeOperationReadModelAccessor;

    public GetFinanceOperationsQueryHandler(
        IFinanceOperationReadModelAccessor financeOperationReadModelAccessor,
        IMapper mapper) : base(mapper)
    {
        _financeOperationReadModelAccessor = financeOperationReadModelAccessor;
    }

    public override async Task<IQueryResult<PagedResult<FinanceOperationDto>>> Handle(
        GetFinanceOperationsQuery query,
        CancellationToken cancellationToken)
    {
        var financeOperations = await _financeOperationReadModelAccessor
            .GetAllAsync(
                query.BudgetPlanId);

        var financeOperationDtos =
            Mapper.Map<IEnumerable<FinanceOperationReadModel>, IEnumerable<FinanceOperationDto>>(financeOperations);

        var paginatedFinanceOperationDtos = financeOperationDtos.Paginate(query);

        return QueryResult<PagedResult<FinanceOperationDto>>.Success(paginatedFinanceOperationDtos);
    }
}