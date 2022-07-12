using System;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.UnitTests.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.UnitTests.Domain.ValueObjects.Commons;

[TestFixture]
public class TitleTests : TestBase
{
    [Test]
    public void CreateTitle_WhenInputValuesIsValid_IsSuccessful()
    {
        // Arrange
        const string title = "title";

        // Act
        Action createTitle = () => Title.Entitle(title);

        // Assert
        createTitle.Should().NotThrow();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidTitles))]
    public void CreateTitle_WhenInputValuesIsInvalid_ShouldThrow(string value)
    {
        // Arrange
        // Act
        void CreateTitle()
        {
            Title.Entitle(value);
        }

        // Assert
        AssertBrokenRule(CreateTitle);
    }

    private class TestData
    {
        public static TestCaseData[] InvalidTitles =
        {
            new(null),
            new(string.Empty),
            new("KO"),
            new("If you were using a different runner, e.g. the console runner," +
                " you would see an error message for this. Because of how the Test Explorer works, " +
                "you don't see one there although there is probably a log message from NUnit in the output window.")
        };
    }
}