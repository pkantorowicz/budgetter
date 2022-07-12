using System;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.Application.Commons.ReadModel;
using Budgetter.BuildingBlocks.Application.Projections;

namespace Budgetter.Application.FinanceOperations.Projections.ReadModel;

public class FinanceOperationReadModel : IReadModel
{
    public Guid BudgetPlanId { get; set; }
    public BudgetPlanReadModel BudgetPlan { get; set; }
    public string Title { get; set; }
    public PriceReadModel Price { get; set; }
    public DateTime OccurredAt { get; set; }
    public bool IsRemoved { get; set; }
    public Guid TargetId { get; set; }
    public bool IsAllocated { get; set; }
    public Guid Id { get; set; }
}