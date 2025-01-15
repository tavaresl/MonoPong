using System;
using System.Linq;

namespace MonoGame.Data.Collision;

public readonly struct Collision : IEquatable<Collision>
{
    public (Collider, Collider) Colliders { get; init; }

    public Collision(Collider collider, Collider other)
    {
        Colliders = (collider, other);
    }

    public bool Equals(Collision other)
    {
        return (Colliders.Item1.Id == other.Colliders.Item1.Id && Colliders.Item2.Id == other.Colliders.Item2.Id)
            || (Colliders.Item1.Id == other.Colliders.Item2.Id && Colliders.Item2.Id == other.Colliders.Item1.Id);
    }

    public override bool Equals(object obj)
    {
        return obj is Collision other && Equals(other);
    }

    public override int GetHashCode()
    {
        int[] ordered = new [] { Colliders.Item1.Id, Colliders.Item2.Id }.Order().ToArray();
        return HashCode.Combine(ordered[0], ordered[1]);
    }

    public static bool operator ==(Collision left, Collision right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Collision left, Collision right)
    {
        return !(left == right);
    }
}