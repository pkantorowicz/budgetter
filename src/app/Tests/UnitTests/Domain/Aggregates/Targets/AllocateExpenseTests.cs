using System;
using System.Linq;
using System.Threading.Tasks;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.Plans;
using Budgetter.Domain.Targets;
using Budgetter.Domain.Targets.DomainEvents;
using Budgetter.Domain.Targets.Services.Interfaces;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.Aggregates.Targets;

[TestFixture]
public class AllocateExpenseTests : TestBase
{
    [Test]
    public void AllocateExpense_WhenInputValuesAreValid_IsSuccessful()
    {
        // Arrange
        var target = Target.CreateBudgetGoal(
            BudgetPlanId.New(Guid.NewGuid()),
            Title.Entitle(
                "Contrary to popular belief."),
            Description.Describe(
                "Contrary to popular belief, Lorem Ipsum is not simply random text. " +
                "It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old"),
            Duration.Specify(
                new DateTime(2021, 08, 20),
                DateTime.UtcNow.AddDays(8)),
            Duration.Specify(
                new DateTime(2021, 08, 19),
                DateTime.UtcNow.AddDays(9)),
            Money.Of(1500, "PLN"));

        var financeOperationId = FinanceOperationId.New(Guid.NewGuid());

        var expenseTitle = Title.Entitle(
            "A piece of classical Latin literature from");

        var expenseUnitPrice = Money.Of(120, "PLN");
        var expenseOccurredAt = DateTime.UtcNow;
        var financeOperationExistenceChecker = new Mock<IFinanceOperationExistenceChecker>();

        financeOperationExistenceChecker
            .Setup(foe => foe.ExistsAsync(
                financeOperationId.Value,
                It.IsAny<Guid>()))
            .Returns(Task.FromResult(true));

        // Act
        target.AllocateExpense(
            financeOperationId,
            expenseTitle,
            expenseUnitPrice,
            expenseOccurredAt,
            financeOperationExistenceChecker.Object);

        // Assert
        var @event = AssertPublishedDomainEvent<ExpenseAllocatedDomainEvent>(target);

        @event.TargetId.Should().Be(target.Id);
        @event.ExpenseId.Should().Be(financeOperationId.Value);
        @event.Title.Should().Be(expenseTitle.Value);
        @event.UnitPrice.Should().Be(expenseUnitPrice.Value);
        @event.Currency.Should().Be(expenseUnitPrice.Currency);
        @event.OccurredAt.Should().Be(expenseOccurredAt);

        var snapshot = target.GetSnapshot();

        snapshot.Balance.Value.Should().Be(TargetBalance.Safe.Value);
        snapshot.Status.Value.Should().Be(Status.Opened.Value);
        Money.Of(snapshot.Expenses.Sum(e => e.UnitPrice.Value), "PLN").Should().Be(expenseUnitPrice);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidDates))]
    public void AllocateExpense_WhenExpenseDoNotMatchDurationRange_ShouldThrowBrokenRule(DateTime occurredAt)
    {
        // Arrange
        var target = Target.CreateBudgetGoal(
            BudgetPlanId.New(Guid.NewGuid()),
            Title.Entitle(
                "Contrary to popular belief."),
            Description.Describe(
                "Contrary to popular belief, Lorem Ipsum is not simply random text. " +
                "It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old"),
            Duration.Specify(
                new DateTime(2021, 08, 22),
                DateTime.UtcNow.AddDays(6)),
            Duration.Specify(
                new DateTime(2021, 08, 20),
                DateTime.UtcNow.AddDays(8)),
            Money.Of(1500, "PLN"));

        var financeOperationId = FinanceOperationId.New(Guid.NewGuid());
        var financeOperationExistenceChecker = new Mock<IFinanceOperationExistenceChecker>();

        financeOperationExistenceChecker
            .Setup(foe => foe.ExistsAsync(
                financeOperationId.Value,
                It.IsAny<Guid>()))
            .Returns(Task.FromResult(true));

        // Act
        void AllocateExpense()
        {
            target.AllocateExpense(
                financeOperationId,
                Title.Entitle(
                    "A piece of classical Latin literature from"),
                Money.Of(120, "PLN"),
                occurredAt,
                financeOperationExistenceChecker.Object);
        }

        // Assert
        AssertBrokenRule(AllocateExpense);
    }

    private class TestData
    {
        public static TestCaseData[] InvalidDates =
        {
            new(new DateTime(2021, 08, 20).AddDays(-1)),
            new(DateTime.UtcNow.AddDays(9))
        };
    }
}