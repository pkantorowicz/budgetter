using Newtonsoft.Json;

namespace Budgetter.Infrastructure.Feeds.Exchanges.Nbp;

public record NpbApiSettings
{
    [JsonConstructor]
    public NpbApiSettings(
        string apiUrl,
        int timeout,
        int maxRecordCount)
    {
        ApiUrl = apiUrl;
        Timeout = timeout;
        MaxRecordCount = maxRecordCount;
    }

    public string ApiUrl { get; }
    public int Timeout { get; }
    public int MaxRecordCount { get; }

    [JsonIgnore]
    public bool IsEmpty =>
        string.IsNullOrEmpty(ApiUrl) ||
        Timeout == default ||
        MaxRecordCount == default;
}