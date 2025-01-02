using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Scripts.Entities;

public interface IEntity : IDisposable
{
    Vector2 Position { get; }
    Rectangle BoundingBox { get; }
    
    void LoadContent(Game game);
    void Initialise(Game game);
    void Update(GameTime gameTime);
    void Draw();
}
