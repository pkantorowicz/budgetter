using System;

namespace Budgetter.Application.Targets.Dtos;

public record TargetItemDto
{
    public Guid Id { get; set; }
    public Guid TargetId { get; set; }
    public Guid BudgetPlanId { get; set; }
    public string Title { get; set; }
    public decimal UnitPrice { get; set; }
    public string Currency { get; set; }
    public DateTime OccurredAt { get; set; }
}