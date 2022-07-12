using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.Targets.ChangeTargetAttributes;

public record ChangeTargetAttributesCommand : CommandBase
{
    public ChangeTargetAttributesCommand(
        Guid targetId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        DateTime planValidFrom,
        DateTime planValidTo,
        decimal maxAmount,
        string currency)
    {
        TargetId = targetId;
        Title = title;
        Description = description;
        ValidFrom = validFrom;
        ValidTo = validTo;
        PlanValidFrom = planValidFrom;
        PlanValidTo = planValidTo;
        MaxAmount = maxAmount;
        Currency = currency;
    }

    public Guid TargetId { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }
    public DateTime PlanValidFrom { get; }
    public DateTime PlanValidTo { get; }
    public decimal MaxAmount { get; }
    public string Currency { get; }
}