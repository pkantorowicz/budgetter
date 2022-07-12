using System;
using System.Threading.Tasks;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.FinanceOperations.GetFinanceOperations;
using Budgetter.BuildingBlocks.Application.Contacts.Queries.Pagination;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Execution;
using Budgetter.BuildingBlocks.Testes.IntegrationTests.Probing;

namespace Budgetter.IntegrationTests.FinanceOperations;

public class GetFinanceOperationProbe : IProbe<IQueryResult<PagedResult<FinanceOperationDto>>>
{
    private readonly Func<IQueryResult<PagedResult<FinanceOperationDto>>, bool> _condition;
    private readonly IExecutor _executor;
    private readonly Guid _planId;

    public GetFinanceOperationProbe(
        IExecutor executor,
        Func<IQueryResult<PagedResult<FinanceOperationDto>>, bool> condition,
        Guid planId)
    {
        _executor = executor;
        _condition = condition;
        _planId = planId;
    }

    public bool IsSatisfied(IQueryResult<PagedResult<FinanceOperationDto>> sample)
    {
        return sample != null && _condition(sample);
    }

    public async Task<IQueryResult<PagedResult<FinanceOperationDto>>> GetSampleAsync()
    {
        return await _executor.ExecuteQueryAsync(
            new GetFinanceOperationsQuery(
                _planId,
                1,
                10,
                nameof(FinanceOperationDto.OccurredAt),
                SortDirection.Ascending));
    }

    public string DescribeFailureTo()
    {
        return "Cannot get finance operations.";
    }
}