using System;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components.Drawables;
using MonoGame.Core.Utils.Extensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Ball : Entity
{
    private const float QuarterPi = float.Pi / 4f;

    private Vector2 _size;
    private Rectangle _bounds;
    private float _speed;
    private Vector2 _initialPosition;
    public Vector2 Dir { get; private set; }

    public override Rectangle BoundingBox => new((int)(Transform.Position.X - _size.X / 2f),
        (int)(Transform.Position.Y - _size.Y / 2f), 
    (int)_size.X, (int)_size.Y);

    public override void Initialise(Game1 game)
    {
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        _size = new Vector2(50, 50);
        _speed = 500;
        _initialPosition = new Vector2(game.GraphicsDevice.Viewport.Width / 2f, game.GraphicsDevice.Viewport.Height / 2f);
        Dir = new Vector2(-1, 1).Normalised();
        Transform.Position = _initialPosition;

        var spriteDrawer = new SpriteDrawer
        {
            SpritePath = "Content/ball.png",
            AnchorPoint = new Vector2(.5f, .5f)
        };

        AddComponent(spriteDrawer);
        base.Initialise(game);
        
        spriteDrawer.Resize(new Vector2(_size.X, _size.Y));
    }

    public override void Update(GameTime gameTime)
    {
        if (Transform.Position.X <= _bounds.X + _size.X / 2f)
            Reflect(normal: Vector2.UnitX);
        
        if (Transform.Position.X >= _bounds.Width - _size.X / 2f)
            Reflect(normal: -Vector2.UnitX);

        if (Transform.Position.Y <= _bounds.Y + _size.Y / 2f)
            Reflect(normal: Vector2.UnitY);

        if (Transform.Position.Y >= _bounds.Height - _size.Y / 2f)
            Reflect(normal: -Vector2.UnitY);
        
        Transform.Position += Dir.Normalised() * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    
    public void Reflect(Vector2 normal)
    {
        Dir = Dir.Reflected(normal);
    }

    public void Reset()
    {
        Transform.Position = _initialPosition;
    }

    public void GetHitBy(IEntity entity, Vector2 normal)
    {
        var halfHeight = entity.BoundingBox.Height / 2f;
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