using Budgetter.BuildingBlocks.Application.Contacts.Base;

namespace Budgetter.BuildingBlocks.Application.Contacts.Queries;

public abstract record QueryBase<TResult> : TraceableMessage, IQuery<TResult>;