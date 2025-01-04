using System;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;

namespace MonoGame.Core.Scripts.Entities;

public interface IEntity : IDisposable
{
    bool Enabled { get; set; }
    public Transform Transform { get; }
    Rectangle BoundingBox { get; }
    
    void Initialise(Game game);
    void LoadContent(Game game);
    void Update(GameTime gameTime);
    void Draw();
}
