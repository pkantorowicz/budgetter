using System;

namespace Budgetter.BuildingBlocks.Testes.IntegrationTests.Probing;

public class AssertErrorException : Exception
{
    public AssertErrorException(string message)
        : base(message)
    {
    }
}