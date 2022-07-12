using System;

namespace Budgetter.BuildingBlocks.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class IgnoreMemberAttribute : Attribute
{
}