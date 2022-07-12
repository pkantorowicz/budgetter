using System.Collections.Generic;

namespace Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;

public class StreamEfDao
{
    public StreamEfDao()
    {
        Messages = new List<MessagesEfDao>();
    }

    public string Id { get; set; }
    public string IdOriginal { get; set; }
    public int IdInternal { get; set; }
    public int Version { get; set; }
    public int Position { get; set; }
    public int? MaxAge { get; set; }
    public int? MaxCount { get; set; }
    public IEnumerable<MessagesEfDao> Messages { get; set; }
}