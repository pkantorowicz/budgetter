using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Exceptions;

namespace Budgetter.Domain.Commons.ValueObjects;

public class Title : ValueObject
{
    private Title(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BusinessRuleDoesNotMatchedException(
                "Value can not be null or empty.");

        if (value.Length is < 3 or > 60)
            throw new BusinessRuleDoesNotMatchedException(
                "Value can not be lower than 3 characters and greater than 60");

        Value = value;
    }

    public string Value { get; }

    public static Title Entitle(string value)
    {
        return new Title(value);
    }
}