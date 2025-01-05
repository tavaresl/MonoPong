using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core.Scripts.Components.Drawables;

public class TextDrawer : DrawableComponent, IDrawableComponent
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    public Vector2 Size => _font.MeasureString(Text);

    public string Text { get; set; } = string.Empty;
    public string FontName { get; set; } = string.Empty;

    public override void Initialise(Game1 game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _font = game.Content.Load<SpriteFont>(FontName);
        base.Initialise(game);
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
            new Vector2(Size.X * AnchorPoint.X, Size.Y * AnchorPoint.Y), 
            Transform.Scale,
            Effect,
            Layer);
        _spriteBatch.End();
    }
}