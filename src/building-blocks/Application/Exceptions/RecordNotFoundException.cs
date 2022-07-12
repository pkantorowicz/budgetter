namespace Budgetter.BuildingBlocks.Application.Exceptions;

public class RecordNotFoundException : ApplicationException
{
    public RecordNotFoundException(string message) : base(message)
    {
    }
}