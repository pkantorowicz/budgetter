using System;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Exceptions;

namespace Budgetter.Domain.Commons.ValueObjects;

public class Duration : ValueObject
{
    private const int _expirationTolerance = 500;

    private Duration(
        DateTime validFrom,
        DateTime validTo)
    {
        if (validFrom >= validTo)
            throw new BusinessRuleDoesNotMatchedException(
                $"Target start date: {validFrom} have to be greater than target end date: {validTo}.");

        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }

    public static Duration Specify(
        DateTime validFrom,
        DateTime validTo)
    {
        return new Duration(validFrom, validTo);
    }

    public int DaysRangeCount()
    {
        return (ValidTo - ValidFrom).Days;
    }

    public bool IsNotStartedYet(DateTime dateTime)
    {
        return ValidFrom > dateTime;
    }

    public bool IsExpired(DateTime dateTime)
    {
        return ValidTo < dateTime.AddMilliseconds(_expirationTolerance);
    }

    public bool IsInRange(DateTime dateTime)
    {
        return dateTime < ValidTo && dateTime > ValidFrom;
    }
}