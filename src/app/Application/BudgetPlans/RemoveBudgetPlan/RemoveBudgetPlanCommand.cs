using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.BudgetPlans.RemoveBudgetPlan;

public record RemoveBudgetPlanCommand : CommandBase
{
    public RemoveBudgetPlanCommand(Guid budgetPlanId)
    {
        BudgetPlanId = budgetPlanId;
    }

    public Guid BudgetPlanId { get; }
}