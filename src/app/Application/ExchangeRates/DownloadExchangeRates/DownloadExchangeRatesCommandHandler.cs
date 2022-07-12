using System.Threading;
using System.Threading.Tasks;
using Budgetter.Application.Configuration.WebClients;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;

namespace Budgetter.Application.ExchangeRates.DownloadExchangeRates;

internal class DownloadExchangeRatesCommandHandler : CommandHandlerBase<DownloadExchangeRatesCommand>
{
    private readonly IExchangeRatesClient _exchangeRatesClient;

    public DownloadExchangeRatesCommandHandler(
        IAggregateStore aggregateStore,
        IExchangeRatesClient exchangeRatesClient) : base(aggregateStore)
    {
        _exchangeRatesClient = exchangeRatesClient;
    }

    public override async Task<ICommandResult> Handle(DownloadExchangeRatesCommand command,
        CancellationToken cancellationToken)
    {
        await _exchangeRatesClient.DownloadExchangeRatesAsync(command.From, command.To);

        return CommandResult.Success();
    }
}