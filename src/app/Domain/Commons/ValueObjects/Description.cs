using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Exceptions;

namespace Budgetter.Domain.Commons.ValueObjects;

public class Description : ValueObject
{
    private Description(string value)
    {
        if (value.Length > 300)
            throw new BusinessRuleDoesNotMatchedException(
                "Value can not be lower than 3 characters and greater than 50");

        Value = value;
    }

    public string Value { get; }

    public static Description Describe(string value)
    {
        return new Description(value);
    }
}