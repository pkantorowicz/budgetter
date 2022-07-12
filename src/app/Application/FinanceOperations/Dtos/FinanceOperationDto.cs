using System;

namespace Budgetter.Application.FinanceOperations.Dtos;

public record FinanceOperationDto
{
    public Guid Id { get; set; }
    public Guid TargetId { get; set; }
    public bool IsAllocated { get; set; }
    public Guid BudgetPlanId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public FinanceOperationTypeDto Type { get; set; }
    public DateTime OccurredAt { get; set; }
    public bool IsRemoved { get; set; }
}