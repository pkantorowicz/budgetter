using System;
using System.Collections.Generic;
using System.Linq;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using MediatR;
using NetArchTest.Rules;
using NUnit.Framework;

namespace Budgetter.ArchitectureTests.Application;

[TestFixture]
public class ApplicationTests : TestBase
{
    [Test]
    public void Command_Should_Be_Immutable()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(CommandBase))
            .Or()
            .Inherit(typeof(CommandBase<>))
            .Or()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .GetTypes();

        AssertAreImmutable(types);
    }

    [Test]
    public void Query_Should_Be_Immutable()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That().ImplementInterface(typeof(IQuery<>)).GetTypes();

        AssertAreImmutable(types);
    }

    [Test]
    public void CommandHandler_Should_Have_Name_EndingWith_CommandHandler()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(CommandHandlerBase<>))
            .Or()
            .Inherit(typeof(CommandHandlerBase<,>))
            .And()
            .DoNotHaveNameMatching(".*Decorator.*").Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void QueryHandler_Should_Have_Name_EndingWith_QueryHandler()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(QueryHandlerBase<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void Command_And_Query_Handlers_Should_Not_Be_Public()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(QueryHandlerBase<,>))
            .Or()
            .Inherit(typeof(CommandHandlerBase<>))
            .Or()
            .Inherit(typeof(CommandHandlerBase<,>))
            .Should().NotBePublic().GetResult().FailingTypes;

        AssertFailingTypes(types);
    }

    [Test]
    public void Validator_Should_Have_Name_EndingWith_Validator()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(CommandValidatorBase<>))
            .Or()
            .Inherit(typeof(CommandValidatorBase<,>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void Validators_Should_Not_Be_Public()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(CommandValidatorBase<>))
            .Or()
            .Inherit(typeof(CommandValidatorBase<,>))
            .Should().NotBePublic().GetResult().FailingTypes;

        AssertFailingTypes(types);
    }

    [Test]
    public void MediatR_RequestHandler_Should_NotBe_Used_Directly()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That().DoNotHaveName("ICommandHandler`1")
            .Should().ImplementInterface(typeof(IRequestHandler<>))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var type in types)
        {
            var isCommandHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
            var isCommandWithResultHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
            var isQueryHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            if (!isCommandHandler && !isCommandWithResultHandler && !isQueryHandler) failingTypes.Add(type);
        }

        AssertFailingTypes(failingTypes);
    }

    [Test]
    public void Command_With_Result_Should_Not_Return_Unit()
    {
        var commandWithResultHandlerType = typeof(ICommandHandler<,>);
        IEnumerable<Type> types = Types.InAssembly(ApplicationAssembly)
            .That().ImplementInterface(commandWithResultHandlerType)
            .GetTypes().ToList();

        var failingTypes = new List<Type>();
        foreach (var type in types)
        {
            var interfaceType = type.GetInterface(commandWithResultHandlerType.Name);
            if (interfaceType?.GenericTypeArguments[1] == typeof(Unit)) failingTypes.Add(type);
        }

        AssertFailingTypes(failingTypes);
    }
}