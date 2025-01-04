using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core.Utils.Extensions;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Ball : Entity
{
    private const float QuarterPi = float.Pi / 4f;

    private Texture2D _texture = null!;
    private SpriteBatch _spriteBatch = null!;
    private Point _size;
    private Rectangle _bounds;
    private float _speed;
    private Vector2 _initialPosition;
    public Vector2 Dir { get; private set; }

    public override Rectangle BoundingBox =>
        new((int)(Transform.Position.X - _size.X / 2f), (int)(Transform.Position.Y - _size.Y / 2f), _size.X, _size.Y);
    
    public override void LoadContent(Game game)
    {
        using var fs = File.OpenRead("Content/ball.png");
        _texture = Texture2D.FromStream(game.GraphicsDevice, fs);
    }

    public override void Initialise(Game game)
    {
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _size = new Point(50, 50);
        _speed = 500;
        _initialPosition = new Vector2(game.GraphicsDevice.Viewport.Width / 2f, game.GraphicsDevice.Viewport.Height / 2f);
        Dir = new Vector2(-1, 1).Normalised();
        Transform.Position = _initialPosition;
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

    public override void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(
            _texture,
            new Rectangle((int)Transform.Position.X, (int)Transform.Position.Y, _size.X, _size.Y),
            new Rectangle(0, 0, _texture.Width, _texture.Height),
            Color.White,
            0f,
            new Vector2(_texture.Width / 2f, _texture.Height / 2f),
            SpriteEffects.None, 
            0f);
        _spriteBatch.End();
    }
    
    public override void Dispose()
    {
        _texture.Dispose();
        _spriteBatch.Dispose();
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