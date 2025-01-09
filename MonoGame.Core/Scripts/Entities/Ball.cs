using System;
using Microsoft.Xna.Framework;
using MonoGame.Data;
using MonoGame.Data.Utils.Extensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Ball : Entity
{
    private const float QuarterPi = float.Pi / 4f;
    private const float InitialSpeed = 512f;
    public float Radius = 16f;
    private const float Acceleration = 4f;

    private Vector2 Size => new (Radius * 2);
    private Rectangle _bounds;
    private float _speed;
    private Vector2 _initialPosition;
    public Vector2 Dir { get; private set; }

    public override Rectangle BoundingBox => new((int)(Transform.Position.X - Size.X / 2f),
        (int)(Transform.Position.Y - Size.Y / 2f), 
    (int)Size.X, (int)Size.Y);

    public override void Initialise(Game game)
    {
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        _initialPosition = new Vector2(game.GraphicsDevice.Viewport.Width / 2f, game.GraphicsDevice.Viewport.Height / 2f);
        Dir = new Vector2(-1, 1).Normalised();
        Reset();
        base.Initialise(game);
    }

    public override void Update(GameTime gameTime)
    {
        if (Transform.Position.X <= _bounds.X + Size.X / 2f)
            Reflect(normal: Vector2.UnitX);
        
        if (Transform.Position.X >= _bounds.Width - Size.X / 2f)
            Reflect(normal: -Vector2.UnitX);

        if (Transform.Position.Y <= _bounds.Y + Size.Y / 2f)
            Reflect(normal: Vector2.UnitY);

        if (Transform.Position.Y >= _bounds.Height - Size.Y / 2f)
            Reflect(normal: -Vector2.UnitY);
        
        Transform.Position += Dir.Normalised() * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _speed += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    
    public void Reflect(Vector2 normal)
    {
        Dir = Dir.Reflected(normal);
    }

    public void Reset()
    {
        Transform.Position = _initialPosition;
        _speed = InitialSpeed;
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