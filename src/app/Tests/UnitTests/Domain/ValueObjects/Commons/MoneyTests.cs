using System;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.ValueObjects.Commons;

[TestFixture]
public class MoneyTests : TestBase
{
    private const string Currency = "PLN";

    [Test]
    public void MoneyOf_WhenInputValuesIsValid_IsSuccessful()
    {
        // Arrange
        const decimal value = 500;

        // Act
        Action of = () => Money.Of(value, Currency);

        // Assert
        of.Should().NotThrow();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidTitles))]
    public void MoneyOperator_ShouldReturnCorrectOutput(
        OperationsType operationsType,
        decimal @decimal,
        Money money)
    {
        switch (operationsType)
        {
            case OperationsType.LeftDecimalRightMoneyGreaterThan:
                _ = (@decimal > money).Should().BeTrue();
                break;
            case OperationsType.LeftDecimalRightMoneyGreaterOrEqualsThan:
                _ = (@decimal >= money).Should().BeTrue();
                break;
            case OperationsType.LeftDecimalRightMoneyLowerThan:
                _ = (@decimal < money).Should().BeTrue();
                break;
            case OperationsType.LeftDecimalRightMoneyLowerOrEqualsThan:
                _ = (@decimal <= money).Should().BeTrue();
                break;
            case OperationsType.LeftMoneyRightDecimalGreaterThan:
                _ = (money > @decimal).Should().BeTrue();
                break;
            case OperationsType.LeftMoneyRightDecimalGreaterOrEqualsThan:
                _ = (money >= @decimal).Should().BeTrue();
                break;
            case OperationsType.LeftMoneyRightDecimalLowerThan:
                _ = (money < @decimal).Should().BeTrue();
                break;
            case OperationsType.LeftMoneyRightDecimalLowerOrEqualsThan:
                _ = (money <= @decimal).Should().BeTrue();
                break;
            case OperationsType.LeftMoneyRightMoneyMinus:
                _ = (money - Money.Of(@decimal, Currency)).Should().Be(Money.Of(500, Currency));
                break;
            case OperationsType.LeftMoneyRightMoneyPlus:
                _ = (money + Money.Of(@decimal, Currency)).Should().Be(Money.Of(500, Currency));
                break;
            case OperationsType.LeftMoneyRightMoneyMultiple:
                _ = (money * Money.Of(@decimal, Currency)).Should().Be(Money.Of(500, Currency));
                break;
            case OperationsType.LeftMoneyRightMoneyDivide:
                _ = (money / Money.Of(@decimal, Currency)).Should().Be(Money.Of(500, Currency));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operationsType), operationsType,
                    "Unknown Operation type.");
        }
    }

    public enum OperationsType
    {
        LeftDecimalRightMoneyGreaterThan,
        LeftDecimalRightMoneyGreaterOrEqualsThan,
        LeftDecimalRightMoneyLowerThan,
        LeftDecimalRightMoneyLowerOrEqualsThan,
        LeftMoneyRightDecimalGreaterThan,
        LeftMoneyRightDecimalGreaterOrEqualsThan,
        LeftMoneyRightDecimalLowerThan,
        LeftMoneyRightDecimalLowerOrEqualsThan,
        LeftMoneyRightMoneyMinus,
        LeftMoneyRightMoneyPlus,
        LeftMoneyRightMoneyMultiple,
        LeftMoneyRightMoneyDivide
    }

    private class TestData
    {
        public static TestCaseData[] InvalidTitles =
        {
            new(OperationsType.LeftDecimalRightMoneyGreaterThan, (decimal)120, Money.Of(100, Currency)),
            new(OperationsType.LeftDecimalRightMoneyGreaterOrEqualsThan, (decimal)100.0, Money.Of(100, Currency)),
            new(OperationsType.LeftDecimalRightMoneyLowerThan, (decimal)100.0, Money.Of(120, Currency)),
            new(OperationsType.LeftDecimalRightMoneyLowerOrEqualsThan, (decimal)100, Money.Of(100, Currency)),
            new(OperationsType.LeftMoneyRightDecimalGreaterThan, (decimal)100.0, Money.Of(120, Currency)),
            new(OperationsType.LeftMoneyRightDecimalGreaterOrEqualsThan, (decimal)100, Money.Of(100, Currency)),
            new(OperationsType.LeftMoneyRightDecimalLowerThan, (decimal)120, Money.Of(100, Currency)),
            new(OperationsType.LeftMoneyRightDecimalLowerOrEqualsThan, (decimal)100, Money.Of(100, Currency)),
            new(OperationsType.LeftMoneyRightMoneyMinus, (decimal)250, Money.Of(750, Currency)),
            new(OperationsType.LeftMoneyRightMoneyPlus, (decimal)250, Money.Of(250, Currency)),
            new(OperationsType.LeftMoneyRightMoneyMultiple, (decimal)50, Money.Of(10, Currency)),
            new(OperationsType.LeftMoneyRightMoneyDivide, (decimal)10, Money.Of(5000, Currency))
        };
    }
}