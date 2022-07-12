using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Exceptions;
using Budgetter.Domain.Commons.Services;
using Budgetter.Infrastructure.Domain.Ef;
using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;
using Microsoft.EntityFrameworkCore;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.DataAccessors;

internal class EfExchangeRatesAccessor : IExchangeRatesProvider, IExchangeRatesWriteAccessor
{
    private const string DefaultExchangeCode = "PLN";

    private readonly BudgetterDbContext _budgetterDbContext;
    private readonly DbSet<DailyExchangeRatesEfDao> _dailyExchangeRatesDbSet;
    private readonly DbSet<ExchangeRateEfDao> _exchangeRateDbSet;

    public EfExchangeRatesAccessor(BudgetterDbContext budgetterDbContext)
    {
        _budgetterDbContext = budgetterDbContext;

        _exchangeRateDbSet = _budgetterDbContext.Set<ExchangeRateEfDao>();
        _dailyExchangeRatesDbSet = _budgetterDbContext.Set<DailyExchangeRatesEfDao>();
    }

    public async Task<IDictionary<string, double>> GetExchangeRateAsync(string effectiveDate, List<string> codes)
    {
        var dailyExchangeRate = _dailyExchangeRatesDbSet
            .Include(der => der.ExchangeRates)
            .AsEnumerable()
            .FirstOrDefault(e =>
                DateTime.Parse(e.EffectiveDate, CultureInfo.InvariantCulture) <=
                DateTime.Parse(effectiveDate, CultureInfo.InvariantCulture));

        WhenRecordDoesNotExistsThenThrow(
            dailyExchangeRate,
            effectiveDate);

        var exchangeRatesDictionary = new Dictionary<string, double>();

        codes.ForEach(code =>
        {
            var exchangeRate = dailyExchangeRate?.ExchangeRates
                .FirstOrDefault(er => er.Code == code);

            if (exchangeRate is not null)
                exchangeRatesDictionary.Add(code, exchangeRate.Rate);

            if (code == DefaultExchangeCode)
                exchangeRatesDictionary.Add(code, 1);
        });

        if (exchangeRatesDictionary.Count != codes.Count)
            throw new RecordNotFoundException(
                "Unable to convert expense currency to plan currency, because rates are not available in database.");

        return await Task.FromResult(exchangeRatesDictionary);
    }

    public async Task SaveExchangeRatesAsync(List<DailyExchangeRatesEfDao> dailyExchangeRates)
    {
        await _budgetterDbContext.Database.BeginTransactionAsync();

        var existingRates = await _dailyExchangeRatesDbSet.ToListAsync();

        _dailyExchangeRatesDbSet.RemoveRange(existingRates);

        await _budgetterDbContext.SaveChangesAsync();

        foreach (var dailyExchangeRatesDao in dailyExchangeRates)
        {
            await _dailyExchangeRatesDbSet.AddAsync(dailyExchangeRatesDao);
            await _exchangeRateDbSet.AddRangeAsync(dailyExchangeRatesDao.ExchangeRates);
        }

        await _budgetterDbContext.SaveChangesAsync();
        await _budgetterDbContext.Database.CommitTransactionAsync();
    }

    private static void WhenRecordDoesNotExistsThenThrow(DailyExchangeRatesEfDao dailyExchangeRatesEf,
        string effectiveDate)
    {
        if (dailyExchangeRatesEf is null || !dailyExchangeRatesEf.ExchangeRates.Any())
            throw new RecordNotFoundException(
                $"Unable to get exchange rate for date: {effectiveDate}.");
    }
}