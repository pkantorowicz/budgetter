using System;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;
using Budgetter.Domain.Targets;
using Budgetter.Domain.Targets.DomainEvents;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.Aggregates.Targets;

[TestFixture]
public class ChangeTargetAttributeTests : TestBase
{
    [Test]
    public void ChangeTargetAttributes_WhenInputValuesAreValid_IsSuccessful()
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
                new DateTime(2021, 08, 25),
                DateTime.UtcNow.AddDays(4)),
            Duration.Specify(
                new DateTime(2021, 08, 23),
                DateTime.UtcNow.AddDays(6)),
            Money.Of(2500, "PLN"));

        var targetTitle = Title.Entitle(
            "Piece of classical Latin");

        var targetDescription = Description.Describe(
            "A piece of classical Latin literature from 45 BC, maki. " +
            "It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old");

        var targetDuration = Duration.Specify(
            new DateTime(2021, 08, 28),
            DateTime.UtcNow.AddDays(5));

        var planDuration = Duration.Specify(
            new DateTime(2021, 08, 24),
            DateTime.UtcNow.AddDays(6));

        var targetMaxAmount = Money.Of(2500, "PLN");

        // Act
        target.ChangeAttributes(
            targetTitle,
            targetDescription,
            targetDuration,
            planDuration,
            targetMaxAmount);

        // Assert
        var @event = AssertPublishedDomainEvent<TargetAttributesChangedDomainEvent>(target);

        @event.TargetId.Should().Be(target.Id);
        @event.Title.Should().Be(targetTitle.Value);
        @event.Description.Should().Be(targetDescription.Value);
        @event.ValidFrom.Should().Be(targetDuration.ValidFrom);
        @event.ValidTo.Should().Be(targetDuration.ValidTo);
        @event.MaxAmount.Should().Be(targetMaxAmount.Value);
        @event.Currency.Should().Be(targetMaxAmount.Currency);
    }

    [Test]
    public void ChangeTargetAttributes_WhenStatusIsClosed_IsSuccessful()
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
                new DateTime(2021, 08, 27),
                DateTime.UtcNow.AddDays(1)),
            Duration.Specify(
                new DateTime(2021, 08, 23),
                DateTime.UtcNow.AddDays(6)),
            Money.Of(2500, "PLN"));

        var targetTitle = Title.Entitle(
            "Piece of classical Latin");

        var targetDescription = Description.Describe(
            "A piece of classical Latin literature from 45 BC, maki. " +
            "It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old");

        var targetDuration = Duration.Specify(
            new DateTime(2021, 08, 20),
            DateTime.UtcNow.AddDays(-2));

        var planDuration = Duration.Specify(
            new DateTime(2021, 08, 24),
            DateTime.UtcNow.AddDays(4));

        var targetMaxAmount = Money.Of(2500, "PLN");

        // Act
        void ChangeAttributes()
        {
            target.ChangeAttributes(
                targetTitle,
                targetDescription,
                targetDuration,
                planDuration,
                targetMaxAmount);
        }

        // Assert
        AssertBrokenRule(ChangeAttributes);
    }
}