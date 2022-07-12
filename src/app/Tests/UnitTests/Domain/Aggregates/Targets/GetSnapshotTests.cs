using System;
using System.Linq;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;
using Budgetter.Domain.Targets;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.Aggregates.Targets;

[TestFixture]
public class GetSnapshotTests : TestBase
{
    [Test]
    public void GetSnapshot_ShouldReturnValidValues()
    {
        // Arrange
        var planId = BudgetPlanId.New(Guid.NewGuid());

        var targetTitle = Title.Entitle(
            "Contrary to popular belief.");

        var targetDescription = Description.Describe(
            "Contrary to popular belief, Lorem Ipsum is not simply random text. " +
            "It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old");

        var targetDuration = Duration.Specify(
            new DateTime(2021, 08, 20),
            DateTime.UtcNow.AddDays(3));

        var planDuration = Duration.Specify(
            new DateTime(2021, 08, 19),
            DateTime.UtcNow.AddDays(5));

        var targetMaxAmount = Money.Of(2500, "PLN");

        var target = Target.CreateBudgetGoal(
            planId,
            targetTitle,
            targetDescription,
            targetDuration,
            planDuration,
            targetMaxAmount);

        // Act
        var targetSnapshot = target.GetSnapshot();

        // Assert
        targetSnapshot.TargetId.Should().BeEquivalentTo(
            TargetId.New(target.Id));

        targetSnapshot.BudgetPlanId.Should().BeEquivalentTo(planId);
        targetSnapshot.Title.Should().Be(targetTitle);
        targetSnapshot.Description.Should().Be(targetDescription);
        targetSnapshot.MaxAmount.Should().Be(targetMaxAmount);
        targetSnapshot.Duration.Should().Be(targetDuration);

        targetSnapshot.Status.Should().Be(
            Status.PredicateStatus(targetDuration));

        targetSnapshot.Balance.Should().Be(
            TargetBalance.CalculateBalance(
                ExpensesList.Create().GetTotalAmount().Value,
                targetMaxAmount.Value));

        targetSnapshot.Expenses.Count().Should().Be(
            ExpensesList.Create().Count());
    }
}