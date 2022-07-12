using System;

namespace Budgetter.BuildingBlocks.Domain.Types;

public interface ITypedId
{
    public Guid Value { get; }
}