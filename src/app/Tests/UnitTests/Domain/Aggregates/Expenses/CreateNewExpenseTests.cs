using System;
using Budgetter.Domain.Commons.Services;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.FinanceOperations.DomainEvents;
using Budgetter.Domain.Plans;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.Aggregates.Expenses;

[TestFixture]
public class CreateNewExpenseTests : TestBase
{
    [Test]
    public void CreateNewExpense_WhenInputValuesAreValid_IsSuccessful()
    {
        // Arrange
        var title = Title.Entitle(
            "Contrary to popular belief, Lorem Ipsum is not");

        var planId = BudgetPlanId.New(Guid.NewGuid());
        var price = Money.Of(500, "PLN");
        var occurredAt = new DateTime(2021, 08, 20);
        var planCurrency = "PLN";
        var type = FinanceOperationType.Income;
        var exchangeRatesAccessor = new Mock<IExchangeRatesProvider>();
        var planDuration = Duration.Specify(occurredAt.AddDays(-20), occurredAt.AddDays(10));

        // Act
        var financeOperation = FinanceOperation.Do(
            planId,
            title,
            price,
            planCurrency,
            occurredAt,
            type,
            planDuration,
            exchangeRatesAccessor.Object);

        // Assert
        var @event = AssertPublishedDomainEvent<FinanceOperationCreatedDomainEvent>(financeOperation);

        @event.FinanceOperationId.Should().Be(financeOperation.Id);
        @event.BudgetPlanId.Should().Be(planId.Value);
        @event.Title.Should().Be(title.Value);
        @event.Price.Should().Be(price.Value);
        @event.Currency.Should().Be(price.Currency);
        @event.OccurredAt.Should().Be(occurredAt);
    }
}