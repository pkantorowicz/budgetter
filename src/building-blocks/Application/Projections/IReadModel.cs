using System;

namespace Budgetter.BuildingBlocks.Application.Projections;

public interface IReadModel
{
    public Guid Id { get; }
}