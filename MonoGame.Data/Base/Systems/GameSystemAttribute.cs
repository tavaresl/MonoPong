using System;

namespace MonoGame.Data;

[AttributeUsage(AttributeTargets.Class)]
public class GameSystemAttribute(Type type) : Attribute
{
    public Type Type { get; private set; } = type;
}