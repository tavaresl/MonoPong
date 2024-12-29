using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core.Utils.Extensions;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Enemy : IEntity
{
    private const float MovementSpeed = 200f;
    
    private Texture2D _texture = null!;
    private SpriteBatch _spriteBatch = null!;
    private Vector2 _position;
    private Rectangle _bounds;

    public Ball Ball { get; set; }

    public void Dispose()
    {
        _texture.Dispose();
        _spriteBatch.Dispose();
    }

    public void LoadContent(Game game)
    {
    }

    public void Initialise(Game game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _texture = new Texture2D(game.GraphicsDevice, 20, 80);
        _texture.SetData(Enumerable.Repeat(Color.White, _texture.Width * _texture.Height).ToArray());
        _position = new Vector2(game.GraphicsDevice.Viewport.Width - 80, game.GraphicsDevice.Viewport.Height / 2f);
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
    }

    public void Update(GameTime gameTime)
    {
        var dir = Vector2.Zero;
        
        if (Ball.Position.Y + Ball.BoundingBox.Height / 2f <= _position.Y - _texture.Height / 2f) dir -= Vector2.UnitY;
        if (Ball.Position.Y + Ball.BoundingBox.Height / 2f >= _position.Y + _texture.Height / 2f) dir += Vector2.UnitY;

        if (dir != Vector2.Zero)
            _position += dir.Normalised() * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_position.Y - _texture.Height / 2f <= 0)
            _position = new Vector2(_position.X, _texture.Height / 2f);
        else if (_position.Y + _texture.Height / 2f >= _bounds.Height)
            _position = new Vector2(_position.X, _bounds.Height - _texture.Height / 2f);

        if (Ball.BoundingBox.X + Ball.BoundingBox.Width >= _position.X - _texture.Width / 2f)
        {
            if (Ball.Position.Y >= _position.Y - _texture.Height / 2f &&
                Ball.Position.Y <= _position.Y + _texture.Height / 2f)
            {
                Ball.Reflect(normal: -Vector2.UnitX);
            }
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture,
            _position, 
            null,
            Color.White,
            0f,
            new Vector2(
                (float)_texture.Width / 2,
                (float)_texture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f);
        _spriteBatch.End();
    }
}