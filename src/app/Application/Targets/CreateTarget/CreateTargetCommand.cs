using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.Targets.CreateTarget;

public record CreateTargetCommand : CommandBase<Guid>
{
    public CreateTargetCommand(
        Guid budgetPlanId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        DateTime planValidFrom,
        DateTime planValidTo,
        decimal maxAmount,
        string currency)
    {
        BudgetPlanId = budgetPlanId;
        Title = title;
        Description = description;
        ValidFrom = validFrom;
        ValidTo = validTo;
        PlanValidFrom = planValidFrom;
        PlanValidTo = planValidTo;
        MaxAmount = maxAmount;
        Currency = currency;
    }

    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }
    public DateTime PlanValidFrom { get; }
    public DateTime PlanValidTo { get; }
    public decimal MaxAmount { get; }
    public string Currency { get; }
}