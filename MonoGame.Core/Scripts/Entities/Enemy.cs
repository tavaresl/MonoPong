using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core.Utils.Extensions;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Enemy : Entity
{
    private const float MovementSpeed = 300f;
    
    private Texture2D _texture = null!;
    private SpriteBatch _spriteBatch = null!;

    private Rectangle _bounds;
    private bool _previouslyIntersectedBall;
    public Ball Ball { get; init; }
    
    public override Rectangle BoundingBox => new((int)(Transform.Position.X - _texture.Width / 2f), 
        (int)(Transform.Position.Y - _texture.Height / 2f), _texture.Width, _texture.Height);

    public override void Initialise(Game game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _texture = new Texture2D(game.GraphicsDevice, 20, 80);
        _texture.SetData(Enumerable.Repeat(Color.White, _texture.Width * _texture.Height).ToArray());
        Transform.Position = new Vector2(game.GraphicsDevice.Viewport.Width - 80, game.GraphicsDevice.Viewport.Height / 2f);
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
    }

    public override void Update(GameTime gameTime)
    {
        var dir = Vector2.Zero;
        
        if (Ball.Transform.Position.Y <= Transform.Position.Y - _texture.Height / 2f) dir -= Vector2.UnitY;
        if (Ball.Transform.Position.Y >= Transform.Position.Y + _texture.Height / 2f) dir += Vector2.UnitY;

        if (dir != Vector2.Zero)
            Transform.Position += dir.Normalised() * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Transform.Position.Y - _texture.Height / 2f <= 0)
            Transform.Position = new Vector2(Transform.Position.X, _texture.Height / 2f);
        else if (Transform.Position.Y + _texture.Height / 2f >= _bounds.Height)
            Transform.Position = new Vector2(Transform.Position.X, _bounds.Height - _texture.Height / 2f);
        
        if (Ball.BoundingBox.Intersects(BoundingBox))
        {
            if (!_previouslyIntersectedBall) Ball.GetHitBy(this, -Vector2.UnitX);    
            _previouslyIntersectedBall = true;
        }
        else
        {
            _previouslyIntersectedBall = false;
        }
    }

    public override void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture,
            Transform.Position, 
            null,
            Color.White,
            Transform.Rotation,
            new Vector2(
                (float)_texture.Width / 2,
                (float)_texture.Height / 2),
            Transform.Scale,
            SpriteEffects.None,
            0f);
        _spriteBatch.End();
    }
    
    public override void Dispose()
    {
        _texture.Dispose();
        _spriteBatch.Dispose();
    }
}