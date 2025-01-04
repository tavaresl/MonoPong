using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Score : Entity
{
    private Rectangle _bounds;
    private SpriteFont _font;
    private SpriteBatch _spriteBatch;
    
    public int PlayerPoints { get; set; }
    public int EnemyPoints { get; set; }

    private string Text => $"{PlayerPoints} - {EnemyPoints}";
    private Vector2 Size => _font.MeasureString(Text);
    public override Rectangle BoundingBox => new(
        (int)(Transform.Position.X - Size.X / 2),
        (int)(Transform.Position.Y - Size.Y / 2),
        (int)Size.X,
        (int)Size.Y / 2);

    public override void LoadContent(Game game)
    {
        _font = game.Content.Load<SpriteFont>("Fonts/Font");
    }

    public override void Initialise(Game game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _bounds = game.GraphicsDevice.Viewport.Bounds;
    }

    public override void Update(GameTime gameTime)
    {
        Transform.Position = new Vector2(_bounds.Width / 2f, 20f);
    }

    public override void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(
            _font, 
            Text,
            Transform.Position,
            Color.White, 
            Transform.Rotation, 
            Size / 2, 
            Transform.Scale,
            SpriteEffects.None, 
            0f);
        _spriteBatch.End();
    }

    public override void Dispose()
    {
        _spriteBatch.Dispose();
    }
}