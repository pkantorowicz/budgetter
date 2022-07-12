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
public class CreateNewTargetTests : TestBase
{
    [Test]
    public void CreateNewTarget_WhenInputValuesAreValid_IsSuccessful()
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
            DateTime.UtcNow.AddDays(6));

        var planDuration = Duration.Specify(
            new DateTime(2021, 08, 19),
            DateTime.UtcNow.AddDays(9));


        var targetMaxAmount = Money.Of(2500, "PLN");

        // Act
        var target = Target.CreateBudgetGoal(
            planId,
            targetTitle,
            targetDescription,
            targetDuration,
            planDuration,
            targetMaxAmount);

        // Assert
        var @event = AssertPublishedDomainEvent<TargetCreatedDomainEvent>(target);

        @event.TargetId.Should().Be(target.Id);
        @event.BudgetPlanId.Should().Be(planId.Value);
        @event.Title.Should().Be(targetTitle.Value);
        @event.Description.Should().Be(targetDescription.Value);
        @event.MaxAmount.Should().Be(targetMaxAmount.Value);
        @event.Currency.Should().Be(targetMaxAmount.Currency);
        @event.ValidFrom.Should().Be(targetDuration.ValidFrom);
        @event.ValidTo.Should().Be(targetDuration.ValidTo);
    }

    [Test]
    public void CreateNewTarget_WhenInputStatus_ShouldThrowBrokenRule()
    {
        // Arrange
        // Act
        static void CreateBudgetGoal()
        {
            Target.CreateBudgetGoal(
                BudgetPlanId.New(Guid.NewGuid()),
                Title.Entitle(
                    "Contrary to popular belief."),
                Description.Describe(
                    "Contrary to popular belief, Lorem Ipsum is not simply random text. " +
                    "It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old"),
                Duration.Specify(
                    new DateTime(2021, 08, 20),
                    new DateTime(2021, 09, 01)),
                Duration.Specify(
                    new DateTime(2021, 08, 19),
                    DateTime.UtcNow.AddDays(2)),
                Money.Of(2500, "PLN"));
        }

        // Assert
        AssertBrokenRule(CreateBudgetGoal);
    }
}