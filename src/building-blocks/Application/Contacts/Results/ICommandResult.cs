namespace Budgetter.BuildingBlocks.Application.Contacts.Results;

public interface ICommandResult : IResult
{
}

public interface ICommandResult<out T> : IResult
{
    T Data { get; }
}