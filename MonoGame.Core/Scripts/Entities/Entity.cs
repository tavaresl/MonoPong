using System;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;

namespace MonoGame.Core.Scripts.Entities;

public abstract class Entity : IEntity
{
    public bool Enabled { get; set; }
    public Transform Transform { get; protected set; }

    public abstract Rectangle BoundingBox { get; }

    protected Entity()
    {
        Transform = new Transform
        {
            Entity = this,
            Position = Vector2.Zero,
            Scale = Vector2.One,
            Rotation = 0f
        };
    }

    public virtual void Initialise(Game game)
    {
    }

    public virtual void LoadContent(Game game)
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