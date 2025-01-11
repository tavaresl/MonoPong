using Microsoft.Xna.Framework;
using MonoGame.Data;

namespace MonoGame.Core.Scripts.Components.Paddles;

public class PaddleController : Component
{
    public readonly float MovementSpeed = 300f;
    public IEntity Ball { get; set; }
    public Vector2 Size { get; set; }

    public IPaddleControlStrategy Handler { get; set;  }
}