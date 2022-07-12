using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.Domain.FinanceOperations;

public class FinanceOperationType : ValueObject
{
    private FinanceOperationType(string value)
    {
        Value = value;
    }

    public static FinanceOperationType Expense => new(nameof(Expense));
    public static FinanceOperationType Income => new(nameof(Income));

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }
}