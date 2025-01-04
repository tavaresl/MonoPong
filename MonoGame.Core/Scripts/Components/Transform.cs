using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Entities;

namespace MonoGame.Core.Scripts.Components;

public record Transform
{
    public IEntity Entity { get; init; }
    public Vector2 Position { get; set; }
    public Vector2 Scale { get; set; }
    public float Rotation { get; set; }
}