using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core.Scripts.Components.Drawables;

public sealed class SpriteDrawer : DrawableComponent, IDrawableComponent
{
    private Texture2D _sprite;
    private SpriteBatch _spriteBatch;

    public string SpritePath { get; set; } = string.Empty;

    public override void Initialise(Game1 game)
    {
        using var fs = File.OpenRead(SpritePath);
        _sprite = Texture2D.FromStream(game.GraphicsDevice, fs);
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
    }

    public override void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_sprite,
            Transform.Position,
            null,
            Color.White,
            Transform.Rotation,
            new Vector2(_sprite.Width * AnchorPoint.X, _sprite.Height * AnchorPoint.Y),
            Transform.Scale,
            Effect, 
            Layer);
        _spriteBatch.End();
    }

    public void Resize(Vector2 dimensions)
    {
        if (_sprite == null) return;
        if (_sprite.Width != 0 && _sprite.Height != 0)
            Transform.Scale = new Vector2(dimensions.X / _sprite.Width, dimensions.Y / _sprite.Height);
    }

    public override void Dispose()
    {
        _sprite.Dispose();
        _spriteBatch.Dispose();
    }
}