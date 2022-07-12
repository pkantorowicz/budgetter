using Budgetter.Domain.Targets;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.ValueObjects.Targets;

[TestFixture]
public class TargetBalanceTests
{
    [Test]
    [TestCaseSource(typeof(BalanceTestData), nameof(BalanceTestData.TestData))]
    public void CalculateTargetBalance_BasedOnInput_ShouldReturnCorrectStatus(
        decimal totalAmount,
        decimal maxAmount,
        TargetBalance targetBalance)
    {
        // Arrange
        // Act
        var balance = TargetBalance.CalculateBalance(totalAmount, maxAmount);

        balance.Should().Be(targetBalance);
    }

    private class BalanceTestData
    {
        public static TestCaseData[] TestData =
        {
            new(Decimal(850), Decimal(500), TargetBalance.HighExceeded),
            new(Decimal(645), Decimal(500), TargetBalance.Exceeded),
            new(Decimal(545), Decimal(500), TargetBalance.LittleExceeded),
            new(Decimal(490), Decimal(500), TargetBalance.Safe)
        };
    }

    private static decimal Decimal(int value)
    {
        return new decimal(value);
    }
}