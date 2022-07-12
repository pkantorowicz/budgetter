using Newtonsoft.Json;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataContracts;

public record ExchangeRateDataContract
{
    [JsonProperty("code")] public string Code { get; set; }
    [JsonProperty("currency")] public string Currency { get; set; }
    [JsonProperty("mid")] public double Rate { get; set; }
}