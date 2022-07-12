using System;
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
public class DeallocateExpenseTests : TestBase
{
    [Test]
    public void DeallocateExpense_WhenInputValuesAreValid_IsSuccessful()
    {
        // Arrange
        var budgetPlanId = BudgetPlanId.New(Guid.NewGuid());

        var target = Target.CreateBudgetGoal(
            budgetPlanId,
            Title.Entitle(
                "Contrary to popular belief."),
            Description.Describe(
                "Contrary to popular belief, Lorem Ipsum is not simply random text. " +
                "It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old"),
            Duration.Specify(
                new DateTime(2021, 08, 23),
                DateTime.UtcNow.AddDays(8)),
            Duration.Specify(
                new DateTime(2021, 08, 21),
                DateTime.UtcNow.AddDays(8)),
            Money.Of(1500, "PLN"));

        var financeOperationId = FinanceOperationId.New(Guid.NewGuid());
        var financeOperationExistenceChecker = new Mock<IFinanceOperationExistenceChecker>();

        financeOperationExistenceChecker
            .Setup(foe => foe.ExistsAsync(
                financeOperationId.Value,
                budgetPlanId.Value))
            .Returns(Task.FromResult(true));

        target.AllocateExpense(
            financeOperationId,
            Title.Entitle(
                "A piece of classical Latin literature from"),
            Money.Of(120, "PLN"),
            DateTime.UtcNow,
            financeOperationExistenceChecker.Object);

        // Act
        target.DeallocateExpense(financeOperationId);

        // Assert
        var @event = AssertPublishedDomainEvent<ExpenseDeallocatedDomainEvent>(target);

        @event.TargetId.Should().Be(target.Id);
        @event.ExpenseId.Should().Be(financeOperationId.Value);
    }
}