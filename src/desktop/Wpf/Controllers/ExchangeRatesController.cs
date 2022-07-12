using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.ExchangeRates.DownloadExchangeRates;
using Budgetter.Application.ExchangeRates.GetAvailableCurrencies;
using Budgetter.Wpf.Controllers.Base;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Settings;

namespace Budgetter.Wpf.Controllers;

internal class ExchangeRatesController : ControllerBase, IEchangeRatesController
{
    private readonly ISettingsProvider _settingsProvider;

    public ExchangeRatesController(ISettingsProvider settingsProvider)
    {
        _settingsProvider = settingsProvider;
    }

    public async Task<IEnumerable<string>> GetAvailableCurrencies()
    {
        var availableCurrencies = await QueryAsync(
            new GetAvailableCurrenciesQuery());

        return availableCurrencies;
    }

    public async Task DownloadExchangeRates(DateTime startDate, DateTime endDate)
    {
        await SendAsync(new DownloadExchangeRatesCommand(
            startDate,
            endDate,
            _settingsProvider.GetNbpApiSettings().MaxRecordCount));
    }
}