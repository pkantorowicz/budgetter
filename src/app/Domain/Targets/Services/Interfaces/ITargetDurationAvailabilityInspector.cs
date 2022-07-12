using Budgetter.Domain.Commons.ValueObjects;

namespace Budgetter.Domain.Targets.Services.Interfaces;

public interface ITargetDurationAvailabilityInspector
{
    bool InspectIsTargetDurationMatchedPlanDuration(
        Duration targetDuration,
        Duration planDuration);

    bool IsTargetExpired(
        Duration targetDuration);
}