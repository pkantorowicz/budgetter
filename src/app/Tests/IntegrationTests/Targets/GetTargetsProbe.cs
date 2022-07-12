using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.Targets.Dtos;
using Budgetter.Application.Targets.GetTargets;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Execution;
using Budgetter.BuildingBlocks.Testes.IntegrationTests.Probing;

namespace Budgetter.IntegrationTests.Targets;

public class GetTargetsProbe : IProbe<IQueryResult<IEnumerable<TargetDto>>>
{
    private readonly Func<IQueryResult<IEnumerable<TargetDto>>, bool> _condition;
    private readonly IExecutor _executor;
    private readonly Guid _planId;

    public GetTargetsProbe(
        IExecutor executor,
        Func<IQueryResult<IEnumerable<TargetDto>>, bool> condition,
        Guid planId)
    {
        _executor = executor;
        _condition = condition;
        _planId = planId;
    }

    public bool IsSatisfied(IQueryResult<IEnumerable<TargetDto>> sample)
    {
        return sample != null && _condition(sample);
    }

    public async Task<IQueryResult<IEnumerable<TargetDto>>> GetSampleAsync()
    {
        return await _executor.ExecuteQueryAsync(
            new GetTargetsQuery(
                _planId,
                DateTime.UtcNow.AddDays(-25),
                DateTime.UtcNow.AddDays(25)));
    }

    public string DescribeFailureTo()
    {
        return "Cannot get targets.";
    }
}