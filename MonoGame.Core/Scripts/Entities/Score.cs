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
        var position = new Vector2(
            _bounds.Width / 2f,
            20f);
        var text = $"{PlayerPoints} - {EnemyPoints}";
        var size = _font.MeasureString(text);

        _spriteBatch.Begin();
        _spriteBatch.DrawString(
            _font, 
            text,
            position,
            Color.White, 
            0f, 
            size / 2, 
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