using System;

namespace Budgetter.BuildingBlocks.Domain;

public class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}