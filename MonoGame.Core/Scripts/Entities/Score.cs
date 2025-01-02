using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Score : IEntity
{
    private Rectangle _bounds;
    private SpriteFont _font;
    private SpriteBatch _spriteBatch;
    
    public int PlayerPoints { get; set; }
    public int EnemyPoints { get; set; }

    private string Text => $"{PlayerPoints} - {EnemyPoints}";
    private Vector2 Size => _font.MeasureString(Text);
    public Vector2 Position => new (_bounds.Width / 2f, 20f);
    public Rectangle BoundingBox => new(
        (int)(Position.X - Size.X / 2),
        (int)(Position.Y - Size.Y / 2),
        (int)Size.X,
        (int)Size.Y / 2);

    public void LoadContent(Game game)
    {
        _font = game.Content.Load<SpriteFont>("Fonts/Font");
    }

    public void Initialise(Game game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _bounds = game.GraphicsDevice.Viewport.Bounds;
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw()
    {

        _spriteBatch.Begin();
        _spriteBatch.DrawString(
            _font, 
            Text,
            Position,
            Color.White, 
            0f, 
            Size / 2, 
            Vector2.One,
            SpriteEffects.None, 
            0f);
        _spriteBatch.End();
    }

    public void Dispose()
    {
        _spriteBatch.Dispose();
    }
}