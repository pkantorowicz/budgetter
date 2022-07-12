using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.Configuration.WebClients;
using Budgetter.BuildingBlocks.Application.Exceptions;
using Budgetter.BuildingBlocks.Domain.Extensions;
using Budgetter.BuildingBlocks.Infrastructure.WebServices;
using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.DataAccessors;
using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;
using Budgetter.Infrastructure.Feeds.Exchanges.DataContracts;
using RestEase;
using Serilog;

namespace Budgetter.Infrastructure.Feeds.Exchanges.Nbp;

internal class NbpExchangeRatesClient : IExchangeRatesClient
{
    private readonly IExchangeRatesWriteAccessor _exchangeRatesWriteAccessor;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly INbpExchangeRatesApi _nbpExchangeRatesApi;
    private readonly NpbApiSettings _npbApiSettings;

    public NbpExchangeRatesClient(
        IExchangeRatesWriteAccessor exchangeRatesWriteAccessor,
        ILogger logger,
        IMapper mapper,
        NpbApiSettings npbApiSettings)
    {
        _exchangeRatesWriteAccessor = exchangeRatesWriteAccessor;
        _logger = logger;
        _mapper = mapper;
        _npbApiSettings = npbApiSettings;

        var nbpExchangeRatesApiHttpClientHandler =
            BudgetterHttpClientHandler.CreateHttpClient(
                npbApiSettings.ApiUrl,
                npbApiSettings.Timeout,
                _logger);

        _nbpExchangeRatesApi = RestClient.For<INbpExchangeRatesApi>(nbpExchangeRatesApiHttpClientHandler);
    }

    public async Task DownloadExchangeRatesAsync(DateTime from, DateTime to)
    {
        try
        {
            if ((to - from).Days > _npbApiSettings.MaxRecordCount)
                throw new ArgumentException(
                    $"Response content predicated based on startDate: {from} and endDate: " +
                    $"{to} is too long to fetch. Please reduce a date range to solve this problem.");

            var convertedStartDate = from.ToYmd();
            var convertedEndDate = to.ToYmd();

            var exchangeRates = await _nbpExchangeRatesApi.FetchExchangeRates(
                "A",
                convertedStartDate,
                convertedEndDate);

            var exchangeRatesAsList = exchangeRates?.ToList();

            if (exchangeRatesAsList is null || !exchangeRatesAsList.Any())
            {
                _logger.Warning(
                    "NBP Web API returns empty list of expense rates.");

                throw new RecordNotFoundException(
                    $"NBP Web API returns empty list of expense rates for range - from: {from} and to: {to}.");
            }

            await _exchangeRatesWriteAccessor
                .SaveExchangeRatesAsync(
                    _mapper.Map<List<DailyExchangeRatesDataContract>, List<DailyExchangeRatesEfDao>>(
                        exchangeRatesAsList));
        }
        catch (Exception)
        {
            _logger.Warning(
                "NBP Web API is unavailable or something went wrong when trying to get exchange rates.");

            throw;
        }
    }
}