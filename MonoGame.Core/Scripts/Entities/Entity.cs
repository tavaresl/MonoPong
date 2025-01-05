using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;

namespace MonoGame.Core.Scripts.Entities;

public abstract class Entity : IEntity
{
    public bool Enabled { get; set; }
    public string Name { get; set; }
    public Transform Transform { get; protected set; }

    public abstract Rectangle BoundingBox { get; }
    public IList<IComponent> Components { get; } = new List<IComponent>();

    protected Entity()
    {
        Transform = new Transform
        {
            Entity = this,
            Position = Vector2.Zero,
            Scale = Vector2.One,
            Rotation = 0f
        };
        Name = GetType().Name;
    }
    
    public void AddComponent(IComponent component)
    {
        component.Entity = this;
        Components.Add(component);
    }

    public bool RemoveComponent(IComponent component)
    {
        component.Entity = null;
        component.Dispose();
        return Components.Remove(component);
    }

    public T GetComponent<T>() where T : IComponent
    {
        var component = Components.First(c => c is T);
        return (T)component;
    }

    public T GetComponent<T>(string name) where T : IComponent
    {
        var component = Components.First(c => c.Name == name);
        return (T)component;
    }

    public virtual void Initialise(Game1 game)
    {
        foreach (var component in Components)
        {
            component.Initialise(game);
        }
    }

    public virtual void LoadContent(Game1 game)
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw()
    {
    }
    
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}