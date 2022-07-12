using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.BudgetPlans.RemoveBudgetPlan;

internal class RemoveBudgetPlanCommandValidator : CommandValidatorBase<RemoveBudgetPlanCommand>
{
    public RemoveBudgetPlanCommandValidator()
    {
        RuleFor(c => c.BudgetPlanId).NotEmpty();
    }
}