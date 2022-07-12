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
public class ChangeExpenseAttributesTests : TestBase
{
    [Test]
    public void ChangeExpenseAttributes_WhenInputValuesAreValid_IsSuccessful()
    {
        // Arrange
        var occurredAt = new DateTime(2021, 08, 20);

        var financeOperation = FinanceOperation.Do(
            BudgetPlanId.New(Guid.NewGuid()),
            Title.Entitle(
                "Contrary to popular belief, Lorem"),
            Money.Of(-500, "PLN"),
            "PLN",
            occurredAt,
            FinanceOperationType.Expense,
            Duration.Specify(occurredAt.AddDays(-20), occurredAt.AddDays(10)),
            new Mock<IExchangeRatesProvider>().Object);

        var title = Title.Entitle(
            "Richard McClintock, a Latin professor");

        var price = Money.Of(-200, "PLN");

        // Act
        financeOperation.ChangeAttributes(
            title,
            price.Value);

        // Assert
        var @event = AssertPublishedDomainEvent<FinanceOperationAttributesChangedDomainEvent>(financeOperation);

        @event.Title.Should().Be(title.Value);
        @event.Price.Should().Be(price.Value);
    }
}