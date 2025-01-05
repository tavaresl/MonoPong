using System;
using MonoGame.Core.Scripts.Entities;

namespace MonoGame.Core.Scripts.Components;

public abstract class Component : IComponent
{
    public string Name { get; set; }
    public IEntity Entity { get; set; }
    protected Transform Transform => Entity.Transform;

    protected Component()
    {
        Name = GetType().Name;
    }
    
    public virtual void Initialise(Game1 game)
    {
    }
    
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}