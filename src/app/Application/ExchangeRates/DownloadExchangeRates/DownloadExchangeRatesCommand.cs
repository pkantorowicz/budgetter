using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.ExchangeRates.DownloadExchangeRates;

public record DownloadExchangeRatesCommand : CommandBase
{
    public DownloadExchangeRatesCommand(
        DateTime from,
        DateTime to,
        int maxDaysCount)
    {
        From = from;
        To = to;
        MaxDaysCount = maxDaysCount;
    }

    public DateTime From { get; }
    public DateTime To { get; }
    public int MaxDaysCount { get; }
}