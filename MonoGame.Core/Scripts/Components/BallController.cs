using System;
using Microsoft.Xna.Framework;
using MonoGame.Data;
using MonoGame.Data.Utils.Extensions;
using Newtonsoft.Json;

namespace MonoGame.Core.Scripts.Components;

public class BallController : Component
{
    private const float QuarterPi = float.Pi / 4f;
    
    [JsonIgnore] public Vector2 InitialPosition { get; private set; }
    [JsonIgnore] public float InitialSpeed { get; private set; }
    public Vector2 Dir { get; set; }
    public float Acceleration { get; set; }
    public float Speed { get; set; }


    public override void Initialise()
    {
        InitialPosition = Transform.Position;
        InitialSpeed = Speed;
    }
    
    public void Reflect(Vector2 normal)
    {
        Dir = Dir.Reflected(normal);
    }

    public void Reset()
    {
        Transform.Position = InitialPosition;
        Speed = InitialSpeed;
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