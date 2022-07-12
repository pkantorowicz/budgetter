using System;
using System.Collections.Generic;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.Application.Targets.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Projections;

namespace Budgetter.Application.BudgetPlans.Projections.ReadModel;

public class BudgetPlanReadModel : IReadModel
{
    public BudgetPlanReadModel()
    {
        FinanceOperations = new List<FinanceOperationReadModel>();
        Targets = new List<TargetReadModel>();
    }

    public string Title { get; set; }
    public string Currency { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public ICollection<FinanceOperationReadModel> FinanceOperations { get; set; }
    public ICollection<TargetReadModel> Targets { get; set; }
    public bool IsRemoved { get; set; }
    public Guid Id { get; set; }
}