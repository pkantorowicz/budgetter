using System;
using System.Linq;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Exceptions;
using FluentAssertions;

namespace Budgetter.UnitTests.Base;

public abstract class TestBase
{
    protected static T AssertPublishedDomainEvent<T>(AggregateRoot aggregate)
        where T : IDomainEvent
    {
        var domainEvent = aggregate
            .GetDomainEvents()
            .OfType<T>()
            .SingleOrDefault();

        if (domainEvent == null) throw new Exception($"{typeof(T).Name} event not published");

        return domainEvent;
    }

    protected static void AssertBrokenRule(Action action)
    {
        action
            .Should()
            .Throw<BusinessRuleDoesNotMatchedException>();
    }
}