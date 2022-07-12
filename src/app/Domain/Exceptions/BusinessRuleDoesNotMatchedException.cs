using Budgetter.BuildingBlocks.Domain;

namespace Budgetter.Domain.Exceptions;

public class BusinessRuleDoesNotMatchedException : DomainException
{
    public BusinessRuleDoesNotMatchedException(string message) : base(message)
    {
    }
}