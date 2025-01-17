using System;
using MonoGame.Core;
using MonoGame.Core.Utils.Extensions;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Game.Scripts.Components;

public class Ball : Component
{
    private const float QuarterPi = float.Pi / 4f;
    public Vector2 Dir { get;  set; }
    public float Acceleration { get; set; }
    public float Speed { get; set; }
    
    public void Reflect(Vector2 normal)
    {
        Dir = Dir.Reflected(normal);
    }

    public void GetHitBy(IEntity entity, Vector2 normal)
    {
        const int halfHeight = 40;
        var yDist = Math.Abs(Transform.Position.Y - entity.Transform.Position.Y);
        var clampDist = Math.Clamp(yDist, 0f, halfHeight);
        var factor = clampDist / halfHeight;
        var angle = float.Lerp(0, QuarterPi, factor);

        if (normal.X > 0 && Transform.Position.Y < entity.Transform.Position.Y 
            || normal.X < 0 && Transform.Position.Y > entity.Transform.Position.Y)
        {
            angle = -angle;
        }
        
        Dir = Vector2.Rotate(normal, angle);
    }
}