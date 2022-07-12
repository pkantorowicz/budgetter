using FluentValidation;

namespace Budgetter.BuildingBlocks.Application.Contacts.Commands;

public abstract class CommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : CommandBase
{
    protected CommandValidatorBase()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.When).NotEmpty();
    }
}

public abstract class CommandValidatorBase<TCommand, TResult> : AbstractValidator<TCommand>
    where TCommand : CommandBase<TResult>
{
    protected CommandValidatorBase()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.When).NotEmpty();
    }
}