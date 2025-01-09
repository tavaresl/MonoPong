using Microsoft.Xna.Framework;
using MonoGame.Data;

namespace MonoGame.Core.Scripts.Components.Paddles;

public class PaddleController : Component
{
    public readonly float MovementSpeed = 300f;
    public IEntity Ball { get; set; }
    public Vector2 Size { get; set; }

    public IPaddleControlStrategy Handler { get; set;  }
    
    public Rectangle BoundingBox => new((int)(Entity.Transform.Position.X - Size.X / 2), 
        (int)(Entity.Transform.Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
}