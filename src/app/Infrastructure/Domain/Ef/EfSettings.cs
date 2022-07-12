using Newtonsoft.Json;

namespace Budgetter.Infrastructure.Domain.Ef;

public record EfSettings
{
    [JsonConstructor]
    public EfSettings(
        string mainConnectionString)
    {
        MainConnectionString = mainConnectionString;
    }

    public string MainConnectionString { get; }

    [JsonIgnore] public bool IsEmpty => string.IsNullOrEmpty(MainConnectionString);
}