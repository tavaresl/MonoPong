using Microsoft.Xna.Framework;
using MonoGame.Core;

namespace MonoGame.Game.Scripts.Components;

public class Paddle : Component
{
    public readonly float MovementSpeed = 300f;
    public IEntity Ball { get; set; }
    public Vector2 Size { get; set; }
    public Vector2 HitDirection { get; set; }
}