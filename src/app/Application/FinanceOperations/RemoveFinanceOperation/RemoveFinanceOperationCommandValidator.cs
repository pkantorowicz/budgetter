using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.FinanceOperations.RemoveFinanceOperation;

public class RemoveFinanceOperationCommandValidator : CommandValidatorBase<RemoveFinanceOperationCommand>
{
    public RemoveFinanceOperationCommandValidator()
    {
        RuleFor(c => c.FinanceOperationId).NotEmpty();
    }
}