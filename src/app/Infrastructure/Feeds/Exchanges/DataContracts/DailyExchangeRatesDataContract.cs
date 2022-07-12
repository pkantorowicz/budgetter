using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataContracts;

public record DailyExchangeRatesDataContract
{
    [JsonProperty("effectiveDate")] public DateTime EffectiveDate { get; set; }
    [JsonProperty("rates")] public List<ExchangeRateDataContract> Rates { get; set; }
}