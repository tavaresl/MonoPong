using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Scripts.Entities;

public interface IEntity : IDisposable
{
    bool Enabled { get; set; }
    Vector2 Position { get; }
    Rectangle BoundingBox { get; }
    
    void Initialise(Game game);
    void LoadContent(Game game);
    void Update(GameTime gameTime);
    void Draw();
}
