using System;
using System.Collections.Generic;

namespace Budgetter.Application.Targets.Dtos;

public record TargetDto
{
    public Guid Id { get; set; }
    public Guid BudgetPlanId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public bool IsRemoved { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public TargetStatusDto Status { get; set; }
    public TargetBalanceDto Balance { get; set; }
    public ICollection<TargetItemDto> TargetItems { get; set; }
}