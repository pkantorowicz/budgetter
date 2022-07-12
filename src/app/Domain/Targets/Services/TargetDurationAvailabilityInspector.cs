using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Targets.Services.Interfaces;

namespace Budgetter.Domain.Targets.Services;

public class TargetDurationAvailabilityInspector : ITargetDurationAvailabilityInspector
{
    public bool InspectIsTargetDurationMatchedPlanDuration(Duration targetDuration, Duration planDuration)
    {
        var isMatched = planDuration.IsInRange(targetDuration.ValidFrom) &&
                        planDuration.IsInRange(targetDuration.ValidTo);

        return isMatched;
    }

    public bool IsTargetExpired(Duration targetDuration)
    {
        return targetDuration.IsExpired(SystemClock.Now);
    }
}