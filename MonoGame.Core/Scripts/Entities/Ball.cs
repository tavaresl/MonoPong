using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core.Utils.Extensions;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Core.Scripts.Entities;

public class Ball : IEntity
{
    private const float QuarterPi = float.Pi / 4f;

    private Texture2D _texture = null!;
    private SpriteBatch _spriteBatch = null!;
    private Point _size;
    private Rectangle _bounds;
    private float _speed;
    private Vector2 _initialPosition;
    public Vector2 Position { get; private set; }
    public Vector2 Dir { get; private set; }

    public Rectangle BoundingBox =>
        new((int)(Position.X - _size.X / 2f), (int)(Position.Y - _size.Y / 2f), _size.X, _size.Y);
    
    public void LoadContent(Game game)
    {
        using var fs = File.OpenRead("Content/ball.png");
        _texture = Texture2D.FromStream(game.GraphicsDevice, fs);
    }

    public void Initialise(Game game)
    {
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _size = new Point(50, 50);
        
        _speed = 500;
        _initialPosition =new Vector2(game.GraphicsDevice.Viewport.Width / 2f, game.GraphicsDevice.Viewport.Height / 2f);
        Dir = new Vector2(-1, 1).Normalised();
        Position = _initialPosition;
    }

    public void Update(GameTime gameTime)
    {
        if (Position.X <= _bounds.X + _size.X / 2f)
            Reflect(normal: Vector2.UnitX);
        
        if (Position.X >= _bounds.Width - _size.X / 2f)
            Reflect(normal: -Vector2.UnitX);

        if (Position.Y <= _bounds.Y + _size.Y / 2f)
            Reflect(normal: Vector2.UnitY);

        if (Position.Y >= _bounds.Height - _size.Y / 2f)
            Reflect(normal: -Vector2.UnitY);
        
        Position += Dir.Normalised() * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(
            _texture,
            new Rectangle((int)Position.X, (int)Position.Y, _size.X, _size.Y),
            new Rectangle(0, 0, _texture.Width, _texture.Height),
            Color.White,
            0f,
            new Vector2(_texture.Width / 2f, _texture.Height / 2f),
            SpriteEffects.None, 
            0f);
        _spriteBatch.End();
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _texture.Dispose();
        _spriteBatch.Dispose();
    }
    
    public void Reflect(Vector2 normal)
    {
        Dir = Dir.Reflected(normal);
    }

    public void Reset()
    {
        Position = _initialPosition;
    }

    public void GetHitBy(IEntity entity, Vector2 normal)
    {
        var halfHeight = entity.BoundingBox.Height / 2f;
        var yDist = Math.Abs(Position.Y - entity.Position.Y);
        var clampDist = Math.Clamp(yDist, 0f, halfHeight);
        var factor = clampDist / halfHeight;
        var angle = float.Lerp(0, QuarterPi, factor);

        if (normal.X > 0 && Position.Y < entity.Position.Y || normal.X < 0 && Position.Y > entity.Position.Y)
        {
            angle = -angle;
        }
        
        Dir = Vector2.Rotate(normal, angle);
    }
}