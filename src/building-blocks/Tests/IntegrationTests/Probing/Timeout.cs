using System;

namespace Budgetter.BuildingBlocks.Testes.IntegrationTests.Probing;

public class Timeout
{
    private readonly DateTime _endTime;

    public Timeout(int duration)
    {
        _endTime = DateTime.Now.AddMilliseconds(duration);
    }

    public bool HasTimedOut()
    {
        return DateTime.Now > _endTime;
    }
}