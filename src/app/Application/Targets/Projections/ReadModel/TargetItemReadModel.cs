using System;
using Budgetter.Application.Commons.ReadModel;
using Budgetter.BuildingBlocks.Application.Projections;

namespace Budgetter.Application.Targets.Projections.ReadModel;

public class TargetItemReadModel : IReadModel
{
    public Guid TargetId { get; set; }
    public Guid BudgetPlanId { get; set; }
    public string Title { get; set; }
    public PriceReadModel UnitPrice { get; set; }
    public DateTime OccurredAt { get; set; }
    public TargetReadModel Target { get; set; }
    public Guid Id { get; set; }
}