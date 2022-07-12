using System;
using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.Domain.Commons.ValueObjects;

public class Status : ValueObject
{
    private Status(int id, string value)
    {
        Id = id;
        Value = value;
    }

    public static Status WaitingForOpened => new(1, nameof(WaitingForOpened));
    public static Status Opened => new(2, nameof(Opened));
    public static Status Closed => new(3, nameof(Closed));

    public int Id { get; }
    public string Value { get; }

    public static Status PredicateStatus(Duration duration)
    {
        Status status;
        var currentDateTime = SystemClock.Now;

        if (duration.IsInRange(currentDateTime))
            status = Opened;
        else if (duration.IsNotStartedYet(currentDateTime))
            status = WaitingForOpened;
        else if (duration.IsExpired(currentDateTime))
            status = Closed;
        else
            throw new ArgumentOutOfRangeException(nameof(currentDateTime), currentDateTime,
                "Unable to specify target status, because current date time value is invalid.");

        return status;
    }
}