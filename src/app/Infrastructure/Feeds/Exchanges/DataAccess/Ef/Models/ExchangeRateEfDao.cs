using System;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;

public class ExchangeRateEfDao
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string DailyRate { get; set; }
    public string Currency { get; set; }
    public double Rate { get; set; }
    public DailyExchangeRatesEfDao DailyExchangeRateEf { get; set; }
}