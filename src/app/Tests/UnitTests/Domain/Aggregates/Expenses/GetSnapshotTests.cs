using System;
using Budgetter.Domain.Commons.Services;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.Plans;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.Aggregates.Expenses;

[TestFixture]
public class GetSnapshotTests : TestBase
{
    [Test]
    public void GetSnapshot_ShouldReturnValidValues()
    {
        // Arrange
        var planId = BudgetPlanId.New(Guid.NewGuid());
        var title = Title.Entitle(
            "Contrary to popular belief, Lorem Ipsum is not simply");

        var price = Money.Of(-500, "PLN");
        var planCurrency = "PLN";
        var occurredAt = new DateTime(2021, 08, 20);
        var type = FinanceOperationType.Expense;
        var exchangeRatesAccessor = new Mock<IExchangeRatesProvider>();
        var planDuration = Duration.Specify(occurredAt.AddDays(-20), occurredAt.AddDays(10));

        var financeOperation = FinanceOperation.Do(
            planId,
            title,
            price,
            planCurrency,
            occurredAt,
            type,
            planDuration,
            exchangeRatesAccessor.Object);

        // Act
        var expenseSnapshot = financeOperation.GetSnapshot();

        // Assert
        expenseSnapshot.FinanceOperationId.Should().BeEquivalentTo(FinanceOperationId.New(financeOperation.Id));
        expenseSnapshot.BudgetPlanId.Should().BeEquivalentTo(planId);
        expenseSnapshot.Title.Should().Be(title);
        expenseSnapshot.Price.Should().Be(price);
        expenseSnapshot.OccurredAt.Should().Be(occurredAt);
    }
}