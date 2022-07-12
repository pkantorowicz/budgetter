using Budgetter.Infrastructure.Domain.Ef;
using Budgetter.Infrastructure.Feeds.Exchanges.Nbp;
using Newtonsoft.Json;

namespace Budgetter.Wpf.Settings;

internal class AppSettings
{
    [JsonConstructor]
    public AppSettings(
        string culture,
        int maxSupportedDaysCount,
        EfSettings dbSettings,
        NpbApiSettings npbApiSettings)
    {
        Culture = culture;
        MaxSupportedDaysCount = maxSupportedDaysCount;
        DbSettings = dbSettings;
        NpbApiSettings = npbApiSettings;
    }

    public string Culture { get; }
    public int MaxSupportedDaysCount { get; }
    public EfSettings DbSettings { get; }
    public NpbApiSettings NpbApiSettings { get; }
}