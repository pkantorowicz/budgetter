using System;
using Budgetter.Domain.Commons.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.ValueObjects.Targets;

[TestFixture]
public class StatusTests
{
    [Test]
    [TestCaseSource(typeof(StatusTestData), nameof(StatusTestData.TestData))]
    public void PredicateTargetStatus_BasedOnInput_ShouldReturnCorrectStatus(
        Duration duration,
        Status status)
    {
        // Arrange
        // Act
        var calculatedStatus = Status.PredicateStatus(duration);

        calculatedStatus.Should().Be(status);
    }

    private class StatusTestData
    {
        public static TestCaseData[] TestData =
        {
            new(Duration.Specify(
                DateTime.UtcNow.AddDays(-20),
                DateTime.UtcNow.AddDays(-5)), Status.Closed),
            new(Duration.Specify(
                DateTime.UtcNow.AddDays(2),
                DateTime.UtcNow.AddDays(20)), Status.WaitingForOpened),
            new(Duration.Specify(
                DateTime.UtcNow.AddDays(-20),
                DateTime.UtcNow.AddDays(5)), Status.Opened)
        };
    }
}