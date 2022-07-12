using FluentValidation;

namespace Budgetter.Application.Commons.Validators;

public class DescriptionValidator : AbstractValidator<string>
{
    public DescriptionValidator()
    {
        const int max = 300;

        RuleFor(description => description)
            .Must(c => c.Length < max)
            .WithMessage(description =>
                $"Description: {description} cannot contains more than {max} characters.");
    }
}