using System;
using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.Domain.Targets;

public class TargetBalance : ValueObject
{
    private TargetBalance(int id, string value)
    {
        Id = id;
        Value = value;
    }

    public static TargetBalance HighExceeded => new(1, nameof(HighExceeded));
    public static TargetBalance Exceeded => new(2, nameof(Exceeded));
    public static TargetBalance LittleExceeded => new(3, nameof(LittleExceeded));
    public static TargetBalance Safe => new(4, nameof(Safe));

    public int Id { get; }
    public string Value { get; }

    public static TargetBalance CalculateBalance(decimal totalAmount, decimal maxAmount)
    {
        var percents = totalAmount / maxAmount * 100;

        return percents switch
        {
            < 100 => Safe,
            > 100 and < 110 => LittleExceeded,
            > 110 and < 130 => Exceeded,
            > 130 => HighExceeded,
            _ => throw new ArgumentOutOfRangeException(nameof(percents), percents,
                "Unable to specify target balance, because percents value is unknown.")
        };
    }
}