using System;
using System.Collections.Generic;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.Application.Commons.ReadModel;
using Budgetter.BuildingBlocks.Application.Projections;

namespace Budgetter.Application.Targets.Projections.ReadModel;

public class TargetReadModel : IReadModel
{
    public TargetReadModel()
    {
        TargetItems = new List<TargetItemReadModel>();
    }

    public Guid BudgetPlanId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PriceReadModel MaxAmount { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public BudgetPlanReadModel BudgetPlan { get; set; }
    public ICollection<TargetItemReadModel> TargetItems { get; }
    public bool IsRemoved { get; set; }
    public Guid Id { get; set; }
}