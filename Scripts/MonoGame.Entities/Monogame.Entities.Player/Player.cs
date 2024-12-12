using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Entities.Monogame.Entities.Player;

public class Player : IEntity
{
    private const float MovementSpeed = 200f;
    
    private Texture2D _texture;
    private Vector2 _position;
    private SpriteBatch _spriteBatch;

    public void LoadContent()
    {
    }

    public void Initialise(Game game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _texture = new Texture2D(game.GraphicsDevice, 20, 20);
        _texture.SetData(Enumerable.Repeat(Color.White, 400).ToArray());
        _position = new Vector2(
            (float)game.GraphicsDevice.Viewport.Width / 2,
            (float)game.GraphicsDevice.Viewport.Height / 2);
    }
    
    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var dir = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.W))
        {
            dir -= Vector2.UnitY;
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            dir -= Vector2.UnitX;
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            dir += Vector2.UnitY;
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            dir += Vector2.UnitX;
        }

        if (dir != Vector2.Zero)
        {
            dir.Normalize();
            _position += dir * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
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

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _texture.Dispose();
        _spriteBatch.Dispose();
    }
}