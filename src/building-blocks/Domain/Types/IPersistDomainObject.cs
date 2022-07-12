using System;

namespace Budgetter.BuildingBlocks.Domain.Types;

public interface IPersistDomainObject
{
    Guid Id { get; }
}