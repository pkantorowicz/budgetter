using System;

namespace Budgetter.BuildingBlocks.Application.Contacts.Base;

public abstract record TraceableMessage
{
    protected TraceableMessage()
    {
        Id = Guid.NewGuid();
        When = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public DateTime When { get; }
}