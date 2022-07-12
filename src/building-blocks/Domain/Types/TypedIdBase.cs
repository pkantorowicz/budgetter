using System;

namespace Budgetter.BuildingBlocks.Domain.Types;

public abstract class TypedIdBase : IEquatable<TypedIdBase>, ITypedId
{
    protected TypedIdBase(Guid value)
    {
        if (value == Guid.Empty) throw new InvalidOperationException("Id value cannot be empty!");

        Value = value;
    }

    public bool Equals(TypedIdBase other)
    {
        return Value == other?.Value;
    }

    public Guid Value { get; }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;

        return obj is TypedIdBase other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(TypedIdBase obj1, TypedIdBase obj2)
    {
        if (Equals(obj1, null))
        {
            if (Equals(obj2, null)) return true;

            return false;
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(TypedIdBase x, TypedIdBase y)
    {
        return !(x == y);
    }
}