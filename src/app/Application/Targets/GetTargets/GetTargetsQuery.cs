using System;
using System.Collections.Generic;
using Budgetter.Application.Targets.Dtos;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;

namespace Budgetter.Application.Targets.GetTargets;

public record GetTargetsQuery : QueryBase<IEnumerable<TargetDto>>
{
    public GetTargetsQuery(
        Guid budgetPlanId,
        DateTime validFrom,
        DateTime validTo)
    {
        BudgetPlanId = budgetPlanId;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public Guid BudgetPlanId { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }
}