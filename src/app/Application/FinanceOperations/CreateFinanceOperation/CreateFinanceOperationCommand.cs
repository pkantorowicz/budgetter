using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.FinanceOperations.CreateFinanceOperation;

public record CreateFinanceOperationCommand : CommandBase<Guid>
{
    public CreateFinanceOperationCommand(
        Guid budgetPlanId,
        string title,
        decimal price,
        string currency,
        string planCurrency,
        DateTime planValidFrom,
        DateTime planValidTo,
        DateTime? occurredAt)
    {
        BudgetPlanId = budgetPlanId;
        Title = title;
        Price = price;
        Currency = currency;
        PlanCurrency = planCurrency;
        PlanValidFrom = planValidFrom;
        PlanValidTo = planValidTo;
        OccurredAt = occurredAt;
    }

    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public decimal Price { get; }
    public string Currency { get; }
    public string PlanCurrency { get; }
    public DateTime PlanValidFrom { get; }
    public DateTime PlanValidTo { get; }
    public DateTime? OccurredAt { get; }
}