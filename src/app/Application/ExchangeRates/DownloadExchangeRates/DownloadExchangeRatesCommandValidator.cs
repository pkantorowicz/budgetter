using FluentValidation;

namespace Budgetter.Application.ExchangeRates.DownloadExchangeRates;

internal class DownloadExchangeRatesCommandValidator : AbstractValidator<DownloadExchangeRatesCommand>
{
    public DownloadExchangeRatesCommandValidator()
    {
        RuleFor(command => command.From)
            .NotEmpty()
            .Must((command, from) => from < command.To && (command.To - from).Days <= command.MaxDaysCount)
            .WithMessage((command, from) => $"Date from: {from} must be lower than date to: {command.To}. " +
                                            $"Nbp api can not supported longer date range like {command.MaxDaysCount}");

        RuleFor(command => command.To)
            .NotEmpty()
            .Must((command, to) => to > command.From && (to - command.From).Days <= command.MaxDaysCount)
            .WithMessage((command, to) => $"Date to: {to} must be greater than date from: {command.From}. " +
                                          $"Nbp api can not supported longer date range like {command.MaxDaysCount}");

        RuleFor(command => command.MaxDaysCount)
            .GreaterThan(0)
            .WithMessage("Max days count must be positive value,");
    }
}