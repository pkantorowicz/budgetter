using System;
using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.Domain.Commons.ValueObjects;

public class Money : ValueObject
{
    private Money(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    public decimal Value { get; }
    public string Currency { get; }

    public static Money Of(decimal value, string currency)
    {
        return new Money(value, currency);
    }

    public static Money Of(decimal value, string currency, double rate)
    {
        var calculatedValue = Math.Round(value * Convert.ToDecimal(rate), 2);

        return new Money(calculatedValue, currency);
    }

    public static bool operator >(decimal left, Money right)
    {
        return left > right.Value;
    }

    public static bool operator <(decimal left, Money right)
    {
        return left < right.Value;
    }

    public static bool operator >=(decimal left, Money right)
    {
        return left >= right.Value;
    }

    public static bool operator <=(decimal left, Money right)
    {
        return left <= right.Value;
    }

    public static bool operator >(Money left, decimal right)
    {
        return left.Value > right;
    }

    public static bool operator <(Money left, decimal right)
    {
        return left.Value < right;
    }

    public static bool operator >=(Money left, decimal right)
    {
        return left.Value >= right;
    }

    public static bool operator <=(Money left, decimal right)
    {
        return left.Value <= right;
    }

    public static Money operator -(Money left, Money right)
    {
        return Of(left.Value - right.Value, left.Currency);
    }

    public static Money operator +(Money left, Money right)
    {
        return Of(left.Value + right.Value, left.Currency);
    }

    public static Money operator *(Money left, Money right)
    {
        return Of(left.Value * right.Value, left.Currency);
    }

    public static Money operator /(Money left, Money right)
    {
        return Of(left.Value / right.Value, left.Currency);
    }
}