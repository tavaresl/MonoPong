using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core.Drawing.Sprites;

public class TextureDrawingSystem(Game game) : DrawingGameSystem<DrawableTexture>(game)
{
    private readonly SpriteBatch _spriteBatch = new(game.GraphicsDevice);
    
    public override void Draw(DrawableTexture component, GameTime gameTime)
    {
        if (component.Texture == null) component.CreateTexture();
        _spriteBatch.Begin();
        _spriteBatch.Draw(component.Texture,
            component.Transform.Position, 
            null,
            component.Mask,
            component.Transform.Rotation,
            component.Origin,
            component.Transform.Scale,
            component.Effect,
            0f);
        _spriteBatch.End();
    }
}