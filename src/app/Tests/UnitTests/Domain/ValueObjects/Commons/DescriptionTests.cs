using System;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.ValueObjects.Commons;

[TestFixture]
public class DescriptionTests : TestBase
{
    [Test]
    public void CreateDescription_WhenInputValuesIsValid_IsSuccessful()
    {
        // Arrange
        const string title = "description";

        // Act
        Action createDescription = () => Description.Describe(title);

        // Assert
        createDescription.Should().NotThrow();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidDescriptions))]
    public void CreateDescription_WhenInputValuesIsInvalid_ShouldThrow(string value)
    {
        // Arrange
        // Act
        void CreateDescription()
        {
            Description.Describe(value);
        }

        // Assert
        AssertBrokenRule(CreateDescription);
    }

    private class TestData
    {
        public static TestCaseData[] InvalidDescriptions =
        {
            new("Contrary to popular belief, Lorem Ipsum is not simply random text. " +
                "It has roots in a piece of classical Latin literature from 45 BC, " +
                "making it over 2000 years old. Richard McClintock, a Latin professor at " +
                "Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, " +
                "consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature")
        };
    }
}