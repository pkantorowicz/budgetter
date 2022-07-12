using System;

namespace Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;

public class MessagesEfDao
{
    public string EventId { get; private set; }
    public int StreamIdInternal { get; set; }
    public int StreamVersion { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Type { get; set; }
    public string JsonData { get; set; }
    public string JsonMetaData { get; set; }
    public StreamEfDao Stream { get; set; }
}