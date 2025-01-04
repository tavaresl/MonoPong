using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Scripts.Entities;

public abstract class Entity : IEntity
{
    public bool Enabled { get; set; }
    public Vector2 Position { get; protected set; }
    public abstract Rectangle BoundingBox { get;  }

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