using System;
using System.Collections.Generic;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.Targets.Dtos;

namespace Budgetter.Application.BudgetPlans.Dtos;

public record BudgetPlanDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Currency { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsRemoved { get; set; }
    public ICollection<FinanceOperationDto> FinanceOperations { get; set; }
    public ICollection<TargetDto> Targets { get; set; }
}