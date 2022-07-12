using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.Targets.DeallocateExpense;

internal class DeallocateExpenseCommandValidator : CommandValidatorBase<DeallocateExpenseCommand>
{
    public DeallocateExpenseCommandValidator()
    {
        RuleFor(command => command.TargetId)
            .NotEmpty()
            .WithMessage("Target id can not be empty.");

        RuleFor(command => command.ExpenseId)
            .NotEmpty()
            .WithMessage("Expense id can not be empty.");
    }
}