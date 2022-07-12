using Budgetter.BuildingBlocks.Application.Contacts.Base;

namespace Budgetter.BuildingBlocks.Application.Contacts.Commands;

public abstract record CommandBase : TraceableMessage, ICommand;

public abstract record CommandBase<TResult> : TraceableMessage, ICommand<TResult>;